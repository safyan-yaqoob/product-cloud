using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernal.Messaging.Abstractions
{
    public class BrokerOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExchangeType { get; set; } = "topic";
        public int RetryCount { get; set; } = 5;
    }
}
