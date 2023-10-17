using KlusterG.ArgumentModel.Sample.Models;

namespace KlusterG.ArgumentModel.Sample;
public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var model = ConvertArgument.DefineModel<Person>(args);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
