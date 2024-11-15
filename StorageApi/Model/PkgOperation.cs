using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class PkgOperation
{
    public int OperationId { get; set; }

    public int PackageId { get; set; }

    public int UserId { get; set; }

    public int TypeId { get; set; }

    public DateTime OperationDate { get; set; }

    public int ActionstorageId { get; set; }

    public virtual Storage Actionstorage { get; set; } = null!;

    public virtual Package Package { get; set; } = null!;

    public virtual OperationType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
