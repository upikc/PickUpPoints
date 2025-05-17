using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class UnitofWeight
{
    public int UnitofWeightId { get; set; }

    public string UnitofWeightTitle { get; set; } = null!;

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
