using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Windows.Azure.Msbuild
{
    public interface IFileManager
    {
        Stream GetFile(string pathToFile);
    }
}

