
using ATMS.Domain.Abstracts;
using ATMS.Domain.Common;

namespace ATMS.Domain.Entities;

public sealed class DocumentCover : AggregateRoot<DocumentCoverId>
{
    // Properties
    public int FiscalYear { get; set; }
    public DateOnly DocumentDate { get; set; }
    public DocumentType DocumentType { get; set; }
    public DocumentStatus Status { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PostedAt { get; set; }
    public int? DocumentNumber { get; set; }

    // Private constructor for EF Core
    private DocumentCover() { }

    // Factory Method
    public static DocumentCover Create(
        int fiscalYear,
        DateOnly documentDate,
        DocumentType documentType,
        string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("شرح سند الزامی است");

        ValidateDocumentType(documentType, documentDate);

        return new DocumentCover
        {
            Id = DocumentCoverId.CreateNew(),
            FiscalYear = fiscalYear,
            DocumentDate = documentDate,
            DocumentType = documentType,
            Description = description.Trim(),
            Status = DocumentStatus.Draft,
            CreatedAt = DateTime.UtcNow,
            PostedAt = null,
            DocumentNumber = null // بعداً تنظیم می‌شود
        };
    }

    private static void ValidateDocumentType(DocumentType type, DateOnly date)
    {
        if (!Enum.IsDefined(typeof(DocumentTypeEnum), type))
            throw new DomainException("نوع سند نامعتبر است");

        switch (type)
        {
            case DocumentType.Opening:
                if (date.DayOfYear > 5)
                    throw new DomainException("سند افتتاحیه فقط در 5 روز اول سال مالی");
                break;

            case DocumentType.Closing:
                if (date.Month != 12)
                    throw new DomainException("سند اختتامیه فقط در اسفند ماه");
                break;
        }
    }

    // Domain Methods
    public void AssignDocumentNumber(int documentNumber)
    {
        if (documentNumber <= 0)
            throw new DomainException("شماره سند باید بزرگتر از صفر باشد");

        if (DocumentNumber.HasValue)
            throw new DomainException("سند قبلاً شماره‌گذاری شده است");

        DocumentNumber = documentNumber;
    }

    public void Post()
    {
        if (Status != DocumentStatus.Draft)
            throw new DomainException("فقط سندهای پیش‌نویس قابل ثبت هستند");

        Status = DocumentStatus.Posted;
        PostedAt = DateTime.UtcNow;
    }

    public void Cancel(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("علت لغو باید مشخص شود");

        Status = DocumentStatus.Cancelled;
    }

    // Helper Methods
    public bool IsDraft() => Status == DocumentStatus.Draft;
    public bool IsPosted() => Status == DocumentStatus.Posted;
    public bool IsSystemDocument() =>
        DocumentType == DocumentType.Opening ||
        DocumentType == DocumentType.Closing;
}

// Enum برای وضعیت سند (در Domain یا Shared)
public enum DocumentStatus
{
    Draft = 1,
    Posted = 2,
    Cancelled = 3
}
