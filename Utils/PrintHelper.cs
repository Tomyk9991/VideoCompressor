using System;
using System.Text;

namespace VideoCompressor.Utils
{
    public class PrintHelper
    {
        public static void DrawDivisionFullWidth()
        {
            for (int i = 0; i < Console.BufferWidth; i++) Console.Write("-");
            Console.WriteLine();
        }
        
        public static void WriteLineStringBy(string str1, char sep, string str2, int startX = 48)
        {
            StringBuilder builder = new StringBuilder(str1);
            
            int sepRepeats = startX - str1.Length;
            if (sepRepeats < 0)
                throw new ArgumentException(str1 + " must me shorter than " + startX + " characters");

            builder.Append(sep, sepRepeats);
            builder.Append(str2);


            Console.WriteLine(builder.ToString());
        }
        
        public static void WriteStringBy(string str1, char sep, string str2, int startX = 48)
        {
            StringBuilder builder = new StringBuilder(str1);
            
            int sepRepeats = startX - str1.Length;
            if (sepRepeats < 0)
                throw new ArgumentException(str1 + " must me shorter than " + startX + " characters");

            builder.Append(sep, sepRepeats);
            builder.Append(str2);


            Console.Write(builder.ToString());
        }
        

        public static string SplitStringBy(string str1, char sep, string str2, int startX = 48)
        {
            StringBuilder builder = new StringBuilder(str1);
            
            int sepRepeats = startX - str1.Length;
            if (sepRepeats < 0)
                throw new ArgumentException(str1 + " must me shorter than " + startX + " characters");

            builder.Append(sep, sepRepeats);
            builder.Append(str2);


            return builder.ToString();
        }
    }
}