using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Core
{
    public class Result<T>
    {
        private Result(T value)
        {
            isSuccess = true;
            Value = value;
            Error = Error.None;
        }

        private Result(Error error)
        {
            isSuccess = false;
            Error = error;
        }

        public bool isSuccess { get; }
        public T? Value { get; }
        public Error Error { get; }

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(Error error) => new(error);
    }
}
