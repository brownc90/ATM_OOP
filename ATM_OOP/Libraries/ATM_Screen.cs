using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace ATM_OOP
{
    // ***MAY BE OBSOLETE***
    // Selection options for main user menu, for use in switch statement    
    public enum Menu2Items
    {
        [Description("View Balance")]
        ViewBal = 1,
        [Description("Deposit")]
        Deposit,
        [Description("Withdraw")]
        Withdraw,
        [Description("Transfer")]
        Transfer,
        [Description("View Transaction History")]
        ViewHistory,
        [Description("Exit")]
        Exit
    }
    // ***MAY BE OBSOLETE***
    public enum TransType
    {
        deposit,
        withdrawal
    }

    static class ATM_Screen
    {
        // Declare global variables
        public static string InvalidInputStr = "That is not a valid input";
        public const int MENU_BOX_WIDTH_SM = 30;
        public const int MENU_BOX_WIDTH_LG = 39;

        public static void ShowWelcome()
        {
            Console.Clear();
            Console.Title = "ATM System";
            Console.WriteLine("Welcome!\n");

            PrintDotAnim();
        }

        public static void ConfirmLogout()
        {
            Console.Clear();
            Console.Write("Logout successful.\n"
                        + "Thank you for choosing this ATM!\n");

            PrintDotAnim();
        }

        public static void ShowBye()
        {
            Console.Clear();
            Console.Write("Thank you for choosing this ATM! Now shutting down");
            PrintDotAnim(3, 100);

            for (int i=0; i < 3; i++)
            {
                PrintDotAnim(speedMS: 100);
                ClearLastLine();
            }
        }

        // Initial menu displayed to user, to log in or exit
        public static void ShowMenu1()
        {
            Console.Clear();
            Console.Write(" ------------------------------\n"
                        + "| Login Menu                   |\n"
                        + "|                              |\n"
                        + "| 1. Insert Debit Card         |\n"
                        + "| 2. Exit                      |\n"
                        + "|                              |\n"
                        + " ------------------------------\n"
                        + "What would you like to do? ");
        }

        // Main menu displayed to user once verified
        public static void ShowMenu2()
        {
            Console.Clear();
            Console.Write(" ------------------------------\n"
                        + "| ATM Main Menu                |\n"
                        + "|                              |\n"
                        + "| 1. View Balance              |\n"
                        + "| 2. Make Deposit              |\n"
                        + "| 3. Make Withdrawal           |\n"
                        + "| 4. Transfer Funds            |\n"
                        + "| 5. View Transaction History  |\n"
                        + "| 6. Log Out                   |\n"
                        + "|                              |\n"
                        + " ------------------------------\n"
                        + "What would you like to do? ");
        }

        // Menu that displays all Customer's accounts after Deposit or Withdrawal selected
        public static void ShowAcctsMenu(Customer cust)
        {
            // Declare local variables
            string acctLine;

            Console.Clear();
            Console.Write(" ------------------------------\n"
                        + "| Your Accounts:               |\n"
                        + "|                              |\n");

            for (int i = 0; i < cust.CustAccts.Count; i++)
            {
                acctLine = " " + (i+1).ToString() + ". " + cust.CustAccts[i].AccountName;

                Console.Write("|" + acctLine.PadRight(MENU_BOX_WIDTH_SM) + "|\n");
            }

            Console.Write("|                              |\n"
                        + " ------------------------------\n");
//                        + "Select an account for {0}: ", transType);
        }

        public static void ConfirmTransaction()
        {

        }

        // Formats and displays warning or error message
        public static void PrintMessage(string msg, bool isError)
        {
            // Declare local variables
            ConsoleKeyInfo keyInfo;

            if (isError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: " + msg);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(msg);
            }

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            // Elect to not display entered character(s)
            keyInfo = Console.ReadKey(true);
        }

        // Prints a loading animation (exclusively visual)
        public static void PrintDotAnim(int numDots = 10, int speedMS = 250)
        {
            for (int x = 0; x < numDots; x++)
            {
                Console.Write(".");
                Thread.Sleep(speedMS);
            }
            Console.WriteLine();
        }

        // Erases a single line in the console, rather than the entire buffer/window
        // Used in the shut-down screen
        public static void ClearLastLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        // To Do: Transfer Form method (in ATMScreen class)
    }
}
