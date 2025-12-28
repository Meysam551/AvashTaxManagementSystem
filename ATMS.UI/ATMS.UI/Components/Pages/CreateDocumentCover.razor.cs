using ATMS.ApplicationService;
using ATMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ATMS.UI.Components.Pages
{
    public partial class CreateDocumentCover
    {
        // ViewModel
        private CreateDocumentCoverVm _model = new();
        private bool _isSubmitting = false;
        private bool _isLoading = false;

        protected override void OnInitialized()
        {
            // مقداردهی اولیه
            _model.FiscalYear = DateTime.Now.Year;
            _model.DocumentDate = DateTime.Now;
            _model.DocumentType = DocumentTypeEnum.General;
        }

        private async Task HandleSubmit()
        {
            _isSubmitting = true;

            try
            {
                // تبدیل تاریخ به DateOnly
                var documentDate = DateOnly.FromDateTime(_model.DocumentDate);

                // ایجاد Command
                var command = new CreateDocumentCoverCommand(
                    _model.FiscalYear,
                    documentDate,
                    _model.DocumentType!.Value, // مطمئن هستیم مقدار دارد
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
                // می‌توانید از State مدیریت خطا استفاده کنید
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

        // ViewModel Class
        public class CreateDocumentCoverVm
        {
            [Required(ErrorMessage = "تاریخ سند الزامی است")]
            public DateTime DocumentDate { get; set; }

            [Required(ErrorMessage = "سال مالی الزامی است")]
            [Range(1400, 2030, ErrorMessage = "سال مالی باید بین ۱۴۰۰ تا 2030 باشد")]
            public int FiscalYear { get; set; }

            [Required(ErrorMessage = "نوع سند الزامی است")]
            public DocumentTypeEnum? DocumentType { get; set; }

            [Required(ErrorMessage = "شرح سند الزامی است")]
            [StringLength(500, ErrorMessage = "شرح سند نمی‌تواند بیش از ۵۰۰ کاراکتر باشد")]
            public string Description { get; set; } = string.Empty;
        }
    }
}
