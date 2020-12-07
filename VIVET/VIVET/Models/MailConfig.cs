using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ehr.Models
{
    public class MailConfig
    {
        public int Id { get; set; }
        public string ServerAddress { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        public bool UseSSL { get; set; }
        public string EmailSend { get; set; }
        public string EmailCC { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}