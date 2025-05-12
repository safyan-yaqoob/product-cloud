namespace ProductCloud.SharedKernal.Messaging.Abstractions
{
    public class BrokerOptions
    {
        public string Host { get; set; } = string.Empty;
        public int RetryCount { get; set; } = 3;
    }

}
