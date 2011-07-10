using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.StorageClient;

namespace Windows.Azure.Msbuild.AzureTools
{
    public class BlobContainer : IBlobContainer
    {
        public bool CreateIfNotExists()
        {
            return container.CreateIfNotExist();
        }

        public BlobContainer(CloudBlobContainer container)
        {
            this.container = container;
        }

        private readonly CloudBlobContainer container;
    }
}

