using System;
using System.Collections.Generic;
using System.Data;

namespace StorageApi.Model;

public partial class Package
{
    public int PackageId { get; set; }

    public decimal Weight { get; set; }

    public string ClientFullname { get; set; } = null!;

    public string ClientMail { get; set; } = null!;

    public string ClientNumber { get; set; } = null!;

    public virtual ICollection<PkgOperation> PkgOperations { get; set; } = new List<PkgOperation>();


    public Package(int PackageId , decimal Weight , string ClientFullname , string ClientMail , string ClientNumber)
    {
        this.PackageId = PackageId;
        this.Weight = Weight;
        this.ClientFullname = ClientFullname;
        this.ClientMail = ClientMail;
        this.ClientNumber = ClientNumber;

    }
}
