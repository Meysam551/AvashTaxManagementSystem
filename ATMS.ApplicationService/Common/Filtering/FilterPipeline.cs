
using System.Text.Json;

namespace ATMS.ApplicationService;

public sealed class FilterPipeline<T>
{
    private readonly IReadOnlyDictionary<string, ICustomFilter<T>> _filters;

    public FilterPipeline(IEnumerable<ICustomFilter<T>> filters)
    {
        _filters = filters.ToDictionary(x => x.Key);
    }

    public IQueryable<T> Apply(
        IQueryable<T> query,
        IReadOnlyDictionary<string, JsonElement>? tags)
    {
        if (tags is null)
            return query;

        foreach (var tag in tags)
        {
            if (_filters.TryGetValue(tag.Key, out var filter))
            {
                query = filter.Apply(query, tag.Value);
            }
        }

        return query;
    }
}

