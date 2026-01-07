using System.Globalization;
using ATMS.ApplicationService;
using ATMS.Shared.Dtos;

namespace ATMS.UI.Components.Pages.Documents
{
    public partial class DocumentCoverList
    {
        private bool _isLoading = false;
        private bool _isDeleting = false;
        private List<DocumentCoverDto> _documents = new();
        private DocumentCoverDto? _documentToDelete = null;

        // فیلترها
        private GetDocumentCoversQuery _filter = new();
        private DateTime? _fromDateFilter = null;
        private DateTime? _toDateFilter = null;

        // صفحه‌بندی
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalCount = 0;
        private int _totalPages = 0;

        protected override async Task OnInitializedAsync()
        {
            await LoadDocuments();
        }

        private async Task LoadDocuments()
        {
            try
            {
                _isLoading = true;

                // تبدیل تاریخ‌های فیلتر
                if (_fromDateFilter.HasValue)
                {
                    _filter.FromDate = DateOnly.FromDateTime(_fromDateFilter.Value);
                }

                if (_toDateFilter.HasValue)
                {
                    _filter.ToDate = DateOnly.FromDateTime(_toDateFilter.Value);
                }

                var result = await Mediator.Send(_filter);
                _documents = result.Value.ToList();
                _totalCount = _documents.Count;
                _totalPages = (int)Math.Ceiling((double)_totalCount / _pageSize);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "خطا در بارگذاری اسناد");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void ResetFilters()
        {
            _filter = new GetDocumentCoversQuery();
            _fromDateFilter = null;
            _toDateFilter = null;
            _currentPage = 1;
        }

        private void NavigateToCreate()
        {
            Navigation.NavigateTo("/");
        }

        private void ViewDocument(Guid id)
        {
            Navigation.NavigateTo($"/documents/{id}");
        }

        private void EditDocument(Guid id)
        {
            Navigation.NavigateTo($"/documents/{id}/edit");
        }

        private void DeleteDocument(DocumentCoverDto document)
        {
            _documentToDelete = document;
        }

        private void CancelDelete()
        {
            _documentToDelete = null;
        }

        private async Task ConfirmDelete()
        {
            if (_documentToDelete == null) return;

            try
            {
                _isDeleting = true;

                // TODO: کد حذف داکیومنت
                // var command = new DeleteDocumentCoverCommand(_documentToDelete.Id);
                // await Mediator.Send(command);

                // حذف از لیست محلی
                _documents.Remove(_documentToDelete);
                _documentToDelete = null;

                await LoadDocuments();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "خطا در حذف سند");
            }
            finally
            {
                _isDeleting = false;
            }
        }

        private void ChangePage(int page)
        {
            _currentPage = page;
            // TODO: لود داده‌های صفحه جاری
            // می‌توانید کوئری را برای صفحه‌بندی سرور-ساید اصلاح کنید
        }

        private string ConvertToPersianDate(DateOnly date)
        {
            try
            {
                var persianCalendar = new PersianCalendar();
                return $"{persianCalendar.GetYear(date.ToDateTime(TimeOnly.MinValue))}/{persianCalendar.GetMonth(date.ToDateTime(TimeOnly.MinValue)):00}/{persianCalendar.GetDayOfMonth(date.ToDateTime(TimeOnly.MinValue)):00}";
            }
            catch
            {
                return date.ToString();
            }
        }

        // private string GetDocumentTypeBadgeClass(DocumentType documentType)
        // {
        //     return documentType switch
        //     {
        //         DocumentType.SanadDasti => "bg-primary",
        //         DocumentType.SanadHesabdari => "bg-success",
        //         DocumentType.SanadBanki => "bg-info",
        //         DocumentType.SanadDaryaft => "bg-warning text-dark",
        //         DocumentType.SanadPardakht => "bg-danger",
        //         _ => "bg-secondary"
        //     };
        // }
    }
}
