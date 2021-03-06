using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace chat_app.Models
{
    public class AppConfigurations
    {
        public string LocalHost { get; set; }
        public string RemoteHost { get; set; }
        public bool IsUsingLocalHost { get; set; }
    }
}
