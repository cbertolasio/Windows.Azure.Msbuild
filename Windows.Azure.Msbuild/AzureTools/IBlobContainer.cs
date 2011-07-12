using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IBlobContainer
    {
        IBlob GetBlobReference(string fileName);
        bool CreateIfNotExists();
    }
}

