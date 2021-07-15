using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_OOP
{
    class Bank
    {
        public List<Customer> CustomerList { get; set; }
        public List<BankATM> ATM_List { get; set; }

        public void Initialize()
        {
            // Build customer base
            // TO DO: integrate with a database of customers
            CustomerList = new List<Customer>
            {
                // Customer object allowing for quick testing by coder
                new Customer { FullName = "ADMIN",
                                CardNum = "0", Pin = "0",
                                CustAccts = new List<Account>
                                {
                                    new Account() { AccountName = "Checking", Balance = 1000m },
                                    new Account() { AccountName = "Savings", Balance = 1000m },
                                    new Account() { AccountName = "Custom1", Balance = 1000m }
                                }
                },
                new Customer { FullName = "Olivia Brown",
                                CardNum = "12345", Pin = "4321",
                                CustAccts = new List<Account>
                                {
                                    new Account() { AccountName = "Checking", Balance = 0m },
                                    new Account() { AccountName = "Savings", Balance = 150m },
                                    new Account() { AccountName = "Custom1", Balance = 1157.27m }
                                }
                },
                new Customer { FullName = "Liberty Bibberty",
                                CardNum = "55555", Pin = "5555",
                                CustAccts = new List<Account>
                                {
                                    new Account() { AccountName = "Checking", Balance = 0m },
                                    new Account() { AccountName = "Savings", Balance = 4500m },
                                    new Account() { AccountName = "Custom1", Balance = 50.95m },
                                    new Account() { AccountName = "Custom2", Balance = 50m },
                                    new Account() { AccountName = "Emergency Fund", Balance = 1000m }
                                }
                }
            };

            ATM_List = new List<BankATM>();
        }

        public void AddATM()
        {
            ATM_List.Add(new BankATM { ATM_Num = ATM_List.Count + 1, ParentBank = this} );
        }
    }
}
