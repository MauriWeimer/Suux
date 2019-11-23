using BusinessLayout.Helper;
using Reports.Liquidations;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Reports.Helper
{
    public class GetReports
    {
        private static SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(GetConnectionString.GetConnection());

        public static async Task<ReceiptRPT> GetReceipts(int[] fileNs, int liquidationFixedDataId)
        {
            ReceiptRPT receiptRPT = new ReceiptRPT();

            await Task.Run(() =>
            {     
                receiptRPT.DataSourceConnections[0].SetConnection(builder.DataSource, builder.InitialCatalog, true);

                receiptRPT.SetParameterValue(receiptRPT.Parameter_EmployeesFileN.ParameterFieldName, fileNs);
                receiptRPT.SetParameterValue(receiptRPT.Parameter_liquidationFixedDataId.ParameterFieldName, liquidationFixedDataId);
            });

            return receiptRPT;
        }

        public static async Task<ReceiptBookRPT> GetReceiptsBook(int[] liquidationFixedDatasId, bool hideHeader, bool showTotals, int folioN)
        {
            ReceiptBookRPT receiptBookRPT = new ReceiptBookRPT();            

            await Task.Run(() =>
            {
                receiptBookRPT.DataSourceConnections[0].SetConnection(builder.DataSource, builder.InitialCatalog, true);

                receiptBookRPT.SetParameterValue(receiptBookRPT.Parameter_LiquidationFixedDataId.ParameterFieldName, liquidationFixedDatasId);
                receiptBookRPT.SetParameterValue(receiptBookRPT.Parameter_hideHeader.ParameterFieldName, hideHeader);
                receiptBookRPT.SetParameterValue(receiptBookRPT.Parameter_showTotals.ParameterFieldName, showTotals);
                receiptBookRPT.SetParameterValue(receiptBookRPT.Parameter_folio.ParameterFieldName, folioN);
            });

            return receiptBookRPT;
        }
    }
}
