using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

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
            blob.UploadFromFile(fileName, FileMode.Create);
        }

        private readonly CloudBlockBlob blob;

        public AzureBlockBlob(CloudBlockBlob blob)
        {
            this.blob = blob;
        }
    }
}

