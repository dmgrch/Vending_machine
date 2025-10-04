using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace VendingMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            VendingMachine machine = new VendingMachine();

            bool isRunning = true;
            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("=== ВЕНДИНГОВЫЙ АВТОМАТ ===");
                Console.WriteLine("1. Режим покупателя");
                Console.WriteLine("2. Режим администратора");
                Console.WriteLine("3. Выход");
                Console.Write("Выберите режим: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CustomerMode(machine);
                        break;
                    case "2":
                        AdminMode(machine);
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

            static void CustomerMode(VendingMachine machine)
            {
                bool inCustomerMode = true;

                while (inCustomerMode)
                {
                    Console.Clear();
                    Console.WriteLine("=== РЕЖИМ ПОКУПАТЕЛЯ ===");
                    machine.DisplayProducts();
                    Console.WriteLine($"\nТекущий баланс: {machine.GetCurrentBalance()} руб.");
                    Console.WriteLine("1. Внесите монеты");
                    Console.WriteLine("2. Выбрать товар");
                    Console.WriteLine("3. Отменить операцию");
                    Console.WriteLine("4. Вернуться в главное меню");
                    Console.Write("Выберите действие: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("\nДоступные номиналы: 1, 2, 5, 10 рублей");
                            Console.Write("Введите номинал моенты: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal coinAmount))
                            {
                                if (coinAmount == 1 || coinAmount == 2 || coinAmount == 5 || coinAmount == 10)
                                {
                                    machine.InsertCoin(coinAmount);
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
                            break;
                        case "2":
                            Console.Write("Введите номер товара: ");

                            if (int.TryParse(Console.ReadLine(), out int productNumber))
                            {
                                int productIndex = productNumber - 1;
                                machine.PurchaseProduct(productIndex);
                            }
                            else
                            {
                                Console.WriteLine("Неверный формат номера!");
                            }

                            Console.WriteLine("Нажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        case "3":
                            machine.CancelOperation();
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
            
            static void AdminMode(VendingMachine machine)
            {
                Console.Write("Введите пароль: ");
                string password = Console.ReadLine();

                if (password != "admin")
                {
                    Console.WriteLine("Неверный пароль!");
                    Console.ReadKey();
                    return;
                }

                bool inAdminMode = true;

                while (inAdminMode)
                {
                    Console.Clear();
                    Console.WriteLine("=== РЕЖИМ АДМИНИСТРАТОРА ===");
                    machine.DisplayProducts();
                    machine.DisplayCoins();

                    Console.WriteLine("\n1. Пополнить товар");
                    Console.WriteLine("2. Добавить новый товар");
                    Console.WriteLine("3. Собрать деньги");
                    Console.WriteLine("4. Вернуться в главное меню");
                    Console.Write("Выберите действие: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Введите номер товара для пополнения: ");
                            if (int.TryParse(Console.ReadLine(), out int restockProductNumber))
                            {
                                int productIndex = restockProductNumber - 1;

                                Console.Write("Введите количество для добавления: ");
                                if (int.TryParse(Console.ReadLine(), out int restockQuantity))
                                {
                                    machine.RestockProduct(productIndex, restockQuantity);
                                }
                                else
                                {
                                    Console.WriteLine("Неверное количетсво!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Неверный номер товара!");
                            }
                            Console.WriteLine("Нажмите любую кнопку для продолжения...");
                            Console.ReadKey();
                            break;
                        case "2":
                            Console.Write("Введите название товара: ");
                            string productName = Console.ReadLine();

                            Console.Write("Введите цену товара: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal productPrice) && productPrice > 0)
                            {
                                Console.Write("Введите количество товара: ");
                                if (int.TryParse(Console.ReadLine(), out int productQuantity) && productQuantity > 0)
                                {
                                    machine.AddNewProduct(productName, productPrice, productQuantity);
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
                            break;
                        case "3":
                            machine.CollectMoney();
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
        }

        public class Product
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }

            public Product(string name, decimal price, int quantity)
            {
                Name = name;
                Price = price;
                Quantity = quantity;
            }

            public override string ToString()
            {
                return $"{Name} - {Price} руб. (осталось: {Quantity})";
            }
        }

        public class Coin
        {
            public decimal Denomination { get; set; }
            public int Count { get; set; }

            public Coin(decimal denomination, int count)
            {
                Denomination = denomination;
                Count = count;
            }
            public override string ToString()
            {
                return $"{Denomination} руб. - {Count} монет";
            }
        }

        public class VendingMachine
        {
            private List<Product> products;
            private List<Coin> coins;

            private decimal currentBalance;

            public VendingMachine()
            {
                products = new List<Product>();
                coins = new List<Coin>();
                currentBalance = 0;

                InitilizeProducts();
                InitializeCoins();
            }

            private void InitilizeProducts()
            {
                products.Add(new Product("Вода", 50, 10));
                products.Add(new Product("Шоколад", 80, 5));
                products.Add(new Product("Чипсы", 120, 8));
            }

            private void InitializeCoins()
            {
                coins.Add(new Coin(1, 10));
                coins.Add(new Coin(2, 10));
                coins.Add(new Coin(5, 10));
                coins.Add(new Coin(10, 10));
            }
            public void DisplayProducts()
            {
                Console.WriteLine("\n=== ДОСТУПНЫЕ ТОВАРЫ ===");
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {products[i]}");
                }
            }

            public void InsertCoin(decimal amount)
            {
                currentBalance += amount;
                foreach (var coin in coins)
                {
                    if (coin.Denomination == amount)
                    {
                        coin.Count++;
                        break;
                    }
                }
                Console.WriteLine($"Внесено: {amount} руб. Текущий баланс: {currentBalance} руб.");
            }

            public decimal GetCurrentBalance()
            {
                return currentBalance;
            }

            public bool PurchaseProduct(int productIndex)
            {
                if (productIndex < 0 || productIndex >= products.Count)
                {
                    Console.WriteLine("Неверный номер товара!");
                    return false;
                }

                Product selectedProduct = products[productIndex];

                if (selectedProduct.Quantity <= 0)
                {
                    Console.WriteLine("Товар закончился!");
                    return false;
                }

                if (currentBalance < selectedProduct.Price)
                {
                    Console.WriteLine($"Недостаточно средств! Нужно: {selectedProduct.Price} руб., у вас {currentBalance} руб.");
                    return false;
                }

                selectedProduct.Quantity--;
                currentBalance -= selectedProduct.Price;

                Console.WriteLine($"Вы купили: {selectedProduct.Name}");
                Console.WriteLine($"Сдача: {currentBalance} руб.");

                GiveChange();

                return true;
            }

            private void GiveChange()
            {
                if (currentBalance > 0)
                {
                    Console.WriteLine($"Выдаем сдачу: {currentBalance} руб.");
                    currentBalance = 0;
                }
            }

            public void CancelOperation()
            {
                if (currentBalance > 0)
                {
                    Console.WriteLine($"Операция отменена. Возврат: {currentBalance} руб.");
                    GiveChange();
                }
            }
            public void DisplayCoins()
            {
                Console.WriteLine("\n=== МОНЕТЫ В АВТОМАТЕ ===");
                foreach (var coin in coins)
                {
                    Console.WriteLine(coin);
                }
                Console.WriteLine($"Общая сумма: {GetTotalMoney()} руб.");
            }
            public decimal GetTotalMoney()
            {
                decimal total = 0;
                foreach (var coin in coins)
                {
                    total += coin.Denomination * coin.Count;
                }
                return total;
            }
            public void RestockProduct(int productIndex, int quantity)
            {
                if (productIndex >= 0 && productIndex < products.Count)
                {
                    products[productIndex].Quantity += quantity;
                    Console.WriteLine($"Товар пополнен! Новое количество: {products[productIndex].Quantity}");
                }
                else
                {
                    Console.WriteLine("Неверный номер товара!");
                }
            }
            public void AddNewProduct(string name, decimal price, int quantity)
            {
                products.Add(new Product(name, price, quantity));
                Console.WriteLine($"Новый товар добавлен: {name}");
            }
            public decimal CollectMoney()
            {
                decimal collectAmount = GetTotalMoney();

                foreach (var coin in coins)
                {
                    coin.Count = 0;
                }

                Console.WriteLine($"Собрано: {collectAmount} руб.");
                return collectAmount;
            }
        }
    }
}
