using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class OperationType
{
    public int TypeId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<PkgOperation> PkgOperations { get; set; } = new List<PkgOperation>();
}
