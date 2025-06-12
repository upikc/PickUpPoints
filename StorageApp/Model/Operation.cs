namespace StorageApp.Model;

public class Operation
{
    public int OperationId { get; set; }

    public int PackageId { get; set; }

    public int UserId { get; set; }
    public string UserFullname { get { return Context.getUsers().First(x => x.UserId == UserId).Fullname; } }

    public string Type { get; set; } = null!;

    public int ActionstorageId { get; set; }

    public DateTime OperationDate { get; set; }

    public int TypeId { get; set; }

    public int? CommandingstorageId { get; set; }
    public string StorageAddress { get { return Context.getStorages().First(x => x.storageId == ActionstorageId).storageAddr; }  }


    public string CommandingStorageAddress { get { return Context.getStorages().First(x => x.storageId == CommandingstorageId).storageAddr; } }
}
