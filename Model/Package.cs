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


    //internal Package(PackageDTO Pack)
    //{
    //    PackageId = Pack.PackageId;
    //    Weight  = Pack.Weight;
    //    ClientFullname = Pack.ClientFullname;
    //    ClientMail = Pack.ClientMail;
    //    ClientNumber  = Pack.ClientNumber;

    //}
}
