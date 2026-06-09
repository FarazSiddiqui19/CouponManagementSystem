namespace CouponManagementSystem.DTO;

public class ServiceResult<T>
{
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }

    public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

    public static ServiceResult<T> Success(T data) =>
        new() { Data = data, StatusCode = 200 };

    public static ServiceResult<T> Failure(int statusCode, string error) =>
        new() { StatusCode = statusCode, Error = error };
}
