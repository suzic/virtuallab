using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.API.Service.po
{
    public class ConsoleSendReq
    {
        public string session_id { get; set; }
        public string device_id { get; set; }
        public string app_name { get; set; }
        public string input_line { get; set; }


        public string ssh_uuid { get; set; }
        public string cmd { get; set; }

    }
}