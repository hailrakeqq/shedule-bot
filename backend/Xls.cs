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
    }
}