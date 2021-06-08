using System;

namespace ATM_OOP
{
    class Tester
    {
        static void Main(string[] args)
        {
            var thisATM = new ATM1();

            thisATM.Initialize();

            thisATM.Execute();
        }
    }
}
