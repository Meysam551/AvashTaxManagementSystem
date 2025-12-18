
using ATMS.Domain.Aggregates;

namespace ATMS.Domain.Entities;

public class DocHead : AuditableAggregate<DocHeadId>
{
    public string DocSerialNo { get; set; } = string.Empty;
    public string OfficeCode { get; set; } = string.Empty;
    public string DocYear { get; set; } = string.Empty;
    public string DocNo { get; set; } = string.Empty;
    public string DocDescription { get; set; } = string.Empty;

    public ICollection<DocItem> DocItems { get; set; }

    public DocHead() { }

    public DocHead(string docSerNo, string officeCode, string docYear, string docNo, string docDesc)
    {
        this.Id = DocHeadId.CreateNew();
        this.DocSerialNo = docSerNo;
        this.OfficeCode = officeCode;
        this.DocYear = docYear;
        this.DocNo = docNo;
        this.DocDescription = docDesc;
        this.CDT = DateTime.UtcNow;
    }

    public static DocHead Create(string docSerNo, string officeCode, string docYear, string docNo, string docDesc)
        => new(docSerNo, officeCode, docYear, docNo, docDesc);

    public void Update(string docSerNo, string officeCode, string docYear, string docNo, string docDesc)
    {
        this.DocSerialNo = docSerNo;
        this.OfficeCode = officeCode;
        this.DocYear = docYear;
        this.DocNo = docNo;
        this.DocDescription = docDesc;
        this.MDT = DateTime.UtcNow;
    }
}
