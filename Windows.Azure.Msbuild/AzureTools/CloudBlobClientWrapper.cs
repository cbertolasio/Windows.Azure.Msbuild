using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Framework)]
    public class CloudBlobClientWrapper : IBlobClient
    {
        public IBlobContainer GetContainerReference(string containerName)
        {
            var blobContainer = blobClient.GetContainerReference(containerName);
            return new BlobContainer(blobContainer);
        }

        public IBlob GetBlobReference(string fileName)
        {
            var blob = blobClient.GetBlobReference(fileName);
            blob.UploadFile(fileName);
            return new Blob(blob);
        }

        public CloudBlobClientWrapper(Uri endpoint, string accountName, string accountKey)
        {
            var credentials = new StorageCredentialsAccountAndKey(accountName, accountKey);
            blobClient = new CloudBlobClient(endpoint, credentials);
        }

        private readonly CloudBlobClient blobClient;
    }
}
