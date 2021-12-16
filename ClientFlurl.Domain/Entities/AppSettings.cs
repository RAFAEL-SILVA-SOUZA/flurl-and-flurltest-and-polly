namespace ClientFlurl.Entities
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public int PollyRetryCount { get; set; }
        public int[] PollyRetryStatusCodes { get; set; }
    }
}
