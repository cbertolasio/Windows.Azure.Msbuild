using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Delegate)]
    public class Blob : IBlob
    {
        public bool DeleteIfExists()
        {
            return cloudBlob.DeleteIfExists();
        }

        public void UploadFromStream(Stream stream)
        {
            cloudBlob.UploadFromStream(stream);
        }

        public void UploadFile(string fileName)
        {
            cloudBlob.UploadFile(fileName);
        }

        public Blob(CloudBlob cloudBlob)
        {
            this.cloudBlob = cloudBlob;
        }

        private readonly CloudBlob cloudBlob;
    }
}

