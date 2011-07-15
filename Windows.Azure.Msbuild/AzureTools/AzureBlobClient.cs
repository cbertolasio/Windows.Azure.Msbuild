using System;
using System.Collections.Generic;
using System.Linq;
 
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Framework)]
    public class AzureBlobClient : IAzureBlobClient
    {
        public IAzureBlobContainer GetContainerReference(string containerName)
        {
            var blobContainer = blobClient.GetContainerReference(containerName);
            return new AzureBlobContainer(blobContainer);
        }

        public IAzureBlob GetBlobReference(string fileName)
        {
            var blob = blobClient.GetBlobReference(fileName);
            blob.UploadFile(fileName);
            return new AzureBlob(blob);
        }

        public AzureBlobClient(Uri endpoint, string accountName, string accountKey)
        {
            var credentials = new StorageCredentialsAccountAndKey(accountName, accountKey);
            blobClient = new CloudBlobClient(endpoint, credentials);
        }

        private readonly CloudBlobClient blobClient;
    }
}
