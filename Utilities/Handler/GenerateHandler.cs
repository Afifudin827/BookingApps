using Server.DTOs.Employees;
using Server.Models;

namespace Server.Utilities.Handler;

public class GenerateHandler
{
    public static string NikGenerate(IEnumerable<Employee> employee)
    {
        int i = 111111;
        foreach (var item in employee)
        {
            if(int.TryParse(item.NIK, out i))
            {
                return i.ToString();
            }

        }
        return "111111";
    }
}
