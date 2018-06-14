using System;
using System.Collections.Generic;
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
    public static class Logger {

        #region [ private ]
        /// <summary>
        ///     Interface of log </summary>
        private static readonly Dictionary<Type, ILog> _commonLoggers = new Dictionary<Type, ILog>();

        #endregion [ private ]

        #region [ .ctor ]

        private static ILog GetLoggerInstance() {
#if DEBUG
            var mth = new StackTrace().GetFrame(3).GetMethod();
#else
            var mth = new StackTrace().GetFrame(2).GetMethod();
#endif
            if (mth?.DeclaringType != null) {
                if (_commonLoggers.ContainsKey(mth.DeclaringType) == false) {
                    ILog logger;
                    if (mth.DeclaringType.IsGenericType == false) {
                        logger = LogManager.GetLogger(mth.DeclaringType.Name);
                    } else {
                        logger = LogManager.GetLogger(
                            $"{mth.DeclaringType.Name}<{mth.DeclaringType.GetGenericArguments().Select(t => t.Name).FirstOrDefault()}>");
                    }
                    _commonLoggers[mth.DeclaringType] = logger;
                }
                return _commonLoggers[mth.DeclaringType];
            } else {
                throw new Exception("can not create logger");
            }
        }

        #endregion [ .ctor ]

        #region [ private ]

        private static void Log(LogLevel level, string message, params object[] args) {
            var _commonLogger = GetLoggerInstance();
            switch (level) {
                case LogLevel.Info:
                    _commonLogger.InfoFormat(message, args);
                    break;
                case LogLevel.Debug:
#if DEBUG
                    _commonLogger.DebugFormat(message, args);
#endif
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

        private static void Log(LogLevel level, string message, Exception ex, params object[] args) {
            var _commonLogger = GetLoggerInstance();
            switch (level) {
                case LogLevel.Info:
                    _commonLogger.InfoFormat(message, ex, args);
                    break;
                case LogLevel.Debug:
#if DEBUG
                    _commonLogger.DebugFormat(message, ex, args);
#endif
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
        public static void Log(string message) {
            Log(LogLevel.Info, message);
        }

        public static void i(string message, params object[] args) {
            Log(LogLevel.Info, message, args);
        }

        public static void w(string message, params object[] args) {
            Log(LogLevel.Warn, message, args);
        }
        public static void w(string message, Exception ex, params object[] args) {
            Log(LogLevel.Warn, message, ex, args);
        }

        public static void e(string message, params object[] args) {
            Log(LogLevel.Error, message, args);
        }
        public static void e(string message, Exception ex, params object[] args) {
            Log(LogLevel.Error, message, ex, args);
        }

        public static void f(string message, params object[] args) {
            Log(LogLevel.Fatal, message, args);
        }
        public static void f(string message, Exception ex, params object[] args) {
            Log(LogLevel.Fatal, message, ex, args);
        }

        public static void d(string message, params object[] args) {
            Log(LogLevel.Debug, message, args);
        }
        public static void d(string message, Exception ex, params object[] args) {
            Log(LogLevel.Debug, message, ex, args);
        }

        #endregion [ Public ]
    }
}