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
        public bool IsActive { get; set; }
        public List<Account> CustAccts;

        // TO DO: Add constructor class to initialize new customers w single account

        public static decimal CalcTotalBal(Customer cust)
        {
            decimal total = 0m;

            foreach (Account a in cust.CustAccts)
            {
                total += a.Balance;
            }
            return total;
        }
    }
}
