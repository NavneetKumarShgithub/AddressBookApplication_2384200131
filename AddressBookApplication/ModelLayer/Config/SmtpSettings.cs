using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Config
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string EnableSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
