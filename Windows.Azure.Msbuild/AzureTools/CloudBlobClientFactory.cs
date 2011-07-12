using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    [CoverageExclude(Reason.Humble)]
    public class CloudBlobClientFactory : ICloudBlobClientWrapper
    {
        public IBlobClient Create(Uri endpoint, string accountName, string accountKey)
        {
            return new CloudBlobClientWrapper(endpoint, accountName, accountKey);
        }
    }
}

