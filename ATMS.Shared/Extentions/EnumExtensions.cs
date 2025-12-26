
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ATMS.Shared;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Name ?? value.ToString();
    }

    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Description ?? string.Empty;
    }

    public static List<EnumItemDto> ToList<T>() where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Select(e => new EnumItemDto
            {
                Id = Convert.ToInt32(e),
                Title = e.GetDisplayName(),
                Description = e.GetDescription()
            })
            .ToList();
    }
}

public class EnumItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}