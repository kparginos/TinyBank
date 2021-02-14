using System.Collections.Generic;

namespace TinyBank.Core.Services.Results
{
    public class ResultList<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}
