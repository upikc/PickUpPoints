namespace StorageApp.Model;

public class Package
{
    public int PackageId { get; set; }

    public decimal Weight { get; set; }

    public string ClientFullname { get; set; } = null!;

    public string ClientMail { get; set; } = null!;

    public string ClientNumber { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime StatusDate { get; set; }

    public int ActionstorageId { get; set; }
}

