using System;

namespace VendingMachine
{
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
}