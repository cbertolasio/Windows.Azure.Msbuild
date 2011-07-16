using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;

namespace Windows.Azure.Msbuild.AzureTools
{
    public class AzureBlockBlob : IAzureBlob
    {

        public bool DeleteIfExists()
        {
            return blob.DeleteIfExists();
        }

        public void UploadFromStream(Stream stream)
        {
            blob.UploadFromStream(stream);
        }

        public void UploadFile(string fileName)
        {
            blob.UploadFile(fileName);
        }

        private readonly CloudBlockBlob blob;

        public AzureBlockBlob(CloudBlockBlob blob)
        {
            this.blob = blob;
        }
    }
}

