using System;
using System.Collections.Generic;
using System.Text;

namespace FF.Backend.Results
{
    public class JsonStringResult: Result<string>
    {
        public JsonStringResult(string data)
        {
            this.Data = data;
        }
    }
}
