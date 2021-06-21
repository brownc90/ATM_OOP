using System;
using System.Collections.Generic;
using System.Text;

namespace ATM_OOP
{
    class Utility
    {

        public static string ProcessHiddenInput()
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo keyInfo;

            // Loop thru each keystroke and replace w/ '*', if not a control character
            // and until 'Enter' key is pressed
            do
            {
                // Elect to not display entered character(s)
                keyInfo = Console.ReadKey(true);

                if (!char.IsControl(keyInfo.KeyChar))
                {
                    sb.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }

            } while (keyInfo.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return sb.ToString();
        }

        public static Int32 ValidateIntInput(string prompt = "")
        {
            // Declare local variables
            int value = 0;        // 0 is default because there will never be an option 0 on a menu
            bool valid = false;

            do
            {
                Console.Write(prompt);
                valid = Int32.TryParse(Console.ReadLine(), out value);
                if (!valid)
                    ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);
            } while (!valid);

            return value;
        }

        public static decimal ValidateDecInput(string prompt)
        {
            // Declare local variables
            decimal value = 0;
            bool valid = false;

            do
            {
                Console.Clear();

                Console.Write(prompt);
                valid = decimal.TryParse(Console.ReadLine(), out value);
                if (!valid)
                    ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);
                else if (value < 0)
                {
                    ATM_Screen.PrintMessage("Amount cannot be negative", true);
                    valid = false;
                }
            } while (!valid);

            return value;
        }
    }
}
