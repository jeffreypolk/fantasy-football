using System.Threading.Tasks;

namespace FF.Backend.Results
{
    public class Result : IResult
    {
        protected const string MessageFail = "An unexpected error occurred, please contact support if this error continues";
        protected const string MessageFailNotFound = "The item was not found";
        protected const string MessageFailValidation = "A validation error occurred";
        protected const string MessageFailForbidden = "You do not have authorization";

        public int StatusCode { get; set; } = 200;

        public string Message { get; set; }

        public bool Succeeded { get; set; } = true;

        public Result()
        {
        }

        #region Success Results

        public static IResult Success()
        {
            return new Result();
        }

        public static IResult Success(string message)
        {
            return new Result { Message = message };
        }

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        #endregion

        #region Fail Results

        public static IResult Fail()
        {
            return new Result { Succeeded = false, StatusCode = 500, Message = MessageFail };
        }

        public static IResult Fail(string message)
        {
            return new Result { Succeeded = false, StatusCode = 500, Message = message };
        }

        public static Task<IResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<IResult> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        #endregion

        #region Fail Not Found Results

        public static IResult FailNotFound()
        {
            return new Result { Succeeded = false, StatusCode = 404, Message = MessageFailNotFound };
        }

        public static IResult FailNotFound(string message)
        {
            return new Result { Succeeded = false, StatusCode = 404, Message = message };
        }

        public static Task<IResult> FailNotFoundAsync()
        {
            return Task.FromResult(FailNotFound());
        }

        public static Task<IResult> FailNotFoundAsync(string message)
        {
            return Task.FromResult(FailNotFound(message));
        }

        #endregion

        #region Fail Validation Results

        public static IResult FailValidation()
        {
            return new Result { Succeeded = false, StatusCode = 400, Message = MessageFailValidation };
        }

        public static IResult FailValidation(string message)
        {
            return new Result { Succeeded = false, StatusCode = 400, Message = message };
        }

        public static Task<IResult> FailValidationAsync()
        {
            return Task.FromResult(FailValidation());
        }

        public static Task<IResult> FailValidationAsync(string message)
        {
            return Task.FromResult(FailValidation(message));
        }

        #endregion

        #region Fail Forbidden Results

        public static IResult FailForbidden()
        {
            return new Result { Succeeded = false, StatusCode = 403, Message = MessageFailForbidden };
        }

        public static IResult FailForbidden(string message)
        {
            return new Result { Succeeded = false, StatusCode = 403, Message = message };
        }

        public static Task<IResult> FailForbiddenAsync()
        {
            return Task.FromResult(FailForbidden());
        }

        public static Task<IResult> FailForbiddenAsync(string message)
        {
            return Task.FromResult(FailForbidden(message));
        }

        #endregion
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result()
        {
        }

        public Result(T data)
        {
            Data = data;
        }

        public T Data { get; set; }

        #region Success Results

        public static new Result<T> Success()
        {
            return new Result<T>();
        }

        public static new Result<T> Success(string message)
        {
            return new Result<T> { Message = message };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Data = data };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T> { Data = data, Message = message };
        }

        public static new Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static new Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }

        #endregion

        #region Fail Results

        public static new Result<T> Fail()
        {
            return new Result<T> { Succeeded = false, StatusCode = 500, Message = MessageFail };
        }

        public static new Result<T> Fail(string message)
        {
            return new Result<T> { Succeeded = false, StatusCode = 500, Message = message };
        }

        public static new Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static new Task<Result<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        #endregion

        #region Fail Not Found Results

        public static new Result<T> FailNotFound()
        {
            return new Result<T> { Succeeded = false, StatusCode = 404, Message = MessageFailNotFound };
        }

        public static new Result<T> FailNotFound(string message)
        {
            return new Result<T> { Succeeded = false, StatusCode = 404, Message = message };
        }

        public static new Task<Result<T>> FailNotFoundAsync()
        {
            return Task.FromResult(FailNotFound());
        }

        public static new Task<Result<T>> FailNotFoundAsync(string message)
        {
            return Task.FromResult(FailNotFound(message));
        }

        #endregion

        #region Fail Validation Results

        public static new Result<T> FailValidation()
        {
            return new Result<T> { Succeeded = false, StatusCode = 400, Message = MessageFailValidation };
        }

        public static new Result<T> FailValidation(string message)
        {
            return new Result<T> { Succeeded = false, StatusCode = 400, Message = message };
        }

        public static new Task<Result<T>> FailValidationAsync()
        {
            return Task.FromResult(FailValidation());
        }

        public static new Task<Result<T>> FailValidationAsync(string message)
        {
            return Task.FromResult(FailValidation(message));

        }

        #endregion

        #region Fail Forbidden Results

        public static new Result<T> FailForbidden()
        {
            return new Result<T> { Succeeded = false, StatusCode = 403, Message = MessageFailForbidden };
        }

        public static new Result<T> FailForbidden(string message)
        {
            return new Result<T> { Succeeded = false, StatusCode = 403, Message = message };
        }

        public static new Task<Result<T>> FailForbiddenAsync()
        {
            return Task.FromResult(FailForbidden());
        }

        public static new Task<Result<T>> FailForbiddenAsync(string message)
        {
            return Task.FromResult(FailForbidden(message));
        }

        #endregion
    }
}