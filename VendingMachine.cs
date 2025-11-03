using System;
using System.Collections.Generic;

namespace VendingMachine
{
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

            InitializeProducts();
            InitializeCoins();
        }

        private void InitializeProducts()
        {
            products.Add(new Product("Вода \"Святой источник\"", 25, 10));
            products.Add(new Product("Шоколад \"Milka\"", 40, 5));
            products.Add(new Product("Чипсы \"Lay's\"", 30, 8));
        }

        private void InitializeCoins()
        {
            coins.Add(new Coin(1, 15));
            coins.Add(new Coin(2, 15));
            coins.Add(new Coin(5, 15));
            coins.Add(new Coin(10, 15));
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

            decimal changeAmount = currentBalance - selectedProduct.Price;
    
            if (changeAmount > 0 && !CanGiveChange(changeAmount))
            {
                Console.WriteLine("Извините, не могу выдать сдачу! Операция отменена.");
                return false;
            }

            selectedProduct.Quantity--;
            currentBalance = 0;

            Console.WriteLine($"Вы купили: {selectedProduct.Name}");

            if (changeAmount > 0)
            {
                GiveChange(changeAmount);
            }
            else
            {
                Console.WriteLine("Сдача не требуется.");
            }

            return true;
        }

        public bool CanGiveChange(decimal amount)
        {
            if (amount == 0) return true;

            var tempCoins = coins.ConvertAll(coin => new Coin(coin.Denomination, coin.Count));
            return TryCalculateChange(amount, tempCoins);
        }

        private bool TryCalculateChange(decimal amount, List<Coin> availableCoins)
        {
            availableCoins.Sort((a, b) => b.Denomination.CompareTo(a.Denomination));

            decimal remaining = amount;

            foreach (var coin in availableCoins)
            {
                if (coin.Count == 0) continue;

                int coinsToUse = Math.Min((int)(remaining / coin.Denomination), coin.Count);

                if (coinsToUse > 0)
                {
                    decimal usedAmount = coinsToUse * coin.Denomination;
                    remaining -= usedAmount;
                    coin.Count -= coinsToUse;
                }

                if (remaining == 0) return true;
            }

            return remaining == 0;
        }
        
        public bool GiveChange(decimal amount)
        {
            if (amount == 0) return true;
            
            Console.WriteLine($"Выдаем сдачу: {amount} руб.");
            
            coins.Sort((a, b) => b.Denomination.CompareTo(a.Denomination));
            
            decimal remaining = amount;
            bool success = true;
            
            foreach (var coin in coins)
            {
                if (coin.Count == 0) continue;
                
                int coinsToUse = Math.Min((int)(remaining / coin.Denomination), coin.Count);
                
                if (coinsToUse > 0)
                {
                    decimal usedAmount = coinsToUse * coin.Denomination;
                    remaining -= usedAmount;
                    coin.Count -= coinsToUse;
                    
                    Console.WriteLine($"- {coin.Denomination} руб. × {coinsToUse} = {usedAmount} руб.");
                }
                
                if (remaining == 0) break;
            }
            
            if (remaining > 0)
            {
                Console.WriteLine("Ошибка! Не удалось выдать сдачу.");
                success = false;
            }
            
            return success;
        }

        public void CancelOperation()
        {
            if (currentBalance > 0)
            {
                Console.WriteLine($"Операция отменена. Возврат: {currentBalance} руб.");
                bool refundGiven = GiveChange(currentBalance);
                if (refundGiven)
                {
                    currentBalance = 0;
                }
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