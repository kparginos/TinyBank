using TinyBank.Core.Consts;

namespace TinyBank.Core.Services.Results
{
    public class Result<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public bool IsSuccess()
        {
            return Code == ResultCodes.Success;
        }
    }
}
