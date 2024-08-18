using System;
using System.Collections.Generic;
using System.Text;

namespace FF.Backend.Results
{
    public interface IResult
    {
        int StatusCode { get; set; }

        string Message { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}
