using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ATMS.ApplicationService;
using ATMS.Domain.Entities;
using ATMS.Shared;
using ATMS.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATMS.UI.Components.Pages
{
    public partial class CreateDocumentCover
    {
        private bool _isLoading = false;
        private bool _isSubmitting = false;
        private bool _dateError = false;
        private CreateDocumentCoverModel _model = new();
        private string _persianDateString = string.Empty;
        private List<SelectListItem> DocumentTypeOptions { get; set; } = new();

        private Dictionary<string, object> GetInputAttributes()
        {
            var attributes = new Dictionary<string, object>();

            if (_dateError)
            {
                attributes["class"] = "form-control is-invalid";
            }

            return attributes;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _isLoading = true;

                // مقداردهی اولیه سال مالی (مثلاً سال شمسی فعلی)
                var persianCalendar = new PersianCalendar();
                _model.FiscalYear = persianCalendar.GetYear(DateTime.Now);

                // مقداردهی اولیه تاریخ شمسی
                var now = DateTime.Now;
                _persianDateString = $"{persianCalendar.GetYear(now)}/{persianCalendar.GetMonth(now):00}/{persianCalendar.GetDayOfMonth(now):00}";

                // تبدیل به تاریخ میلادی
                UpdateDocumentDateFromPersian();

                DocumentTypeOptions = Enum.GetValues(typeof(DocumentTypeEnum))
                    .Cast<DocumentTypeEnum>()
                    .Select(e => new SelectListItem
                    {
                        Value = ((int)e).ToString(),
                        Text = e.GetDisplayName()
                    })
                    .ToList();
            }
            finally
            {
                _isLoading = false;
            }
        }

        // Property برای bind کردن تاریخ شمسی
        private string PersianDateString
        {
            get => _persianDateString;
            set
            {
                _persianDateString = value;
                UpdateDocumentDateFromPersian();
            }
        }

        // تبدیل تاریخ شمسی به میلادی
        private void UpdateDocumentDateFromPersian()
        {
            _dateError = false;

            if (string.IsNullOrWhiteSpace(_persianDateString))
            {
                _model.DocumentDate = null;
                return;
            }

            // بررسی قالب
            if (!System.Text.RegularExpressions.Regex.IsMatch(_persianDateString, @"^\d{4}/\d{2}/\d{2}$"))
            {
                _dateError = true;
                _model.DocumentDate = null;
                return;
            }

            try
            {
                var parts = _persianDateString.Split('/');
                if (parts.Length != 3)
                {
                    _dateError = true;
                    return;
                }

                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                // تبدیل تاریخ شمسی به میلادی
                var persianCalendar = new PersianCalendar();
                _model.DocumentDate = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
                _dateError = false;
            }
            catch
            {
                _dateError = true;
                _model.DocumentDate = null;
            }
        }

        private async Task HandleSubmit()
        {
            // اعتبارسنجی تاریخ
            if (_dateError || !_model.DocumentDate.HasValue)
            {
                _dateError = true;
                return;
            }

            _isSubmitting = true;

            try
            {
                // تبدیل تاریخ به DateOnly
                var documentDate = DateOnly.FromDateTime(_model.DocumentDate.Value);

                // ایجاد Command
                var command = new CreateDocumentCoverCommand(
                    _model.FiscalYear,
                    documentDate,
                    (DocumentType)_model.DocumentType!.Value,
                    _model.Description
                );

                var documentId = await Mediator.Send(command);

                // نمایش پیام موفقیت
                // می‌توانید از Toast یا Alert استفاده کنید

                // انتقال به صفحه ثبت آرتیکل‌ها
                Navigation.NavigateTo($"/documentcovers/list");
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
            Navigation.NavigateTo("/documentcovers/list");
        }

        // کلاس مدل
        public class CreateDocumentCoverModel
        {
            [Required(ErrorMessage = "سال مالی الزامی است")]
            [Range(1400, 2030, ErrorMessage = "سال مالی باید بین 1400 تا 2030 باشد")]
            public int FiscalYear { get; set; }

            [Required(ErrorMessage = "تاریخ سند الزامی است")]
            public DateTime? DocumentDate { get; set; }

            [Required(ErrorMessage = "نوع سند الزامی است")]
            public int? DocumentType { get; set; }

            [Required(ErrorMessage = "شرح سند الزامی است")]
            [StringLength(500, ErrorMessage = "شرح سند نمی‌تواند بیش از 500 کاراکتر باشد")]
            public string Description { get; set; } = string.Empty;
        }
    }
}
