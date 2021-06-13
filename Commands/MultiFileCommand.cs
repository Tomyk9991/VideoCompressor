using System;

namespace VideoCompressor.Commands
{
    public class MultiFileCommand : Command
    {
        public bool MultiFileMode { get; private set; } = false;
        
        public override bool TryParse(string value)
        {
            string withoutHyphen = value.Substring(1).ToLower();

            if (withoutHyphen is "mf" or "-mf" or "multifile" or "-multifile")
            {
                Console.WriteLine("Mehrfacheditierung ist aktiv. Alle unterstützten Videoformate werden komprimiert");
                Console.WriteLine("Die Ausgaben werden numerisch benannt");
                this.MultiFileMode = true;
            }

            return MultiFileMode;
        }
    }
}