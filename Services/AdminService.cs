using System;

namespace VendingMachine
{
    public class AdminService
    {
        private VendingMachine _machine;

        public AdminService(VendingMachine machine)
        {
            _machine = machine;
        }

        public void Run()
        {
            if (!Authenticate())
            {
                return;
            }

            bool inAdminMode = true;

            while (inAdminMode)
            {
                Console.Clear();
                Console.WriteLine("=== РЕЖИМ АДМИНИСТРАТОРА ===");
                _machine.DisplayProducts();
                _machine.DisplayCoins();

                Console.WriteLine("\n1. Пополнить товар");
                Console.WriteLine("2. Добавить новый товар");
                Console.WriteLine("3. Собрать деньги");
                Console.WriteLine("4. Вернуться в главное меню");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        RestockProduct();
                        break;
                    case "2":
                        AddNewProduct();
                        break;
                    case "3":
                        _machine.CollectMoney();
                        Console.ReadKey();
                        break;
                    case "4":
                        inAdminMode = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private bool Authenticate()
        {
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine() ?? "";

            if (password != "admin")
            {
                Console.WriteLine("Неверный пароль!");
                Console.ReadKey();
                return false;
            }

            return true;
        }

        private void RestockProduct()
        {
            Console.Write("Введите номер товара для пополнения: ");
            if (int.TryParse(Console.ReadLine(), out int restockProductNumber))
            {
                int productIndex = restockProductNumber - 1;

                Console.Write("Введите количество для добавления: ");
                if (int.TryParse(Console.ReadLine(), out int restockQuantity) && restockQuantity > 0)
                {
                    _machine.RestockProduct(productIndex, restockQuantity);
                }
                else
                {
                    Console.WriteLine("Неверное количество!");
                }
            }
            else
            {
                Console.WriteLine("Неверный номер товара!");
            }
            Console.WriteLine("Нажмите любую кнопку для продолжения...");
            Console.ReadKey();
        }

        private void AddNewProduct()
        {
            Console.Write("Введите название товара: ");
            string productName = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(productName))
            {
                Console.WriteLine("Название товара не может быть пустым!");
                Console.ReadKey();
                return;
            }

            Console.Write("Введите цену товара: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal productPrice) && productPrice > 0)
            {
                Console.Write("Введите количество товара: ");
                if (int.TryParse(Console.ReadLine(), out int productQuantity) && productQuantity > 0)
                {
                    _machine.AddNewProduct(productName, productPrice, productQuantity);
                }
                else
                {
                    Console.WriteLine("Неверное количество!");
                }
            }
            else
            {
                Console.WriteLine("Неверная цена!");
            }
            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}