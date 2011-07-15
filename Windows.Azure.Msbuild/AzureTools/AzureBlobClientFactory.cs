using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Humble)]
    public class AzureBlobClientFactory : IAzureBlobClientFactory
    {
        public IAzureBlobClient Create(Uri endpoint, string accountName, string accountKey)
        {
            return new AzureBlobClient(endpoint, accountName, accountKey);
        }
    }
}

