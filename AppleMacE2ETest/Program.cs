using System.Threading.Tasks;

namespace AppleE2ETest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await TestRunner.RunTestAsync();
        }
    }
}