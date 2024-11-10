using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class PkgOperationsWithtype
{
    public int OperationId { get; set; }

    public int PackageId { get; set; }

    public int UserId { get; set; }

    public string Type { get; set; } = null!;

    public int ActionstorageId { get; set; }

    public DateTime OperationDate { get; set; }

    public int TypeId { get; set; }
}
