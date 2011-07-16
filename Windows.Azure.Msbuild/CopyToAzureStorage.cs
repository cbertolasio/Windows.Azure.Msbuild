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
    public class CopyToAzureStorage : Task
    {
        public override bool Execute()
        {
            var msg = "Creating cloud storage client with Endpoint: {0}, StorageAccountKey: {1}, StorageAccountName: {2}";
            logger.LogMessage(msg, Endpoint, StorageAccountKey, StorageAccountName);

            var endpoint = new Uri(Endpoint);
            var client = blobClientWrapper.Create(endpoint, StorageAccountName, StorageAccountKey, StorageClientTimeoutInMinutes, ParallelOptionsThreadCount);
            var container = client.GetContainerReference(ContainerName);
            if (container.CreateIfNotExists()) {
                logger.LogMessage(Resources.Msg_ContainerCreated, ContainerName);
            }

            for (int i = 0; i < SourceFiles.Length; i++) {
                var pathToSource = SourceFiles[i].ItemSpec;
                var pathToDest = (DestinationFiles != null) ? DestinationFiles[i].ItemSpec : Path.GetFileName(pathToSource);

                var blob = container.GetBlockBlobReference(pathToDest);
                if (blob.DeleteIfExists()) {
                    logger.LogMessage(Resources.Msg_DeletedBlob, pathToDest);
                }

                using (Stream stream = fileManager.GetFile(pathToSource)) {
                    blob.UploadFromStream(stream);
                    logger.LogMessage(Resources.Msg_UploadedBlob, pathToSource);
                }
            }


            return true;
        }

        [CoverageExclude(Reason.Humble)]
        public CopyToAzureStorage()
            : this(new AzureBlobClientFactory(), null, new FileManager())
        {
        }

        [CoverageExclude(Reason.Test)]
        public CopyToAzureStorage(IAzureBlobClientFactory blobClientWrapper, ITaskLogger taskLogger, IFileManager fileManager)
        {
            if (taskLogger == null)
                taskLogger = new LoggingHelperWrapper(this);

            this.logger = taskLogger;
            this.fileManager = fileManager;
            this.blobClientWrapper = blobClientWrapper;

            this.StorageClientTimeoutInMinutes = 30;
            this.ParallelOptionsThreadCount = 1;
        }

        [Required]
        public string ContainerName { get; set; }

        [Required]
        public string Endpoint { get; set; }

        [Required]
        public ITaskItem[] SourceFiles { get; set; }

        [Required]
        public ITaskItem[] DestinationFiles { get; set; }

        [Required]
        public string StorageAccountKey { get; set; }

        [Required]
        public string StorageAccountName { get; set; }

        public int StorageClientTimeoutInMinutes { get; set; }

        public int ParallelOptionsThreadCount { get; set; }

        private readonly IAzureBlobClientFactory blobClientWrapper;
        private readonly IFileManager fileManager;
        private readonly ITaskLogger logger;
    }
}
