/**************************************************************************
 *                                                                        *
 * Each object of this BankATM class is an ATM machine for a single       *
 * shared bank of customers.                                              *
 *                                                                        *
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_OOP
{
    class BankATM
    {
        public int ATM_Num { get; set; }
        private Bank parentBank;

        // Declare local variables
        private static Customer currentCustomer;
        //        private static Customer tryCustomer;
        private static decimal transactionAmt;
        private static bool verified;
        private static string transTypeStr; // Should be enum?
        private const int LOGIN_LIMIT = 3;

        // Constructor(s)
        public BankATM(Bank parentBank)
        {
            ATM_Num = parentBank.ATM_List.Count + 1;
            this.parentBank = parentBank;
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
                        verified = false;

                        CheckCredentials();

                        if (!verified)
                            continue;

                        do
                        {
                            ATM_Screen.ShowMenu2();

                            switch (Console.ReadLine())
                            //switch (Utility.ValidateIntInput("What would you like to do? "))
                            {
                                // Option 1. View Balance
                                case "1":
                                //case (int)Menu2Items.ViewBal:
                                    ViewBalance(currentCustomer);

                                    break;
                                // Option 2. Make Deposit
                                case "2":
                                    MakeDeposit(currentCustomer);

                                    break;
                                // Option 3. Make Withdrawal
                                case "3":
                                    MakeWithdrawal(currentCustomer);

                                    break;
                                // Option 4. Transfer Funds
                                case "4":

                                    break;
                                // Option 5. View Transaction History
                                case "5":

                                    break;
                                // Option 6. Log Out
                                case "6":
                                    ATM_Screen.ConfirmLogout();

                                    Execute();

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
            Customer tryCustomer;
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

                foreach (Customer c in parentBank.CustomerList)
                {
                    if (c.CardNum.Equals(cardInput))
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
                        return;
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

                            return;
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
                verified = true;
                currentCustomer = tryCustomer;
                break;
            }
        }

        public void ViewBalance(Customer cust)
        {
            string totalLine, acctLine;

            Console.Clear();

            // TO DO: Format correctly -- with CultureInfo object?
            totalLine = String.Format("Total account balance: {0:C2}", cust.CalcTotalBal());

            Console.Write(" ");
            for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                Console.Write("-");
            Console.WriteLine();
            Console.Write("|" + totalLine.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                        + "|" + "".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n");

           for (int i = 0; i < cust.CustAccts.Count; i++)
            {
                acctLine = " " + cust.CustAccts[i].AccountName + ": "
                                + String.Format("{0:C2}", cust.CustAccts[i].Balance);

                Console.Write("|" + acctLine.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n");
            }

            Console.Write("|" + "".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                        + " ");
            for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                Console.Write("-");
            Console.WriteLine();
            Console.ReadKey();
        }

        public void MakeDeposit(Customer cust)
        {
            // Declare local variables
            int userChoice;
            int acctIndex;
            bool acctValid, confirmed;
            string reviewLine1, reviewLine2, reviewLine3;

            transTypeStr = "Deposit";

            do
            {
                acctValid = false;
                acctIndex = -1;
                transactionAmt = 0m;

                ATM_Screen.ShowAcctsMenu(cust);

                userChoice = Utility.ValidateIntInput("Select an account for "
                                                        + transTypeStr.ToLower() + ": ");

                if (userChoice <= 0 || userChoice > cust.CustAccts.Count)
                {
                    ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);
                    continue;
                }

                acctIndex = userChoice - 1;
                acctValid = true;

            } while (!acctValid);

            transactionAmt = Utility.ValidateDecInput(String.Format("Account for {0}: {1}\n"
                                                    + "Please enter amount for {2}: ",
                                                    transTypeStr.ToLower(),
                                                    cust.CustAccts[acctIndex].AccountName,
                                                    transTypeStr.ToLower()));

            reviewLine1 = String.Format(" Account balance before |   {0:N2}",
                                            cust.CustAccts[acctIndex].Balance);
            reviewLine2 = String.Format("                        | + {0:N2}",
                                            transactionAmt);
            reviewLine3 = String.Format(" Account balance after  |   {0:N2}",
                                            cust.CustAccts[acctIndex].Balance+transactionAmt);

            do
            {
                confirmed = false;

                Console.Clear();

                Console.Write("Account for {0}: {1}\n"
                                + " ",
                                transTypeStr.ToLower(),
                                cust.CustAccts[acctIndex].AccountName);

                for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                    Console.Write("-");
                Console.WriteLine();
                Console.Write("|" + " Transaction Review".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)  + "|\n"
                            + "|" + " ".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)                    + "|\n"
                            + "|" + reviewLine1.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)            + "|\n"
                            + "|" + reviewLine2.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)            + "|\n"
                            + "|                        | ____________ |\n"
                            + "|" + reviewLine3.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)            + "|\n"
                            + "|" + "".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)                     + "|\n"
                            + "|" + " 1. Confirm".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)          + "|\n"
                            + "|" + " 2. Cancel".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)           + "|\n"
                            + "|" + " ".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)                    + "|\n"
                            + " ");
                for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                    Console.Write("-");
                Console.WriteLine();

                // Visual-friendly, less customizable version of above code
                #region
                /*
                Console.Write("Account for {0}: {1}\n"
                            + " ---------------------------------------\n"
                            + "| Transaction Review                    |\n"
                            + "|                                       |\n",
                                transTypeStr.ToLower(),
                                cust.CustAccts[acctIndex].AccountName);
                Console.Write("|" + reviewLine1.PadRight(MENU_BOX_WIDTH) + "|\n");
                Console.Write("|" + reviewLine2.PadRight(MENU_BOX_WIDTH) + "|\n");
                Console.Write("|                          ____________ |\n");
                Console.Write("|" + reviewLine3.PadRight(MENU_BOX_WIDTH) + "|\n");
                Console.Write("|                                       |\n"
                            + "| 1. Confirm                            |\n"
                            + "| 2. Cancel                             |\n"
                            + "|                                       |\n"
                            + " ---------------------------------------\n");
                */
                #endregion

                switch (Console.ReadLine())
                {
                    // Option 1: Confirm
                    case "1":
                        confirmed = true;

                        break;
                    // Option 2: Cancel
                    case "2":
                        ATM_Screen.PrintMessage("Transaction canceled.", false);

                        return;
                    default:
                        ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);

                        break;
                }

            } while (!confirmed);

            cust.CustAccts[acctIndex].Balance += transactionAmt;

            ATM_Screen.PrintMessage(transTypeStr + " successful! Thank you.", false);

            var transaction = new Transaction() { TransID = 1, TransDate = DateTime.Now,
                                                    TransAmount = transactionAmt
            };
        }

        public void MakeWithdrawal(Customer cust)
        {
            // Declare local variables
            int userChoice;
            int acctIndex;
            bool acctValid, amtValid, confirmed;
            string reviewLine1, reviewLine2, reviewLine3;

            transTypeStr = "Withdrawal";

            do
            {
                acctValid = false;
                acctIndex = -1;
                transactionAmt = 0m;

                ATM_Screen.ShowAcctsMenu(cust);

                userChoice = Utility.ValidateIntInput("Select an account for "
                                                        + transTypeStr.ToLower() + ": ");

                if (userChoice <= 0 || userChoice > cust.CustAccts.Count)
                {
                    ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);
                    continue;
                }

                acctIndex = userChoice - 1;
                acctValid = true;

            } while (!acctValid);

            do
            {
                amtValid = false;

                transactionAmt = Utility.ValidateDecInput(String.Format("Account for {0}: {1}\n"
                                                        + "Please enter amount for {2}: ",
                                                        transTypeStr.ToLower(),
                                                        cust.CustAccts[acctIndex].AccountName,
                                                        transTypeStr.ToLower()));

                // TO DO: amt cannot bring balance below 0
                if (transactionAmt > cust.CustAccts[acctIndex].Balance)
                {
                    ATM_Screen.PrintMessage("Insufficient funds.", true);
                }

                amtValid = true;

            } while (!amtValid);

            reviewLine1 = String.Format(" Account balance before : {0:N2}",
                                            cust.CustAccts[acctIndex].Balance);
            reviewLine2 = String.Format("                          - {0:N2}", transactionAmt);
            reviewLine3 = String.Format(" Account balance after  :   {0:N2}",
                                            cust.CustAccts[acctIndex].Balance - transactionAmt);

            do
            {
                confirmed = false;

                Console.Clear();

                Console.Write("Account for {0}: {1}\n"
                + " ",
                transTypeStr.ToLower(),
                cust.CustAccts[acctIndex].AccountName);

                for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                    Console.Write("-");
                Console.WriteLine();
                Console.Write("|" + " Transaction Review".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + "|" + " ".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + "|" + reviewLine1.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + "|" + reviewLine2.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + "|                          ____________ |\n"
                            + "|" + reviewLine3.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + "|" + "".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + "|" + " 1. Confirm".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + "|" + " 2. Cancel".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + "|" + " ".PadRight(ATM_Screen.MENU_BOX_WIDTH_LG) + "|\n"
                            + " ");
                for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                    Console.Write("-");
                Console.WriteLine();

                // Visual-friendly, less customizable version of above code
                #region
                /*
                Console.Write("Account for {0}: {1}\n"
                            + " ---------------------------------------\n"
                            + "| Transaction Review                    |\n"
                            + "|                                       |\n",
                                transTypeStr.ToLower(),
                                cust.CustAccts[acctIndex].AccountName);
                Console.Write("|" + reviewLine1.PadRight(MENU_BOX_WIDTH) + "|\n");
                Console.Write("|" + reviewLine2.PadRight(MENU_BOX_WIDTH) + "|\n");
                Console.Write("|                          ____________ |\n");
                Console.Write("|" + reviewLine3.PadRight(MENU_BOX_WIDTH) + "|\n");
                Console.Write("|                                       |\n"
                            + "| 1. Confirm                            |\n"
                            + "| 2. Cancel                             |\n"
                            + "|                                       |\n"
                            + " ---------------------------------------\n");
                */
                #endregion

                switch (Console.ReadLine())
                {
                    // Option 1: Confirm
                    case "1":
                        confirmed = true;

                        break;
                    // Option 2: Cancel
                    case "2":
                        ATM_Screen.PrintMessage("Transaction canceled.", false);

                        return;
                    default:
                        ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);

                        break;
                }

            } while (!confirmed);

            cust.CustAccts[acctIndex].Balance -= transactionAmt;

            ATM_Screen.PrintMessage(transTypeStr + " successful! Thank you.", false);

            var transaction = new Transaction();
        }
    }
}
