using CartonCaps.Referrals.Services.Enums;

namespace CartonCaps.Referrals.Services.Models.Shared;

/// <summary>
/// Represents the result of an operation that can succeed or fail.
/// </summary>
/// <remarks>Use this class to encapsulate the outcome of an operation, including any returned data, success
/// status, and error details. Use this instead of throwing exceptions.</remarks>
/// <typeparam name="T">The type of the value returned by the operation if it is successful.</typeparam>
public class ResultOf<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
    public IEnumerable<ErrorCode> ErrorCodes { get; set; } = [];

    public ResultOf() { }

    /// <summary>
    /// Constructor for successful result
    /// </summary>
    /// <param name="data"></param>
    public ResultOf(T data)
    {
        Data = data;
        Success = true;
    }

    /// <summary>
    /// Constructor for error result
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="errorCode"></param>
    public ResultOf(string errorMessage, ErrorCode errorCode)
    {
        SetError(errorMessage, errorCode);
    }

    public void SetError(string errorMessage, ErrorCode errorCode)
    {
        Success = false;
        Errors = Errors.Append(errorMessage);
        ErrorCodes = ErrorCodes.Append(errorCode);
    }

    /// <summary>
    /// Allows implicit conversion from T to ResultOf<T> for successful results, enabling more
    /// concise code when returning successful outcomes from methods. For example, instead of writing
    /// 'return new ResultOf<ReferralResponse>(referralResponse);', you can simply write 'return referralResponse;'.
    /// This enhances readability and reduces boilerplate code when the operation is successful.
    /// </summary>
    /// <param name="data"></param>
    public static implicit operator ResultOf<T>(T data) => new(data);
}
