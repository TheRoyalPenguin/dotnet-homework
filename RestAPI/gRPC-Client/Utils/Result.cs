namespace gRPC_Client.Utils;

public class Result
{
    public bool IsSuccess { get; set; }
    public int Status { get; set; }
    public string? Message { get; set; }

    public Result(bool isSuccess, int status, string message)
    {
        IsSuccess = isSuccess;
        Status = status;
        Message = message;
    }
    
    public static Result Success(int status, string message)
    {
        return new Result(true, status, message);
    }

    public static Result Error(int status, string message)
    {
        return new Result(false, status, message);
    }
}
