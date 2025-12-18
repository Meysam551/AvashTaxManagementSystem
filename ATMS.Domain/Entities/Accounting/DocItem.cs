
using ATMS.Domain.Aggregates;

namespace ATMS.Domain.Entities;

public class DocItem : AuditableAggregate<DocItemId>
{
    public DocHeadId ParrentId { get; set; } = new();
    public string DocSerialNo { get; set; } = string.Empty;
    public int ItemNo { get; set; } = 0;
    public string ItemDesc { get; set; } = string.Empty;

    public DocItem() { }

    public DocItem(DocHeadId docHeadId, string docSerNo, int itemNo, string itemDesc)
    {
        this.Id = DocItemId.CreateNew();
        this.ParrentId = docHeadId;
        this.DocSerialNo = docSerNo;
        this.ItemNo = itemNo;
        this.ItemDesc = itemDesc;
        this.CDT = DateTime.UtcNow;
    }

    public static DocItem Create(DocHeadId docHeadId, string docSerNo, int itemNo, string itemDesc)
        => new(docHeadId, docSerNo, itemNo, itemDesc);

    public void Update(DocHeadId docHeadId, string docSerNo, int itemNo, string itemDesc)
    {
        this.Id = DocItemId.CreateNew();
        this.ParrentId = docHeadId;
        this.DocSerialNo = docSerNo;
        this.ItemNo = itemNo;
        this.ItemDesc = itemDesc;
        this.MDT = DateTime.UtcNow;
    }
}
