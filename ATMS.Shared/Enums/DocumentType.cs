
using System.ComponentModel.DataAnnotations;

namespace ATMS.Shared.Enums;

public enum DocumentType
{
    [Display(Name = "سند عمومی", Description = "برای معاملات روزمره")]
    General = 1,

    [Display(Name = "سند افتتاحیه", Description = "افتتاح دوره مالی")]
    Opening = 2,

    [Display(Name = "سند اختتامیه", Description = "اختتام دوره مالی")]
    Closing = 3,

    [Display(Name = "سند تعدیل", Description = "تعدیلات پایان دوره")]
    Adjustment = 4,

    [Display(Name = "سند مالیاتی", Description = "عملیات مالیاتی")]
    Tax = 5,

    [Display(Name = "سند حقوق و دستمزد", Description = "پرداخت حقوق")]
    Payroll = 6
}