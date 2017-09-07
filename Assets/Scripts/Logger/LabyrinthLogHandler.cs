using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Logger
{
    /// <summary>
    /// Custom LogHandler
    /// </summary>
    public class LabyrinthLogHandler : ILogHandler
    {
        public void LogException(System.Exception exception, UnityEngine.Object context)
        {
            Debug.logger.LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            Debug.logger.logHandler.LogFormat(logType, context, format, args);
        }
    }
}
