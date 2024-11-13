using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace source_service.Model
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = null!;

        public string Port { get; set; } = null!;
    }
}