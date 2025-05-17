using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class Dimension
{
    public string DimensionId { get; set; } = null!;

    public string DimensionTitle { get; set; } = null!;

    public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
}
