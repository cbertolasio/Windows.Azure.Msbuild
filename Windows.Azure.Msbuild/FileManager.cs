using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Windows.Azure.Msbuild
{
    [CoverageExclude(Reason.Humble)]
    public class FileManager : IFileManager
    {
        public Stream GetFile(string pathToFile)
        {
            return new FileStream(pathToFile, FileMode.Open, FileAccess.Read);
        }
    }
}

