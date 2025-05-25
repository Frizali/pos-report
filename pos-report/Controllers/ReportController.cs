using pos_report.Models;
using pos_report.Services;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace pos_report.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController()
        {
            _service = new ReportService();
        }

        [Route("report")]
        [HttpGet]
        public async Task<object> Index(ReportParam param)
        {
            return await _service.GenerateReportPDF(param);
        }

    }
}
