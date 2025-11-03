using System;

namespace VendingMachine
{
    public class CustomerService
    {
        private VendingMachine _machine;

        public CustomerService(VendingMachine machine)
        {
            _machine = machine;
        }

        public void Run()
        {
            bool inCustomerMode = true;

            while (inCustomerMode)
            {
                Console.Clear();
                Console.WriteLine("=== РЕЖИМ ПОКУПАТЕЛЯ ===");
                _machine.DisplayProducts();
                Console.WriteLine($"\nТекущий баланс: {_machine.GetCurrentBalance()} руб.");
                Console.WriteLine("1. Внесите монеты");
                Console.WriteLine("2. Выбрать товар");
                Console.WriteLine("3. Отменить операцию");
                Console.WriteLine("4. Вернуться в главное меню");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        InsertCoins();
                        break;
                    case "2":
                        SelectProduct();
                        break;
                    case "3":
                        _machine.CancelOperation();
                        Console.WriteLine("Нажмите любую клавишу для продолжения...");
                        Console.ReadKey();
                        break;
                    case "4":
                        inCustomerMode = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void InsertCoins()
        {
            Console.WriteLine("\nДоступные номиналы: 1, 2, 5, 10 рублей");
            Console.Write("Введите номинал монеты: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal coinAmount))
            {
                if (coinAmount == 1 || coinAmount == 2 || coinAmount == 5 || coinAmount == 10)
                {
                    _machine.InsertCoin(coinAmount);
                }
                else
                {
                    Console.WriteLine("Автомат не принимает монеты такого номинала!");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат суммы!");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        private void SelectProduct()
        {
            Console.Write("Введите номер товара: ");

            if (int.TryParse(Console.ReadLine(), out int productNumber))
            {
                int productIndex = productNumber - 1;
                bool success = _machine.PurchaseProduct(productIndex);
                
                if (success)
                {
                    Console.WriteLine("Покупка успешно завершена!");
                }
                else
                {
                    Console.WriteLine("Не удалось совершить покупку.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат номера!");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}