using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IAzureBlobContainer
    {
        DateTimeOffset? LastModified { get; }

        IAzureBlob GetBlobReference(string fileName);
        IAzureBlob GetBlockBlobReference(string fileName);
        bool CreateIfNotExists();
        void Delete();
    }
}

