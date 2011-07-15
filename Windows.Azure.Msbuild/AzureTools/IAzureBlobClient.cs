using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IAzureBlobClient
    {
        IAzureBlob GetBlobReference(string fileName);
        IAzureBlobContainer GetContainerReference(string containerName);
    }
}

