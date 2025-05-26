using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Dapper;
using pos_report.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace pos_report.Services
{
    public class ReportService : IReportService
    {
        private readonly string _connectionString = "server=(localdb)\\MSSQLLocalDB;database=POS;Trusted_Connection=true;TrustServerCertificate=True;";

        public async Task<object> GenerateReportPDF(ReportParam param)
        {
            var ds = new DataSet();
            TblReport tblReport = await GetDataTblReport(param).ConfigureAwait(false);

            using (var con = new SqlConnection(_connectionString))
            {
                await con.OpenAsync().ConfigureAwait(false);

                var daHeader = new SqlDataAdapter(GetQueryHeader(param), con);
                daHeader.Fill(ds, "Header");

                string query = $"SELECT * FROM dbo.{tblReport.strFunction}('{param.ID}', '{param.FromDate}', '{param.ToDate}')";
                var da = new SqlDataAdapter(query, con);
                da.Fill(ds, "Data");

                if (tblReport.strFunction1 != null)
                {
                    string query1 = $"SELECT * FROM dbo.{tblReport.strFunction1}('{param.ID}', '{param.FromDate}', '{param.ToDate}')";
                    var da1 = new SqlDataAdapter(query1, con);
                    da1.Fill(ds, "Data1");
                }
            }

            MemoryStream output = new MemoryStream();
            using (var rptDoc = new ReportDocument())
            {
                string reportPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Reports",
                    $"{tblReport.rptFileName}.rpt"
                );
                rptDoc.Load(reportPath);
                rptDoc.SetDataSource(ds);

                using (var exportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat))
                {
                    exportStream.CopyTo(output);

                    output.Position = 0;
                }
            }

            using (MemoryStream ms = new MemoryStream())
            {
                output.CopyTo(ms);
                byte[] byteArray = ms.ToArray();
                return new {
                    FileName = tblReport.rptFileName,
                    Data = Convert.ToBase64String(byteArray)
                };
            }
        }


        public async Task ExportXmlAndSchema(ReportParam param)
        {
            var ds = new DataSet();
            TblReport tblReport = await GetDataTblReport(param).ConfigureAwait(false);

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();

                SqlDataAdapter daHeader = new SqlDataAdapter(GetQueryHeader(param), con);
                daHeader.Fill(ds, "Header");

                string query = $"SELECT * FROM dbo.{tblReport.strFunction}('{param.ID}', '{param.FromDate}', '{param.ToDate}')";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(ds, "Data");

                if (tblReport.strFunction1 != null)
                {
                    string query1 = $"SELECT * FROM dbo.{tblReport.strFunction1}('{param.ID}', '{param.FromDate}', '{param.ToDate}')";
                    var da1 = new SqlDataAdapter(query1, con);
                    da1.Fill(ds, "Data1");
                }
            }

            ds.WriteXml($"D:\\pos-report\\{tblReport.rptFileName}.xml", XmlWriteMode.WriteSchema);
            ds.WriteXmlSchema($"D:\\pos-report\\{tblReport.rptFileName}.xsd");
        }

        private string GetQueryHeader(ReportParam param)
        {
            return $"SELECT 'Angkringan OmahMU' AS Owner, '{param.ID}' AS ID, '{param.FromDate}' AS FromDate, '{param.ToDate}' AS ToDate";
        }

        private async Task<TblReport> GetDataTblReport(ReportParam param)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM tblReport WHERE rptName = @rptName";
                return await con.QueryFirstOrDefaultAsync<TblReport>(sql, new { rptName = param.ReportName });
            }
        }
    }
}