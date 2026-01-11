using CManager.Application.Services;
using CManager.Infrastructure.Data;
using CManager.Presentation.ConsoleApp.Controllers;

namespace CManager.Presentation.ConsoleApp
{
    class Program
    {
       static void Main(string[] args) 
        {
           try 
           {
            var appDbContext = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDirectory = Path.Combine(appDbContext, "CManager");
            var filePath = Path.Combine(appDirectory, "customers.json");

            Directory.CreateDirectory(appDirectory);

            var respository = new CustomerRepository(filePath);
            var service = new CustomerService(respository);
            var Controller = new CustomerController(service);
        
            Controller.RunMenu();
           }

           catch (Exception ex)
           {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
           }
    }  }
}
