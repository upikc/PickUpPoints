using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class Storage
{
    public int StorageId { get; set; }

    public string StorageAddr { get; set; } = null!;

    public virtual ICollection<PkgOperation> PkgOperations { get; set; } = new List<PkgOperation>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
