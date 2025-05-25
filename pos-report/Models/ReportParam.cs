using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pos_report.Models
{
    public class ReportParam
    {
        public string ReportName { get; set; }
        public string ID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}