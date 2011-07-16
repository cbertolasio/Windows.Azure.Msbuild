using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Delegate)]
    public class AzureBlobContainer : IAzureBlobContainer
    {
        public IAzureBlob GetBlobReference(string fileName)
        {
            var blob = container.GetBlobReference(fileName);
            return new AzureBlob(blob);
        }

        public IAzureBlob GetBlockBlobReference(string fileName)
        {
            var blob = container.GetBlockBlobReference(fileName);
            return new AzureBlockBlob(blob);
        }

        public bool CreateIfNotExists()
        {
            return container.CreateIfNotExist();
        }

        public AzureBlobContainer(CloudBlobContainer container)
        {
            this.container = container;
        }

        private readonly CloudBlobContainer container;
    }
}

