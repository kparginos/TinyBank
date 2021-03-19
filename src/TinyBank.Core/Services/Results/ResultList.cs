using System.Collections.Generic;

namespace TinyBank.Core.Services.Results
{
    public class ResultList<T> : Result<T>
    {
        //public int Code { get; set; }
        //public string Message { get; set; }
        public new List<T> Data { get; set; }
    }
}
