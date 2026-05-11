namespace SmartHomeApp.Services
{
    // Parameterization/Generics untuk mencatat berbagai tipe data log
    public class AccessLog<T>
    {
        public T Content { get; set; }
        public DateTime LogTime { get; set; }

        public void SaveLog(T data)
        {
            Content = data;
            LogTime = DateTime.Now;
            Console.WriteLine($"[LOG {LogTime}] Data: {Content}");
        }
    }
}