using System.Runtime.Serialization;

namespace Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;

public class ClientException : ServerException
{
    public ClientException(
        string message,
        int? code = null,
        string details = null,
        Exception innerException = null)
        : base(
            message,
            code,
            details,
            innerException)
    {
        Details = details;
    }

    public ClientException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }
}