using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net.Core;
using log4net.Layout;
using System.Linq;

namespace TaskManager {
    public class Log4NetLogger : XmlLayoutSchemaLog4j {

        public static void SetPrivatePropertyValue(object obj, string propName, object val) {
            obj.GetType().GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, val);
        }

        public static object GetPrivatePropertyValue(object obj, string propName) {
            return obj.GetType().GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);
        }

        protected override void FormatXml(XmlWriter writer, LoggingEvent loggingEvent) {
            try {
                if (loggingEvent.LoggerName == nameof(Log4NetLogger)) {
                    writer.WriteRaw(loggingEvent.RenderedMessage);
                    // NOTES: doesn't used.
                } else {
                    if (LocationInfo && loggingEvent.LocationInformation.StackFrames.Length > 1) {
                        var locationInformation = loggingEvent.LocationInformation.StackFrames[2];
                        SetPrivatePropertyValue(loggingEvent.LocationInformation, "m_methodName",
                            String.Format("{0}({1})",
                                locationInformation.Method.Name,
                                String.Join(",", locationInformation.Method.Parameters.Select(e => e.Split('.').Last()))));
                    }
                    base.FormatXml(writer, loggingEvent);
                }
            } catch (Exception e) {
            }
        }
    }
}