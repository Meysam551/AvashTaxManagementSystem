
using ATMS.Shared.Enums;

namespace ATMS.Shared.Dtos;

public class DocumentCoverDto
{
    public Guid Id { get; set; }
    public int FiscalYear { get; set; }
    public DateOnly DocumentDate { get; set; }
    public DocumentType DocumentType { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? PostedAt { get; set; }
    public int? DocumentNumber { get; set; }
}