namespace StorageApp.Model;

public class Package
{

    public int PackageId { get; set; }

    public decimal Weight { get; set; }

    public string? WeightUnit { get; set; }

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

    public string? DimensionTitle { get; set; }

    public string Status { get; set; } = null!;

    public DateTime StatusDate { get; set; }

    public int ActionstorageId { get; set; }



    public string recipientFullName()
    {
        return $"{RecipientFname} {RecipientSname} {RecipientLname}";
    }

    public string senderFullName()
    {
        return $"{SenderFname} {SenderSname} {SenderLname}";
    }
}

