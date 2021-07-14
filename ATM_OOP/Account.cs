using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_OOP
{
    class Account
    {
        public string AccountName { get; set; }
        public int AccountNum { get; set; }
        public decimal Balance { get; set; }

        private static int acctCount = 0;

        public Account()
        {
            acctCount++;
            AccountNum = acctCount;
        }
    }
}
