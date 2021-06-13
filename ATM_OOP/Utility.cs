﻿using System;
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

        // ***MAY BE OBSOLETE***
        public static Int32 ValidateIntInput(string input)
        {
            // Declare local variables
            int value = 0;        // 0 is default because there will never be an option 0 on a menu
            bool valid = false;

            // TO DO: allow for hidden input function to feed into this one
            // pass empty string into prompt input, and ReadLine into user input
//            do
//            {
            Console.Write(input);
            valid = Int32.TryParse(Console.ReadLine(), out value);
            if (!valid)
                ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);
//            } while (!valid);

            return value;
        }

        public static decimal ValidateDecInput(string input)
        {
            decimal value = 0;
            bool valid = false;

            do
            {
                Console.Write(input);
                valid = decimal.TryParse(Console.ReadLine(), out value);
                if (!valid)
                    ATM_Screen.PrintMessage(ATM_Screen.InvalidInputStr, true);
                else if (value < 0)
                {
                    ATM_Screen.PrintMessage("Amount to deposit cannot be negative", true);
                    valid = false;
                }
            } while (!valid);

            return value;
        }
    }
}
