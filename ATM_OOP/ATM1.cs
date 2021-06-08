using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_OOP
{
    class ATM1
    {
        // Declare local variables
        private static List<Customer> customerList;
        private static Customer currentCustomer;
        private static Customer tryCustomer;
        private const int LOGIN_LIMIT = 3;
        
        // Build customer base
        public void Initialize()
        {
            // TO DO: integrate with a database of customers
            customerList = new List<Customer>
            {
                new Customer() { FullName = "Olivia Brown", CustNo = 1,
                                CardNo = "12345", Pin = "4321", IsActive = true,
                                CustAccts = new List<Account>
                                {
                                    new Account() { Balance = 0m },
                                    new Account() { Balance = 150m },
                                    new Account() { Balance = 1157.27m }
                                }
                }
            };
        }

        // Starting point of ATM execution, from customer's perspective
        // Called from Main method of Tester class
        public void Execute()
        {
            ATM_Screen.ShowWelcome();

            do
            {
                ATM_Screen.ShowMenu1();

                switch (Console.ReadLine())
//                switch (Utility.ValidateIntInput("What would you like to do? "))
                {
                    // Option 1. Insert Debit Card
                    case "1":
                        CheckCredentials();

                        do
                        {
                            ATM_Screen.ShowMenu2();

                            switch (Console.ReadLine())
//                            switch (Utility.ValidateIntInput("What would you like to do? "))
                            {
                                // Option 1. View Balance
                                case "1":
//                                case (int)Menu2Items.ViewBal:
                                    ViewBalance(currentCustomer);

                                    break;
                                // Option 2. Make Deposit
                                case "2":
                                    Deposit();

                                    break;
                                // Option 3. Make Withdrawal
                                case "3":

                                    break;
                                // Option 4. Transfer Funds
                                case "4":

                                    break;
                                // Option 5. View Transaction History
                                case "5":

                                    break;
                                // Option 6. Log Out
                                case "6":

                                    break;
                                default:
                                    ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);

                                    break;
                            }

                        } while (true);

                    // Option 2. Exit
                    case "2":
                        ATM_Screen.ShowBye();

                        Environment.Exit(0);

                        break;
                    default:
                        ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);

                        break;
                }

            } while (true);
        }

        public void CheckCredentials()
        {
            // Declare local variables
            string cardInput, pinInput;
            // TO DO: What is difference between below bools, and are they necessary?
            bool cardRecognized;
            //            bool pinValid;
            int i, j;

            Console.Clear();

            // Customer verification loop
            // Outer loop: Card #; inner loop: Pin
            for (i = 1; i <= LOGIN_LIMIT; i++)
            {
                // Next line may be redundant...
                cardRecognized = false;
                //                pinValid = false;
                tryCustomer = new Customer();

                Console.Write("Please enter your card # (Attempt {0}/3): ", i);
                cardInput = Console.ReadLine();

                #region
                /*
                                cardInput = Utility.ValidateIntInput("Please enter your card #: ");
                                if (cardInput == 0)
                                    continue;
                */
                #endregion

                foreach (Customer c in customerList)
                {
                    if (c.CardNo.Equals(cardInput))
                    {
                        tryCustomer = c;
                        cardRecognized = true;
                        break;
                    }
                }

                if (!cardRecognized)
                {
                    ATM_Screen.PrintMessage("Card # not recognized", true);

                    if (i == LOGIN_LIMIT)
                    {
                        ATM_Screen.PrintMessage("Login attempt failed too many times.\n"
                                              + "Please contact Customer Support, "
                                              + "or try again later.", false);
                        Execute();
                    }

                    // Reach this point if Card # incorrect and attempt limit not reached
                    // Loop to next attempt
                    continue;
                }

                // Reach this point if Card # correct
                // Pin verification loop
                for (j = 1; j <= LOGIN_LIMIT; j++)
                {
                    Console.Write("Please enter your pin (Attempt {0}/3): ", j);
                    pinInput = Utility.ProcessHiddenInput();

                    #region
                    /*
                                        pinValid = Int32.TryParse(Utility.ProcessHiddenInput(), out pinInput);
                                        if (!pinValid)
                                        {
                                            ATM_Screen.PrintMessage("That is not a valid input", true);
                                            continue;
                                        }
                    */
                    #endregion

                    if (!(tryCustomer.Pin.Equals(pinInput)))
                    {
                        ATM_Screen.PrintMessage("Incorrect pin", true);

                        if (j == LOGIN_LIMIT)
                        {
                            ATM_Screen.PrintMessage("Login attempt failed too many times.\n"
                                                   + "Please contact Customer Support, "
                                                   + "or try again later.", false);

                            Execute();
                        }

                        // Reach this point if Pin incorrect and attempt limit not reached
                        // Loop to next attempt
                        continue;
                    }

                    // Reach this point if Pin correct
                    // Exit the inner pin verification loop
                    break;
                }

                // Reach this point if credentials are verified
                // Approve customer and exit the verification loop
                currentCustomer = tryCustomer;
                break;
            }
        }

        public void ViewBalance(Customer cust)
        {
            // TO DO: Format correctly -- with CultureInfo object?
            ATM_Screen.PrintMessage(String.Format("Total account balance: {0:C2}",
                                                    Customer.CalcTotalBal(cust)), false);
        }

        public void MakeDeposit()
        {

        }
    }
}
