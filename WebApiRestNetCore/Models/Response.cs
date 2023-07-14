namespace WebApiRestNetCore.Models
{
    public class Response<T>
    {
        public T Data { get; set; } 

        public bool IsSuccess { get; set; }
        //public bool IsError { get; set; }
        public string Message { get; set; }
    }
}
