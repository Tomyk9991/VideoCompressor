using System;
using System.Collections.Generic;

namespace VideoCompressor.Commands
{
    public class HelpCommand : Command, ICanAbortProgram
    {
        public bool NeedAbort { get; set; }
        
        public override bool TryParse(string value)
        {
            string withoutHyphen = value.Substring(1).ToLower();

            if (withoutHyphen is "h" or "-h" or "help" or "-help")
            {
                NeedAbort = true;
                Console.WriteLine();
                Console.WriteLine("-[Video compressor format] [inputPath.mp4] Optional:[outputPath.mp4]");
                Console.WriteLine("Video compressor Formate haben vordefinierte Bitraten:");

                Console.WriteLine();
                foreach (KeyValuePair<string,int> valuePair in BitRateHolder.Instance.ToDictionary<int>())
                {
                    Console.WriteLine(" - \t" + valuePair.Key + "\t\t" + valuePair.Value);
                }
                
                Console.WriteLine();

                Console.WriteLine(
                    "Alternativ kann auch eine Zielgröße der Ausgabedatei angegeben werden, statt ein Compressor Format");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Beispiele:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t" + "-dc 'Funny Moment.mp4' 'Output.mp4'");
                Console.WriteLine("\t" + "-12 'FHD Render.mp4'");
                
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine("");
                
                return true;
            }

            return false;
        }

    }
}