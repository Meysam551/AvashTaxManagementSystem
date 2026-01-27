
using System.Text.Json;

namespace ATMS.ApplicationService;

public sealed class YearFilter : ICustomFilter<TaxRecord>
{
    public string Key => "year";

    public IQueryable<TaxRecord> Apply(
        IQueryable<TaxRecord> query,
        JsonElement value)
    {
        if (value.ValueKind != JsonValueKind.Number)
            return query;

        var year = value.GetInt32();
        return query.Where(x => x.Year == year);
    }
}

