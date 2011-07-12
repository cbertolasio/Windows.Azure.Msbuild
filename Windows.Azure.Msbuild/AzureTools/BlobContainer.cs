using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.StorageClient;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Delegate)]
    public class BlobContainer : IBlobContainer
    {
        public IBlob GetBlobReference(string fileName)
        {
            var blob = container.GetBlobReference(fileName);
            return new Blob(blob);
        }

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

