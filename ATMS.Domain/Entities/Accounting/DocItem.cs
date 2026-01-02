
using ATMS.Domain.Aggregates;

namespace ATMS.Domain.Entities;

public class DocItem : AuditableAggregate<DocItemId>
{
    public DocHeadId DocHeadId { get; set; } = default!;
    public string DocSerialNo { get; set; } = string.Empty;
    public int ItemNo { get; set; }
    public string ItemDesc { get; set; } = string.Empty;

    public DocHead DocHead { get; set; } = default!;

    private DocItem() { }

    public DocItem(DocHeadId docHeadId, string docSerNo, int itemNo, string itemDesc)
    {
        Id = DocItemId.CreateNew();
        DocHeadId = docHeadId;
        DocSerialNo = docSerNo;
        ItemNo = itemNo;
        ItemDesc = itemDesc;
        CDT = DateTime.UtcNow;
    }

    public static DocItem Create(DocHeadId docHeadId, string docSerNo, int itemNo, string itemDesc)
        => new(docHeadId, docSerNo, itemNo, itemDesc);

    public void Update(DocHeadId docHeadId, string docSerNo, int itemNo, string itemDesc)
    {
        this.Id = DocItemId.CreateNew();
        this.DocHeadId = docHeadId;
        this.DocSerialNo = docSerNo;
        this.ItemNo = itemNo;
        this.ItemDesc = itemDesc;
        this.MDT = DateTime.UtcNow;
    }
}
