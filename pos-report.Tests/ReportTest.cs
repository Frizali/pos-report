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
                ID = "45f8b720-92cc-47da-9e81-59e99fa9231e",
                FromDate = "",
                ToDate = "",
                ReportName = "Order"
            };

            await reportService.ExportXmlAndSchema(reportParam);
        }

        [TestMethod]
        public async Task Create_PDFReport_ShouldReturnStream()
        {
            var reportService = new ReportService();
            var reportParam = new ReportParam()
            {
                ID = "45f8b720-92cc-47da-9e81-59e99fa9231e",
                FromDate = "",
                ToDate = "",
                ReportName = "Order"
            };

            var stream = await reportService.GenerateReportPDF(reportParam);
        }
    }
}
