using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IBlobClient
    {
        IBlobContainer GetContainerReference(string containerName);
    }
}

