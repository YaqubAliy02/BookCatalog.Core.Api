namespace Application.Models
{
    public class ResponseCore<T>
    {
        public int StatusCode { get; set; } = 200;
        public object[] ErrorMessage { get; set; }
        public T Result { get; set; }
    }
}
