using System;
using System.Diagnostics;
using System.Linq;
using Common.Logging;

namespace TaskManager.Common {
    /// <summary>
    ///     Logging class </summary>
    /// <remarks>
    /// Info - logs of user actions or outside events
    /// Debug - logs of detail debuggin. default turn off
    /// Warning - logs of notes
    /// Error - logs of occur errors
    /// Fatal - logs of crush application
    /// </remarks>
    public class Logger<T> {

        #region [ private ]
        /// <summary>
        ///     Interface of log </summary>
        private readonly ILog _commonLogger;

        #endregion [ private ]

        #region [ .ctor ]

        public Logger() {
            if (typeof(T).IsGenericType == false) {
                _commonLogger = LogManager.GetLogger(typeof(T).Name);
            } else {
                _commonLogger = LogManager.GetLogger($"{typeof(T).Name}<{typeof(T).GetGenericArguments().Select(t => t.Name).FirstOrDefault()}>");
            }
        }

        #endregion [ .ctor ]

        #region [ private ]

        private void Log(LogLevel level, string message, params object[] args) {
            switch (level) {
                case LogLevel.Info:
                    _commonLogger.InfoFormat(message, args);
                    break;
                case LogLevel.Debug:
                    _commonLogger.DebugFormat(message, args);
                    break;
                case LogLevel.Warn:
                    _commonLogger.WarnFormat(message, args);
                    break;
                case LogLevel.Error:
                    _commonLogger.ErrorFormat(message, args);
                    break;
                case LogLevel.Fatal:
                    _commonLogger.FatalFormat(message, args);
                    break;
            }
#if DEBUG
            Debug.WriteLine(message, args);
#endif
        }

        private void Log(LogLevel level, string message, Exception ex, params object[] args) {
            switch (level) {
                case LogLevel.Info:
                    _commonLogger.InfoFormat(message, ex, args);
                    break;
                case LogLevel.Debug:
                    _commonLogger.DebugFormat(message, ex, args);
                    break;
                case LogLevel.Warn:
                    _commonLogger.WarnFormat(message, ex, args);
                    break;
                case LogLevel.Error:
                    _commonLogger.ErrorFormat(message, ex, args);
                    break;
                case LogLevel.Fatal:
                    _commonLogger.FatalFormat(message, ex, args);
                    break;
            }
#if DEBUG
            Debug.WriteLine(message, args);
#endif
        }

        #endregion [ private ]

        #region [ Public ]

        /// <summary>
        ///     Write log from clients </summary>
        public void Log(string message) {
            Log(LogLevel.Info, message);
        }

        public void i(string message, params object[] args) {
            Log(LogLevel.Info, message, args);
        }

        public void w(string message, params object[] args) {
            Log(LogLevel.Warn, message, args);
        }
        public void w(string message, Exception ex, params object[] args) {
            Log(LogLevel.Warn, message, ex, args);
        }

        public void e(string message, params object[] args) {
            Log(LogLevel.Error, message, args);
        }
        public void e(string message, Exception ex, params object[] args) {
            Log(LogLevel.Error, message, ex, args);
        }

        public void f(string message, params object[] args) {
            Log(LogLevel.Fatal, message, args);
        }
        public void f(string message, Exception ex, params object[] args) {
            Log(LogLevel.Fatal, message, ex, args);
        }

        public void d(string message, params object[] args) {
            Log(LogLevel.Debug, message, args);
        }
        public void d(string message, Exception ex, params object[] args) {
            Log(LogLevel.Debug, message, ex, args);
        }

        #endregion [ Public ]
    }
}