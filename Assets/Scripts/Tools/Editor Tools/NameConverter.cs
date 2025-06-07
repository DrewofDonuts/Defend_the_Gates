using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Etheral
{
    public static class NameConverter
    {
        
        public static string ConvertToName(string value)
        {
            string name = "";
            for (int i = 0; i < value.Length; i++)
            {
                char currentChar = value[i];
                if (i == 0)
                {
                    name += char.ToUpper(currentChar);
                }
                else if ((i > 0 && char.IsUpper(currentChar)))
                {
                    name += " " + char.ToUpper(currentChar);
                }
                else
                {
                    name += currentChar;
                }
            }

            name.Trim();
            return name;
        }
        
        public static  string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input.Trim();

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

    }
}

