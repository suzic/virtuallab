using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.API.Service.po
{
    public class DeviceRequestRes
    {
        public int fail { get; set; }
        public string device_id { get; set; }
        public string ssh_uuid { get; set; }
        public int total_count { get; set; }
        public int lock_count { get; set; }
        public int fail_count { get; set; }

    }
}