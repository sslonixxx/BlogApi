namespace blog_api.Exeptions;

public class CustomException: Exception
{
    public int ErrorCode { get; set; }

    public CustomException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}