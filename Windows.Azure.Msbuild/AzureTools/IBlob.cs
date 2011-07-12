using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Windows.Azure.Msbuild.AzureTools
{
    public interface IBlob
    {
        bool DeleteIfExists();
        void UploadFromStream(Stream stream);
        void UploadFile(string fileName);
    }
}

