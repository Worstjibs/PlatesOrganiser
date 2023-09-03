using System.Text.Json.Serialization;

namespace PlatesOrganiser.Domain.Shared;

public class Result
{
    protected internal Result(bool isSuccess, Error error, string? message = null, Guid? newEntityId = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        Message = message;
        NewEntityId = newEntityId;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public string? Message { get; }
    public Guid? NewEntityId { get; }

    public static Result Success(Guid? newEntityId = null) => new(isSuccess: true, error: Error.None, newEntityId: newEntityId);
    public static Result Failure(Error error, string? message = null) => new(isSuccess: false, error: error, message: message);

    public static Result<T> Create<T>(T value) => new(value, true, Error.None, null);
    public static Result<T> Failure<T>(Error error, string? message = null) => new(default, false, error, message);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error, string? message)
        : base(isSuccess, error, message) =>
        _value = value;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TValue Value => _value!;

    public static implicit operator Result<TValue>(TValue value) => Create(value);
}
