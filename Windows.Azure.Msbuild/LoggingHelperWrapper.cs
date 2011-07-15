using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Windows.Azure.Msbuild
{
    public class LoggingHelperWrapper : ITaskLogger
    {
        public void LogMessage(string message, params object[] args)
        {
            logger.LogMessage(message, args);
        }

        public LoggingHelperWrapper(ITask taskInstance)
        {
            logger = new TaskLoggingHelper(taskInstance);
        }

        private TaskLoggingHelper logger;
    }
}

