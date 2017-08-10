using System;

namespace Fabric.Platform.Logging
{
    internal class SerilogAdapter : ILogger
    {
        private readonly Serilog.ILogger logger;

        public SerilogAdapter(Serilog.ILogger logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Error(string message, Exception exception)
        {
            this.logger.Error(exception, message);
        }

        public void Error(Type callingType, string message, Exception exception, params object[] propertyValues)
        {
            var contextLogger = this.logger.ForContext(callingType);
            contextLogger.Error(exception, message, propertyValues);
        }

        public void Error(string message, Exception exception, params object[] propertyValues)
        {
            this.logger.Error(exception, message, propertyValues);
        }

        public void Warning(Type callingType, string message, Exception exception, params object[] propertyValues)
        {
            var contextLogger = this.logger.ForContext(callingType);
            contextLogger.Warning(exception, message, propertyValues);
        }

        public void Warning(string message, Exception exception, params object[] propertyValues)
        {
            this.logger.Warning(exception, message, propertyValues);
        }

        public void Information(string message)
        {
            this.logger.Information(message);
        }

        public void Information(string message, params object[] propertyValues)
        {
            this.logger.Information(message, propertyValues);
        }

        public void Information(Type callingType, string message, params object[] propertyValues)
        {
            var contextLogger = this.logger.ForContext(callingType);
            contextLogger.Information(message, propertyValues);
        }

        public void Debug(string message)
        {
            this.logger.Debug(message);
        }

        public void Debug(string message, params object[] propertyValues)
        {
            this.logger.Debug(message, propertyValues);
        }

        public void Debug(Type callingType, string message, params object[] propertyValues)
        {
            var contextLogger = this.logger.ForContext(callingType);
            contextLogger.Debug(message, propertyValues);
        }
    }
}