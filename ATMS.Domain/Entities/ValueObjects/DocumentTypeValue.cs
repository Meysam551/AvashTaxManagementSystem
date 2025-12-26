
using ATMS.Shared;
using ATMS.Domain.Common;

namespace ATMS.Domain.Entities;

public class DocumentTypeValue : ValueObject
{
    // مقادیر ثابت (استاتیک)
    public static DocumentTypeValue General => new(1, "سند عمومی", "GEN");
    public static DocumentTypeValue Opening => new(2, "سند افتتاحیه", "OPN");
    public static DocumentTypeValue Closing => new(3, "سند اختتامیه", "CLS");
    public static DocumentTypeValue Adjustment => new(4, "سند تعدیل", "ADJ");
    public static DocumentTypeValue Tax => new(5, "سند مالیاتی", "TAX");
    public static DocumentTypeValue Payroll => new(6, "سند حقوق و دستمزد", "PAY");

    // Properties
    public int Id { get; }
    public string Title { get; }
    public string Code { get; }

    // Privat constructor
    private DocumentTypeValue(int id, string title, string code)
    {
        Id = id;
        Title = title;
        Code = code;
    }

    // لیست تمام انواع سند
    public static IReadOnlyList<DocumentTypeValue> GetAll()
    {
        return new[]
        {
            General,
            Opening,
            Closing,
            Adjustment,
            Tax,
            Payroll
        };
    }

    // ایجاد از Id
    public static DocumentTypeValue FromId(int id)
    {
        return id switch
        {
            1 => General,
            2 => Opening,
            3 => Closing,
            4 => Adjustment,
            5 => Tax,
            6 => Payroll,
            _ => throw new DomainException($"نوع سند نامعتبر: {id}")
        };
    }

    // تبدیل به Enum (اگر نیاز UI دارید)
    public DocumentTypeEnum ToEnum() => (DocumentTypeEnum)Id;

    // منطق دامنه: آیا این نوع سند قابل ویرایش است؟
    public bool IsEditable => Id != (int)DocumentTypeEnum.Closing;

    // منطق دامنه: آیا نیاز به تایید دارد؟
    public bool RequiresApproval => Id == (int)DocumentTypeEnum.Payroll;

    // منطق دامنه: آیا سند سیستم است؟
    public bool IsSystemDocument => Id is 2 or 3; // Opening, Closing

    // 🔥 مهم: تعریف تساوی
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id; // فقط بر اساس Id مقایسه می‌شود
    }

    public override string ToString() => $"{Code} - {Title}";
}

// Enum معادل برای UI (در Shared یا Application)
public enum DocumentTypeEnum
{
    General = 1,
    Opening = 2,
    Closing = 3,
    Adjustment = 4,
    Tax = 5,
    Payroll = 6
}