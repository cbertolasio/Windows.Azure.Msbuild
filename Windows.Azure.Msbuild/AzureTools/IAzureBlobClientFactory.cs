using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IAzureBlobClientFactory
    {
        IAzureBlobClient Create(Uri endpoint, string accountName, string accountKey, int timeout, int parallelOperationCount);
    }
}

