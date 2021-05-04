using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentsLib.Entities
{
    public class ActiveHours
    {
        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("end")]
        public string End { get; set; }
    }
}
