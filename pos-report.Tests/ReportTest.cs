using Microsoft.VisualStudio.TestTools.UnitTesting;
using pos_report.Models;
using pos_report.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace pos_report.Tests
{
    [TestClass]
    public class ReportTest
    {
        [TestMethod]
        public async Task Create_XMLReport_ShouldSaved()
        {
            var reportService = new ReportService();
            var reportParam = new ReportParam()
            {
                ID = "",
                FromDate = "2025-05-01",
                ToDate = "2025-05-31",
                ReportName = "Sales Analytics"
            };

            await reportService.ExportXmlAndSchema(reportParam);
        }

        [TestMethod]
        public async Task Create_PDFReport_ShouldReturnStream()
        {
            var reportService = new ReportService();
            var reportParam = new ReportParam()
            {
                ID = "",
                FromDate = "2025-05-01",
                ToDate = "2025-05-31",
                ReportName = "Sales Analytics"
            };

            var stream = await reportService.GenerateReportPDF(reportParam);
        }
    }
}
