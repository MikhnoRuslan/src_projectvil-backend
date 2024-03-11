using System.Runtime.Serialization;

namespace Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;

public class ServerException : BaseException
{
    public int? Code { get; set; }
    public string Details { get; set; }
    
    public ServerException(
        string message = null,
        int? code = null,
        string details = null,
        Exception innerException = null)
        : base(message, innerException)
    {
        Code = code;
        Details = details;
    }
    
    public ServerException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {

    }

    public ServerException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}