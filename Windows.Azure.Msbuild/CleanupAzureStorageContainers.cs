using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Windows.Azure.Msbuild.AzureTools;
using System.IO;
using Windows.Azure.Msbuild.Properties;

namespace Windows.Azure.Msbuild
{
    public class CleanupAzureStorageContainers : Task
    {
        public override bool Execute()
        {
            var msg = "Creating cloud storage client with Endpoint: {0}, StorageAccountKey: {1}, StorageAccountName: {2}";
            logger.LogMessage(msg, Endpoint, StorageAccountKey, StorageAccountName);

            var endpoint = new Uri(Endpoint);
            var client = blobClientWrapper.Create(endpoint, StorageAccountName, StorageAccountKey, StorageClientTimeoutInMinutes, ParallelOptionsThreadCount);

            var containers = client.GetAllContainerReferences();
            containers = containers.OrderByDescending(c => c.LastModified).Skip(RemainingContainers);
            foreach (var container in containers)
            {
                container.Delete();
            }

            return true;
        }

        [CoverageExclude(Reason.Humble)]
        public CleanupAzureStorageContainers()
            : this(new AzureBlobClientFactory(), null)
        {
        }

        [CoverageExclude(Reason.Test)]
        public CleanupAzureStorageContainers(IAzureBlobClientFactory blobClientWrapper, ITaskLogger taskLogger)
        {
            if (taskLogger == null)
                taskLogger = new LoggingHelperWrapper(this);

            this.logger = taskLogger;
            this.blobClientWrapper = blobClientWrapper;

            this.StorageClientTimeoutInMinutes = 30;
            this.ParallelOptionsThreadCount = 1;
        }

        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string StorageAccountKey { get; set; }

        [Required]
        public string StorageAccountName { get; set; }

        [Required]
        public int RemainingContainers { get; set; }

        public int StorageClientTimeoutInMinutes { get; set; }

        public int ParallelOptionsThreadCount { get; set; }

        private readonly IAzureBlobClientFactory blobClientWrapper;
        private readonly ITaskLogger logger;
    }
}
