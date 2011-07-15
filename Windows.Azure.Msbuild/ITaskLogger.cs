using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Windows.Azure.Msbuild
{
    public interface ITaskLogger
    {
        void LogMessage(string message, params object[] args);
    }
}