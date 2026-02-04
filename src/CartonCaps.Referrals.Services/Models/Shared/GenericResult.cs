using CartonCaps.Referrals.Services.Enums;

namespace CartonCaps.Referrals.Services.Models.Shared;

/// <summary>
/// Represents the result of an operation that can succeed or fail.
/// </summary>
/// <remarks>Use this class to encapsulate the outcome of an operation, including any returned data, success
/// status, and error details. Use this instead of throwing exceptions.</remarks>
/// <typeparam name="T">The type of the value returned by the operation if it is successful.</typeparam>
public class GenericResult<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
    public IEnumerable<ErrorCode> ErrorCodes { get; set; } = [];

    public GenericResult() { }

    /// <summary>
    /// Constructor for successful result
    /// </summary>
    /// <param name="data"></param>
    public GenericResult(T data)
    {
        Data = data;
        Success = true;
    }

    /// <summary>
    /// Constructor for error result
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="errorCode"></param>
    public GenericResult(string errorMessage, ErrorCode errorCode)
    {
        SetError(errorMessage, errorCode);
    }

    public void SetError(string errorMessage, ErrorCode errorCode)
    {
        Success = false;
        Errors = Errors.Append(errorMessage);
        ErrorCodes = ErrorCodes.Append(errorCode);
    }
}
