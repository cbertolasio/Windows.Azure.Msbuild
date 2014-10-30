using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Delegate)]
    public class AzureBlob : IAzureBlob
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
            cloudBlob.UploadFromFile(fileName, FileMode.Create);
        }

        public AzureBlob(ICloudBlob cloudBlob)
        {
            this.cloudBlob = cloudBlob;
        }

        private readonly ICloudBlob cloudBlob;
    }
}

