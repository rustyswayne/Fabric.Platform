using System;

namespace Fabric.Platform.Logging
{
    public interface ILogger
    {
        void Error(Type callingType, string message, Exception exception, params object[] propertyValues);

        void Error(string message, Exception exception, params object[] propertyValues);

        void Warning(Type callingType, string message, Exception exception, params object[] propertyValues);

        void Warning(string message, Exception exception, params object[] propertyValues);

        void Information(string message);

        void Information(string message, params object[] propertyValues);

        void Information(Type callingType, string message, params object[] propertyValues);

        void Debug(string message);

        void Debug(string message, params object[] propertyValues);

        void Debug(Type callingType, string message, params object[] propertyValues);
    }
}