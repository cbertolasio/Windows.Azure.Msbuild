using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface ICloudBlobClientWrapper
    {
        IBlobClient Create(Uri endpoint, string accountName, string accountKey);
    }
}

