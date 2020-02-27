using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace mygkrnk.Models
{
    public class AnalyticsReport
    {
        public string Dimensions { get; set; }
        public string Views { get; set; }
    }
}
