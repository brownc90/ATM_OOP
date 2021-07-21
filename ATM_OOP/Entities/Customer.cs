using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_OOP
{
    class Customer
    {
        public string FullName { get; set; }
        public int CustNum { get; set; }
        // CardNum is always 5-digit string
        public string CardNum { get; set; }
        // Pin is always 4-digit string
        public string Pin { get; set; }
        public List<Account> CustAccts;
        public List<Transaction> AcctTransactions;

        private static int custCount = 0;

        public Customer()
        {
            custCount++;
            CustNum = custCount;

            //AcctTransactions = new List<Transaction>();
        }

        public decimal CalcTotalBal()
        {
            decimal total = 0m;

            foreach (Account a in CustAccts)
            {
                total += a.Balance;
            }
            return total;
        }
    }
}
