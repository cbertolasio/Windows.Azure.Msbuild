using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IAzureBlobContainer
    {
        IAzureBlob GetBlobReference(string fileName);
        bool CreateIfNotExists();
    }
}

