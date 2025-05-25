using pos_report.Models;
using System.IO;
using System.Threading.Tasks;

namespace pos_report.Services
{
    interface IReportService
    {
        Task<object> GenerateReportPDF(ReportParam param);
        Task ExportXmlAndSchema(ReportParam param);
    }
}
