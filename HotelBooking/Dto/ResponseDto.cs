namespace Hotel.Dto
{
    public class ResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object? Payload { get; set; }
    }
}
