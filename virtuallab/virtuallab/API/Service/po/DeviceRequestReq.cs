using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.API.Service.po
{
    public class DeviceRequestReq
    {
        public string session_id { get; set; }
        public string device_type { get; set; }

    }
}