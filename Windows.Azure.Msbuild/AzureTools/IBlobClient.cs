using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IBlobClient
    {
        IBlob GetBlobReference(string fileName);

        IBlobContainer GetContainerReference(string containerName);
    }
}

