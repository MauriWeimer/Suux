using Microsoft.Win32;
using Reports.Liquidations;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Reports.Helper
{
    public class ExportReports
    {
        public static async Task<bool> PrintOrExportReceipts(int[] fileNs, int liquidationFixedDataId, string type, string period, bool print)
        {
            ReceiptRPT receiptRPT;

            Nullable<Boolean> printed = null;
            Nullable<Boolean> exported = null;

            if (print)
            {
                PrintDialog printDialog = new PrintDialog();
                printed = printDialog.ShowDialog();

                if (printed == true)
                {
                    receiptRPT = await GetReports.GetReceipts(fileNs, liquidationFixedDataId);
                    receiptRPT.PrintToPrinter(1, false, 0, 0);

                    await BackupReceipts(fileNs, liquidationFixedDataId, type, period);
                }
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "LIQUIDACIÓN_" + type + "_" + period;
                saveFileDialog.Filter = "Documento PDF (.pdf)|*.pdf|Documento CrystalReports (*.rpt)|*.rpt";
                exported = saveFileDialog.ShowDialog();

                if (exported == true)
                {
                    receiptRPT = await GetReports.GetReceipts(fileNs, liquidationFixedDataId);
                    receiptRPT.ExportToDisk(saveFileDialog.FilterIndex == 1 ?
                        CrystalDecisions.Shared.ExportFormatType.PortableDocFormat :
                        CrystalDecisions.Shared.ExportFormatType.CrystalReport, saveFileDialog.FileName);

                    await BackupReceipts(fileNs, liquidationFixedDataId, type, period);
                }
            }

            return (bool)(printed ?? exported);
        }        

        public static async Task BackupReceipts(int[] fileNs, int liquidationFixedDataId, string type, string period)
        {
            string docsDirec = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string suuxDirec = Path.Combine(docsDirec, "SUUX");
            string receiptsDirec = Path.Combine(suuxDirec, "Recibos");
            string periodDirec = Path.Combine(receiptsDirec, period);

            await Task.Run(() =>
            {
                if (!Directory.Exists(suuxDirec))
                    Directory.CreateDirectory(suuxDirec);
                if (!Directory.Exists(receiptsDirec))
                    Directory.CreateDirectory(receiptsDirec);

                if (!Directory.Exists(periodDirec))
                    Directory.CreateDirectory(periodDirec);                
            });

            foreach (int fileN in fileNs)
            {
                ReceiptRPT receiptRPT = await GetReports.GetReceipts(new int[] { fileN }, liquidationFixedDataId);

                await Task.Run(() => receiptRPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                    Path.Combine(periodDirec, fileN.ToString() + "_" + type + ".pdf")));
            }
        }

        public static async Task<bool> PrintOrExportReceiptsBook(int[] liquidationFixedDatasId, bool hideHeader, bool showTotals, int folioN, string period, bool print)
        {
            ReceiptBookRPT receiptBookRPT = await GetReports.GetReceiptsBook(liquidationFixedDatasId, hideHeader, showTotals, folioN);

            Nullable<Boolean> printed = null;
            Nullable<Boolean> exported = null;

            if (print)
            {
                PrintDialog printDialog = new PrintDialog();
                printed = printDialog.ShowDialog();

                if (printed == true)
                {
                    receiptBookRPT.PrintToPrinter(1, false, 0, 0);
                }
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "LIBRO_DE_SUELDOS_" + period;
                saveFileDialog.Filter = "Documento PDF (.pdf)|*.pdf|Document CrystalReports (*.rpt)|*.rpt";
                exported = saveFileDialog.ShowDialog();

                if (exported == true)
                {
                    receiptBookRPT.ExportToDisk(saveFileDialog.FilterIndex == 1 ?
                        CrystalDecisions.Shared.ExportFormatType.PortableDocFormat :
                        CrystalDecisions.Shared.ExportFormatType.CrystalReport, saveFileDialog.FileName);
                }
            }

            return (bool)(printed ?? exported);
        }        

        public static async Task BackupReceiptsBook(int[] liquidationFixedDatasId, bool hideHeader, bool showTotals, int folioN, string period)
        {
            string docsDirec = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string suuxDirec = Path.Combine(docsDirec, "SUUX");
            string receiptsDirec = Path.Combine(suuxDirec, "Libros");

            await Task.Run(() =>
            {
                if (!Directory.Exists(suuxDirec))
                    Directory.CreateDirectory(suuxDirec);
                if (!Directory.Exists(receiptsDirec))
                    Directory.CreateDirectory(receiptsDirec);
            });

            ReceiptBookRPT receiptBookRPT = await GetReports.GetReceiptsBook(liquidationFixedDatasId, hideHeader, showTotals, folioN);
            await Task.Run(() => receiptBookRPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat,
                    Path.Combine(receiptsDirec, period + ".pdf")));
        }
    }
}
