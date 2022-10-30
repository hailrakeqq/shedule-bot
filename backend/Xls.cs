using System.Runtime.CompilerServices;
using System.Reflection.Metadata;
using ClosedXML.Excel;
namespace shedule_bot.backend
{
    public class Excel
    {
        public static void Print()
        {
            //B14:B17
            using (var workbook = new XLWorkbook("./main.xlsx"))
            {
                var currentWorksheet = workbook.Worksheet("Лист2");


                Console.WriteLine(currentWorksheet.Cells("AE14:AE15").ToString());
            }
        }

        public static void TestCreate()
        {
            using (var workbook = new XLWorkbook("./main.xlsx"))
            {
                var worksheet = workbook.Worksheets.Add("Sample Sheet");
                worksheet.Cell("A1").Value = "1111323";
                workbook.Save();
            }
        }
        public static void TestChange()
        {
            using (var workbook = new XLWorkbook("./main.xlsx"))
            {
                var currentWorksheet = workbook.Worksheet("Sample Sheet");
                currentWorksheet.Cell("A1").Value = "qqe";
                workbook.Save();
            }
        }
    }
}