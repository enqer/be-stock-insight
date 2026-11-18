using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using StockInsight.Infrastructure.Providers.Browser;
using StockInsight.Infrastructure.Providers.Financial.Models;

namespace StockInsight.Infrastructure.Providers.Financial;

public class FinancialProvider(IOptions<FinancialProviderOptions> options, PlaywrightBrowser browser) : IFinancialProvider
{
    private static readonly Dictionary<string, string> _tickerOverrides = new()
    {
        ["regnon"] = "REG",
    };

    public async Task<IReadOnlyList<CompanyDto>> FetchCompaniesAsync()
    {
        var url = options.Value.CompaniesUrl;
        await using var session = await browser.OpenAsync(url);
        await session.Page.WaitForSelectorAsync("table");

        var scraped = await session.Page.EvaluateAsync<JsonElement>("""
            () => {
              const out = [];
              const reDMY = /\b(\d{2})[-./](\d{2})[-./](\d{4})\b/;
              const reISO = /\b(\d{4})-(\d{2})-(\d{2})\b/;

              const isCompanyLink = x => /\/gpw\/[^,]+,notowania/.test(x.getAttribute('href') || '');
              const table = [...document.querySelectorAll('table')]
                .find(t => [...t.querySelectorAll('a[href]')].some(isCompanyLink));
              if (!table) throw new Error('Financial table is missing');

              for (const tr of table.querySelectorAll('tr')) {
                const a = [...tr.querySelectorAll('a[href]')].find(isCompanyLink);
                if (!a) continue;

                const m = /\/gpw\/([^,]+),notowania/.exec(a.getAttribute('href'));
                const name = m && m[1].trim();
                if (!name || name.toLowerCase() === 'indeks') continue;

                let date = null;
                for (const td of tr.querySelectorAll('td,th')) {
                  const t = (td.textContent || '').replace(/\s+/g, ' ').trim();
                  let r;
                  if (!date && (r = reDMY.exec(t))) date = `${r[3]}-${r[2]}-${r[1]}`;
                  if (!date && (r = reISO.exec(t))) date = `${r[1]}-${r[2]}-${r[3]}`;
                }

                if (!date) {
                  const cells = [...tr.querySelectorAll('td,th')].map(td => td.textContent.trim());
                  throw new Error(`Missing latest update date for ${name}: ${JSON.stringify(cells)}`);
                }

                out.push({ slug: name, lastFinancialUpdate: date });
              }
              return out;
            }
            """);

        return Parse<IReadOnlyList<CompanyDto>>(scraped);
    }

    public async Task<CompanyDetailsDto> FetchCompanyAsync(string slug)
    {
        var url = string.Format(CultureInfo.InvariantCulture, options.Value.StatementUrl, slug);
        await using var session = await browser.OpenAsync(url);
        await session.Page.WaitForSelectorAsync(".table-wrapper.financial-data");

        var scraped = await session.Page.EvaluateAsync<JsonElement>("""
            () => {
              const warnings = [];
              let wrap = null;
              for (const d of document.querySelectorAll('div[class]')) {
                const kl = d.getAttribute('class') || '';
                if (kl.includes('financial-data') && !kl.includes('factors-data')) { wrap = d; break; }
              }
              if (!wrap) throw new Error('Missing container financial-data');

              const kl = wrap.getAttribute('class') || '';
              const isBank = kl.includes('banks-data') && !kl.includes('not-banks');

              const tab = wrap.querySelector('table');
              if (!tab) throw new Error('Missing tag: <table> in container');
              if (!(tab.innerText || tab.textContent || '').includes('Data raportu')) {
                throw new Error("Missing report table");
              }

              const tickEl = document.querySelector('.cTick');
              const ticker = tickEl ? (tickEl.textContent || '').trim() : '';
              if (!ticker) warnings.push('Missing .cTick — ticker not found');

              const sectorLink = document.querySelector('#BreadCrumbs a[href*="/gpw/sektory/"]');
              const [macroSector, sectorName] = (sectorLink?.textContent ?? '').split('»').map(x => x.trim());
              const sector = macroSector && sectorName ? { macroSector, sector: sectorName } : null;

              const bodyText = document.body.innerText || document.body.textContent || '';
              const isinMatch = bodyText.match(/ISIN\s*:?\s*([A-Z]{2}[A-Z0-9]{9}\d)\b/);
              const isin = isinMatch ? isinMatch[1] : '';
              if (!isin) warnings.push('Missing ISIN symbol');

              const h1 = document.querySelector('h1');
              const name = ((h1 && h1.textContent) || (document.title || '').split(/[-–|(]/)[0] || '').split('-')[0].trim();
              if (!name) warnings.push('Missing name');
              const text = (c) => {
                const k = c.cloneNode(true);
                k.querySelectorAll('br').forEach(br => br.replaceWith(' '));
                return (k.textContent || '').replace(/\u2212/g, '-').replace(/\s+/g, ' ').trim();
              };

              const grid = [];
              for (const tr of tab.querySelectorAll('tr')) {
                const cells = Array.from(tr.querySelectorAll('td,th')).map(text);
                if (!cells.length || !cells.some(x => x)) continue;
                if (cells.length > 2 && cells[1] === '') cells.splice(1, 1);
                grid.push(cells);
              }
              if (!grid.length) throw new Error('The table is empty');

              const rows = [];
              let pending = '';
              for (const r of grid) {
                if (r.length > 8) {
                  rows.push([(pending ? pending + ' ' + r[0] : r[0]).trim(), ...r.slice(1)]);
                  pending = '';
                } else if (r.length === 1 && r[0] && r[0].length < 60 && !/\d/.test(r[0])) {
                  pending = r[0];
                }
              }
              if (!rows.length) return { name, isin, ticker, isBank, sector, statements: [], warnings };

              const columns = [];
              rows[0].forEach((cell, i) => {
                const m = /Q([1-4])\s*(\d{4})/.exec(cell);
                if (m) columns.push({ index: i, period: m[0] });
              });
              if (!columns.length) throw new Error('Header without periods');
              const labels = rows.slice(1).filter(r => r.length > 0
                  && !r[0].toLowerCase().startsWith('dynamika'));

              const statements = columns.map(c => ({
                period: c.period,
                items: labels.map(r => ({
                  label: r[0],
                  value: c.index < r.length ? r[c.index] : null,
                })),
              }));

              return { name, isin, ticker, isBank, sector, statements, warnings };
            }
            """);

        var details = Parse<CompanyDetailsDto>(scraped);
        return _tickerOverrides.TryGetValue(slug, out var corrected) ? details with { Ticker = corrected } : details;
    }

    private static T Parse<T>(JsonElement json) =>
        json.Deserialize<T>(JsonSerializerOptions.Web) ?? throw new InvalidDataException($"Invalid data input fetching {typeof(T).Name}");
}
