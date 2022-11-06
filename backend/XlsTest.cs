using ClosedXML.Excel;
namespace shedule_bot.backend
{
    public class XlsTest
    {
        public static string TestView()
        {
            using (var workbook = new XLWorkbook("./test.xlsx"))
            {
                var worksheet = workbook.Worksheet("Sample Sheet");

                string? test = worksheet.Cell("A1").Value.ToString();
                return test;
                // worksheet.Cell("A1").Value = "Hello World!";
                // workbook.SaveAs("test.xlsx");
            }
        }

        public static void TestCreate()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sample Sheet");
                worksheet.Cell("A1").Value = "Hello World!";
                workbook.SaveAs("test.xlsx");
            }
        }
        public static void TestChange()
        {
            using (var workbook = new XLWorkbook("./test.xlsx"))
            {
                var currentWorksheet = workbook.Worksheet("Sample Sheet");
                currentWorksheet.Cell("A1").Value = "qqe";
                workbook.Save();
            }
        }
    }
}