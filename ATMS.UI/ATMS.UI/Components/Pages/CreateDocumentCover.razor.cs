using ATMS.ApplicationService;
using ATMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ATMS.UI.Components.Pages
{
    public partial class CreateDocumentCover
    {
        private bool _isLoading = false;
        private bool _isSubmitting = false;
        private CreateDocumentCoverModel _model = new();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;
                // مقداردهی اولیه
                _model.FiscalYear = DateTime.Now.Year;
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task HandleSubmit()
        {
            _isSubmitting = true;

            try
            {
                // تبدیل تاریخ به DateOnly
                var documentDate = DateOnly.FromDateTime(_model.DocumentDate ?? DateTime.Now);

                // ایجاد Command
                var command = new CreateDocumentCoverCommand(
                    _model.FiscalYear,
                    documentDate,
                    (DocumentTypeEnum)_model.DocumentType!.Value,
                    _model.Description
                );

                var documentId = await Mediator.Send(command);

                // نمایش پیام موفقیت
                // می‌توانید از Toast یا Alert استفاده کنید

                // انتقال به صفحه ثبت آرتیکل‌ها
                Navigation.NavigateTo($"/documents/{documentId}/lines");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "خطا در ثبت سند");
                // نمایش خطا به کاربر
            }
            finally
            {
                _isSubmitting = false;
            }
        }

        private void Cancel()
        {
            Navigation.NavigateTo("/documents");
        }

        // کلاس مدل
        public class CreateDocumentCoverModel
        {
            [Required(ErrorMessage = "سال مالی الزامی است")]
            [Range(1400, 2030, ErrorMessage = "سال مالی باید بین 1400 تا 2030 باشد")]
            public int FiscalYear { get; set; }

            [Required(ErrorMessage = "تاریخ سند الزامی است")]
            public DateTime? DocumentDate { get; set; } = DateTime.Now; // به nullable تغییر دهید

            [Required(ErrorMessage = "نوع سند الزامی است")]
            public int? DocumentType { get; set; }

            [Required(ErrorMessage = "شرح سند الزامی است")]
            [StringLength(500, ErrorMessage = "شرح سند نمی‌تواند بیش از 500 کاراکتر باشد")]
            public string Description { get; set; } = string.Empty;
        }
    }
}
