﻿using System;
using System.Collections.Generic;

namespace StorageApi.Model;

public partial class Storage
{
    public int StorageId { get; set; }

    public string StorageAddr { get; set; } = null!;

    public int Enable { get; set; }

    public virtual ICollection<PkgOperation> PkgOperations { get; set; } = new List<PkgOperation>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

}
    public partial class StorageDTO
    {
        public int StorageId { get; set; }
        public string StorageAddr { get; set; } = null!;

        internal StorageDTO(Storage storage)
        {
            StorageId = storage.StorageId;
            StorageAddr = storage.StorageAddr;
        }
        public StorageDTO()
        {
        }
    }
