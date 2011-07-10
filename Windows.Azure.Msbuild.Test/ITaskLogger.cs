using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows.Azure.Msbuild.Test
{
    public interface ITaskLogger
    {
        void LogMessage(string message, params object[] args);
    }
}

