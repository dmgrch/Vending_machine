using System;

namespace VendingMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            VendingMachine machine = new VendingMachine();
            MenuManager menu = new MenuManager(machine);
            menu.Run();
        }
    }
}