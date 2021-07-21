using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_OOP
{
    class Account
    {
        // Account Name: 15-character limit
        private string accountName;
        public string AccountName
        {
            get { return accountName; }
            set
            {
                if (value.Length <= 15)
                    accountName = value;
                else
                {
                    unnamedCount++;
                    accountName = $"Custom {unnamedCount}";
                }
            }
        }
        public int AccountNum { get; set; }
        public decimal Balance { get; set; }

        private static int acctCount = 0;
        private static int unnamedCount = 0;

        public Account()
        {
            acctCount++;
            AccountNum = acctCount;
        }
    }
}
