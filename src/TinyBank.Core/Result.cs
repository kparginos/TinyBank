
namespace TinyBank.Core
{
    public class Result<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
