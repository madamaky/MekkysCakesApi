namespace MekkysCakes.Shared.CommonResult
{
    public class Result
    {
        protected readonly List<Error> _errors = [];
        public IReadOnlyList<Error> Errors => _errors;

        public bool IsSuccess => _errors.Count == 0;
        public bool IsFailure => !IsSuccess;

        protected Result() { }
        protected Result(Error error) => _errors.Add(error);
        protected Result(List<Error> erros) => _errors.AddRange(erros);


        // Static Factory Methods
        public static Result Ok() => new Result();
        public static Result Fail(Error error) => new Result(error);
        public static Result Fail(List<Error> errors) => new Result(errors);
    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Can not Access The Value Of Failed Result");

        private Result(TValue value) : base() => _value = value;
        private Result(Error error) : base(error) => _value = default!;
        private Result(List<Error> errors) : base(errors) => _value = default!;


        // Static Factory Methods
        public static Result<TValue> Ok(TValue value) => new(value);  // new Result<TValue>(value)
        public new static Result<TValue> Fail(Error error) => new(error);  // new Result<TValue>(error)
        public new static Result<TValue> Fail(List<Error> errors) => new(errors); // new Result<TValue>(errors)

        // Implicit Conversion Operators (Operator Overloading)
        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}
