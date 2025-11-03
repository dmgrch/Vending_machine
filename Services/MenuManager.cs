using System;

namespace VendingMachine
{
    public class MenuManager
    {
        private CustomerService _customerService;
        private AdminService _adminService;

        public MenuManager(VendingMachine machine)
        {
            _customerService = new CustomerService(machine);
            _adminService = new AdminService(machine);
        }

        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("=== ВЕНДИНГОВЫЙ АВТОМАТ ===");
                Console.WriteLine("1. Режим покупателя");
                Console.WriteLine("2. Режим администратора");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите режим: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        _customerService.Run();
                        break;
                    case "2":
                        _adminService.Run();
                        break;
                    case "3":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}