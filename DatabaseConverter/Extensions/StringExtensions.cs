using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.Extensions
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            string[] words = str.Split(new char[] { ' ', '_' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (word.Length > 0)
                {
                    char firstChar = char.ToUpper(word[0]);
                    string restOfString = word.Substring(1).ToLower();
                    words[i] = firstChar + restOfString;
                }
            }

            return string.Join("", words);
        }
    }
}
