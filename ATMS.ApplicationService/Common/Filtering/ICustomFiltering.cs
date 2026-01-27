
using System.Text.Json;

namespace ATMS.ApplicationService;

public interface ICustomFilter<T>
{
    string Key { get; }
    IQueryable<T> Apply(IQueryable<T> query, JsonElement value);
}
