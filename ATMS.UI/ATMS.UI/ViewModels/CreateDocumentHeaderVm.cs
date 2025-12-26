using System.ComponentModel.DataAnnotations;

namespace ATMS.UI.ViewModels;

public class CreateDocumentHeaderVm
{
    [Required(ErrorMessage = "تاریخ سند الزامی است")]
    public DateTime DocumentDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "شماره سند الزامی است")]
    [StringLength(50, ErrorMessage = "حداکثر 50 کاراکتر")]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "نوع سند الزامی است")]
    public int DocumentTypeId { get; set; }

    [StringLength(500, ErrorMessage = "حداکثر 500 کاراکتر")]
    public string Description { get; set; } = string.Empty;
}
