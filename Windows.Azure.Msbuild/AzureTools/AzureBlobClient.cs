using System;
using System.Collections.Generic;
using System.Linq;
 
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;

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

        public IEnumerable<IAzureBlobContainer> GetAllContainerReferences()
        {
            List<IAzureBlobContainer> azureBlobContainers = new List<IAzureBlobContainer>();
            foreach (var container in blobClient.ListContainers())
            {
                azureBlobContainers.Add(new AzureBlobContainer(container));
            }
            return azureBlobContainers;
        }

        public IAzureBlob GetBlobReference(string fileName)
        {
            var blob = blobClient.GetBlobReferenceFromServer(new StorageUri(new Uri(fileName)));
            blob.UploadFromFile(fileName, System.IO.FileMode.Create);
            return new AzureBlob(blob);
        }

        public AzureBlobClient(Uri endpoint, string accountName, string accountKey, int timeoutInMinutes = 30, int parallelOperationThreadCount = 1)
        {
            var credentials = new StorageCredentials(accountName, accountKey);
            blobClient = new CloudBlobClient(endpoint, credentials);
            blobClient.DefaultRequestOptions.MaximumExecutionTime = new TimeSpan(0, timeoutInMinutes, 0);
            blobClient.DefaultRequestOptions.ParallelOperationThreadCount = parallelOperationThreadCount;
        }

        private readonly CloudBlobClient blobClient;
    }
}
