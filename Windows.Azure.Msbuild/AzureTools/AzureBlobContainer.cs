using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Delegate)]
    public class AzureBlobContainer : IAzureBlobContainer
    {
        public DateTimeOffset? LastModified
        {
            get { return container.Properties.LastModified; }
        }

        public IAzureBlob GetBlobReference(string fileName)
        {
            var blob = container.GetBlobReferenceFromServer(fileName);
            return new AzureBlob(blob);
        }

        public IAzureBlob GetBlockBlobReference(string fileName)
        {
            var blob = container.GetBlockBlobReference(fileName);
            return new AzureBlockBlob(blob);
        }

        public bool CreateIfNotExists()
        {
            return container.CreateIfNotExists();
        }

        public AzureBlobContainer(CloudBlobContainer container)
        {
            this.container = container;
        }

        private readonly CloudBlobContainer container;
        
        public void Delete()
        {
            container.Delete();
        }
    }
}

