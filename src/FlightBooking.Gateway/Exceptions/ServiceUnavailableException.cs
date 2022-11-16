using System;
using System.Runtime.Serialization;

namespace FlightBooking.Gateway.Exceptions;

public class ServiceUnavailableException : Exception
{
    public ServiceUnavailableException(string serviceName)
    {
        ServiceName = serviceName;
    }

    protected ServiceUnavailableException(SerializationInfo info, StreamingContext context, string serviceName) : base(info, context)
    {
        ServiceName = serviceName;
    }

    public ServiceUnavailableException(string? message, string serviceName) : base(message)
    {
        ServiceName = serviceName;
    }

    public ServiceUnavailableException(string? message, Exception? innerException, string serviceName) : base(message, innerException)
    {
        ServiceName = serviceName;
    }

    public string ServiceName { get; }
}