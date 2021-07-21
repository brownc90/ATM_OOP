using System;

namespace ATM_OOP
{
    class Tester
    {
        static void Main(string[] args)
        {
            var thisBank = new Bank();

            thisBank.Initialize();

            thisBank.AddATM();

            thisBank.ATM_List[0].Execute();

        }
    }
}
