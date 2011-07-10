using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Windows.Azure.Msbuild.AzureTools;

namespace Windows.Azure.Msbuild
{
    public class CopyToAzureStorage : Task
    {
        [Required]
        public string ContainerName { get; set; }

        [Required]
        public string Endpoint { get; set; }

        [Required]
        public string StorageAccountKey { get; set; }

        [Required]
        public string StorageAccountName { get; set; }

        public override bool Execute()
        {
            var msg = "Creating cloud storage client with Endpoint: {0}, StorageAccountKey: {1}, StorageAccountName: {2}";
            logger.LogMessage(msg, Endpoint.ToString(), StorageAccountKey, StorageAccountName);

            var endpoint = new Uri(Endpoint);
            var client = blobClientWrapper.Create(endpoint, StorageAccountName, StorageAccountKey);
            var container = client.GetContainerReference(ContainerName);
            container.CreateIfNotExists();

            return true;
        }

        public CopyToAzureStorage() : this(new CloudBlobClientFactory(), null)
        {
            
        }
        public CopyToAzureStorage(ICloudBlobClientWrapper blobClientWrapper, ITaskLogger logger)
        {
            if (logger == null)
                logger = new MyTaskLogger(this);

            this.logger = logger;
            this.blobClientWrapper = blobClientWrapper;            
        }

        private readonly ICloudBlobClientWrapper blobClientWrapper;
        private readonly ITaskLogger logger;
    }
}
