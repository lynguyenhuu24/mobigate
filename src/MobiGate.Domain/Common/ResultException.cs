namespace MobiGate.Domain.Common;

public class ResultException : Exception
{
    public int StatusCode { get; }

    public ResultException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}
