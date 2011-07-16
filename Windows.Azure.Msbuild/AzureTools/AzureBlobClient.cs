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

        public AzureBlobClient(Uri endpoint, string accountName, string accountKey, int timeoutInMinutes = 30, int parallelOperationThreadCount = 1)
        {
            var credentials = new StorageCredentialsAccountAndKey(accountName, accountKey);
            blobClient = new CloudBlobClient(endpoint, credentials);
            blobClient.Timeout = new TimeSpan(0, timeoutInMinutes, 0);
            blobClient.ParallelOperationThreadCount = parallelOperationThreadCount;
        }

        private readonly CloudBlobClient blobClient;
    }
}
