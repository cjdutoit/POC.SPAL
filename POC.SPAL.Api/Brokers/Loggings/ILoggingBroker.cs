using System;

namespace POC.SPAL.Api.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogInformation(string message);
    }
}
