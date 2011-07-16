using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Humble)]
    public class AzureBlobClientFactory : IAzureBlobClientFactory
    {
        public IAzureBlobClient Create(Uri endpoint, string accountName, string accountKey, int timeoutInMinutes = 30, int parallelOperationCount = 1)
        {
            return new AzureBlobClient(endpoint, accountName, accountKey, timeoutInMinutes, parallelOperationCount);
        }
    }
}

