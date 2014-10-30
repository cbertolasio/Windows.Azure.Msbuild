using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IAzureBlobClient
    {
        IAzureBlob GetBlobReference(string fileName);

        IEnumerable<IAzureBlobContainer> GetAllContainerReferences();

        IAzureBlobContainer GetContainerReference(string containerName);
    }
}

