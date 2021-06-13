using System;
using System.Reflection;

namespace VideoCompressor.Commands
{
    public class VersionCommand : Command, ICanAbortProgram
    {
        public bool NeedAbort { get; set; }
        
        public override bool TryParse(string value)
        {
            string withoutHyphen = value.Substring(1).ToLower();

            if (withoutHyphen is "v" or "-v" or "version" or "-version")
            {
                NeedAbort = true;
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                Console.WriteLine("Version: " + version);
                return true;
            }

            return false;
        }

    }
}