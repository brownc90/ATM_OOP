/**************************************************************************
 *                                                                        *
 * Each object of this BankATM class is an ATM machine for a single       *
 * shared bank of customers.                                              *
 *                                                                        *
 *************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTables;

namespace ATM_OOP
{
    class BankATM
    {
        // Class Properties
        public int ATM_Num { get; set; }
        public Bank ParentBank { get; set; }

        // Declare local variables
        private static Customer currentCustomer;
        //        private static Customer tryCustomer;
        private static decimal transactionAmt;
        private static bool verified;
        private static string transTypeStr; // Should be enum?
        private const int LOGIN_LIMIT = 3;

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

                        currentCustomer.AcctTransactions = new List<Transaction>();

                        do
                        {
                            ATM_Screen.ShowMenu2();

                            switch (Console.ReadLine())
                            //switch (Utility.ValidateIntInput("What would you like to do? "))
                            {
                                // Option 1. View Balance
                                case "1":
                                //case (int)Menu2Items.ViewBal:
                                    ViewBalance();

                                    break;
                                // Option 2. Make Deposit
                                case "2":
                                    MakeDeposit();

                                    break;
                                // Option 3. Make Withdrawal
                                case "3":
                                    MakeWithdrawal();

                                    break;
                                // Option 4. Transfer Funds
                                case "4":

                                    break;
                                // Option 5. View Transaction History
                                case "5":
                                    ViewTransactions();

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

                Console.Write($"Please enter your card # (Attempt {i}/3): ");
                cardInput = Console.ReadLine();

                #region
                /*
                                cardInput = Utility.ValidateIntInput("Please enter your card #: ");
                                if (cardInput == 0)
                                    continue;
                */
                #endregion

                foreach (Customer c in ParentBank.CustomerList)
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
                    Console.Write($"Please enter your pin (Attempt {j}/3): ");
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

        public void ViewBalance()
        {
            string totalLine, acctLine;

            Console.Clear();

            // TO DO: Format correctly -- with CultureInfo object?
            totalLine = $" Total account balance: {currentCustomer.CalcTotalBal():C2}";
            Console.Write(" ");
            for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                Console.Write("-");
            Console.WriteLine(); 
            Console.Write($"|{totalLine, -ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
            + $"|{"", -ATM_Screen.MENU_BOX_WIDTH_LG}|\n");
            for (int i = 0; i < currentCustomer.CustAccts.Count; i++)
            {
                acctLine = $" {currentCustomer.CustAccts[i].AccountName}: "
                                + $"{currentCustomer.CustAccts[i].Balance:C2}";

                Console.Write($"|{acctLine.PadRight(ATM_Screen.MENU_BOX_WIDTH_LG)}|\n");
            }
            Console.Write($"|{"", -ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                        + " ");
            for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                Console.Write("-");
            ATM_Screen.PrintMessage("", false);
        }

        public void MakeDeposit()
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

                ATM_Screen.ShowAcctsMenu(currentCustomer);

                userChoice = Utility.ValidateIntInput($"Select an account for {transTypeStr.ToLower()}: ");

                if (userChoice <= 0 || userChoice > currentCustomer.CustAccts.Count)
                {
                    ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);
                    continue;
                }

                acctIndex = userChoice - 1;
                acctValid = true;

            } while (!acctValid);

            // TO DO: Account balance cannot surpass 999,999
            transactionAmt = Utility.ValidateDecInput($"Account for {transTypeStr.ToLower()}: "
                                                    + $"{currentCustomer.CustAccts[acctIndex].AccountName}\n"
                                                    + $"Please enter amount for {transTypeStr.ToLower()}: ");

            reviewLine1 = $" {"Account balance before",-23}|{currentCustomer.CustAccts[acctIndex].Balance,13:N2} ";
            reviewLine2 = $" {"",-23}| + {transactionAmt,10:N2} ";
            reviewLine3 = $" {"Account balance after",-23}|{currentCustomer.CustAccts[acctIndex].Balance + transactionAmt,13:N2} ";

            do
            {
                confirmed = false;

                Console.Clear();

                Console.Write($"Account for {transTypeStr.ToLower()}: "
                                + $"{currentCustomer.CustAccts[acctIndex].AccountName}\n"
                                + " ");

                for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                    Console.Write("-");
                Console.WriteLine();
                Console.Write($"|{" Transaction Review", -ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            + $"|{"", -ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            //+ $"|{reviewLine1}|\n"
                            + $"| {"Account balance before",-23}|{currentCustomer.CustAccts[acctIndex].Balance,13:N2} |\n"
                            //+ $"|{reviewLine2}|\n"
                            + $"| {"",-23}| + {transactionAmt,10:N2} |\n"
                            + $"|{"| ____________ ", ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            //+ $"|{reviewLine3}|\n"
                            + $"| {"Account balance after",-23}|{currentCustomer.CustAccts[acctIndex].Balance + transactionAmt,13:N2} |\n"
                            + $"|{"", -ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            + $"|{" 1. Confirm", -ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            + $"|{" 2. Cancel", -ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            + $"|{"", -ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
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

            currentCustomer.CustAccts[acctIndex].Balance += transactionAmt;

            ATM_Screen.PrintMessage($"{transTypeStr} successful! Thank you.", false);

            // Add as a new Transaction object to this Customer's transaction list
            var transaction = new Transaction() { TransDate = DateTime.Now,
                                                    TransType = TransactionType.Deposit,
                                                    TransAmount = transactionAmt,
                                                    SourceAcct = currentCustomer.CustAccts[acctIndex].AccountNum,
                                                    TargetAcct = currentCustomer.CustAccts[acctIndex].AccountNum
            };

            currentCustomer.AcctTransactions.Add(transaction);
        }

        public void MakeWithdrawal()
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

                ATM_Screen.ShowAcctsMenu(currentCustomer);

                userChoice = Utility.ValidateIntInput($"Select an account for {transTypeStr.ToLower()}: ");

                if (userChoice <= 0 || userChoice > currentCustomer.CustAccts.Count)
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

                transactionAmt = Utility.ValidateDecInput($"Account for {transTypeStr.ToLower()}: "
                                                    + $"{currentCustomer.CustAccts[acctIndex].AccountName}\n"
                                                    + $"Please enter amount for {transTypeStr.ToLower()}: ");

                // TO DO: amt cannot bring balance below 0
                if (transactionAmt > currentCustomer.CustAccts[acctIndex].Balance)
                {
                    ATM_Screen.PrintMessage("Insufficient funds.", true);
                }

                amtValid = true;

            } while (!amtValid);

            //reviewLine1 = $" Account balance before |   {currentCustomer.CustAccts[acctIndex].Balance:N2}";
            //reviewLine2 = $"                        | - {transactionAmt:N2}";
            //reviewLine3 = $" Account balance after  |   {currentCustomer.CustAccts[acctIndex].Balance - transactionAmt:N2}";
            reviewLine1 = $" {"Account balance before",-23}|{currentCustomer.CustAccts[acctIndex].Balance,13:N2} ";
            reviewLine2 = $" {"",-23}| - {transactionAmt,10:N2} ";
            reviewLine3 = $" {"Account balance after",-23}|{currentCustomer.CustAccts[acctIndex].Balance - transactionAmt,13:N2} ";

            do
            {
                confirmed = false;

                Console.Clear();

                Console.Write($"Account for {transTypeStr.ToLower()}: "
                                + $"{currentCustomer.CustAccts[acctIndex].AccountName}\n"
                                + " ");

                for (int i = 1; i <= ATM_Screen.MENU_BOX_WIDTH_LG; i++)
                    Console.Write("-");
                Console.WriteLine();
                Console.Write($"|{" Transaction Review",-ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            + $"|{"",-ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            //+ $"|{reviewLine1}|\n"
                            + $"| {"Account balance before",-23}|{currentCustomer.CustAccts[acctIndex].Balance,13:N2} |\n"
                            //+ $"|{reviewLine2}|\n"
                            + $"| {"",-23}| - {transactionAmt,10:N2} |\n"
                            + $"|{"| ____________ ",ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            //+ $"|{reviewLine3}|\n"
                            + $"| {"Account balance after",-23}|{currentCustomer.CustAccts[acctIndex].Balance - transactionAmt,13:N2} |\n"
                            + $"|{"",-ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            + $"|{" 1. Confirm",-ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            + $"|{" 2. Cancel",-ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
                            + $"|{"",-ATM_Screen.MENU_BOX_WIDTH_LG}|\n"
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

            currentCustomer.CustAccts[acctIndex].Balance -= transactionAmt;

            ATM_Screen.PrintMessage($"{transTypeStr} successful! Thank you.", false);

            // Add as a new Transaction object to this Customer's transaction list
            var transaction = new Transaction()
            {
                TransDate = DateTime.Now,
                TransType = TransactionType.Withdrawal,
                TransAmount = transactionAmt,
                SourceAcct = currentCustomer.CustAccts[acctIndex].AccountNum,
                TargetAcct = currentCustomer.CustAccts[acctIndex].AccountNum
            };

            currentCustomer.AcctTransactions.Add(transaction);
        }

        public void ViewTransactions()
        {
            Console.Clear();

            if (currentCustomer.AcctTransactions.Count <= 0)
            {
                ATM_Screen.PrintMessage("No recent transactions to report", false);
                return;
            }

            var ctable = new ConsoleTable("Type", "Account From", "Account To",
                                            "Amount", "Transaction Date");

            foreach (var t in currentCustomer.AcctTransactions)
                ctable.AddRow(t.TransType, $"{t.SourceAcct.ToString("D8")}", $"{t.TargetAcct.ToString("D8")}",
                                $"{t.TransAmount:C2}", t.TransDate);

            // Configure the output table
            ctable.Options.EnableCount = false;
            //ctable.Configure(o => o.NumberAlignment = Alignment.Right);

            // Display the table
            ctable.Write();

            ATM_Screen.PrintMessage("", false);
        }
    }
}
