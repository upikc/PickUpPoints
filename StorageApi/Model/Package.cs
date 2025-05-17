using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class Package
{
    public int PackageId { get; set; }

    public decimal Weight { get; set; }

    public int UnitofWeightId { get; set; }

    public string DimensionId { get; set; } = null!;

    public string SenderFname { get; set; } = null!;

    public string SenderSname { get; set; } = null!;

    public string SenderLname { get; set; } = null!;

    public string SenderMail { get; set; } = null!;

    public string? SenderNumber { get; set; }

    public string RecipientFname { get; set; } = null!;

    public string RecipientSname { get; set; } = null!;

    public string RecipientLname { get; set; } = null!;

    public string RecipientMail { get; set; } = null!;

    public string? RecipientNumber { get; set; }

    public virtual Dimension Dimension { get; set; } = null!;

    public virtual ICollection<PkgOperation> PkgOperations { get; set; } = new List<PkgOperation>();

    public virtual UnitofWeight UnitofWeight { get; set; } = null!;



}
