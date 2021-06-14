using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FFmpeg.NET;
using VideoCompressor.Commands;
using VideoCompressor.Utils;

namespace VideoCompressor.Compressors
{
    public class SingleFileCompressor : Compressor
    {
        public override async Task Run(List<Command> commands)
        {
            BitRateHolder bitRates = BitRateHolder.Instance;
            
            SizeCommand sizeCommand = (SizeCommand) commands[2];
            PathCommand pathInput = (PathCommand) commands[3];
            PathCommand pathOutput = (PathCommand) commands[4];

            if (string.IsNullOrEmpty(pathOutput.Path)) pathOutput.Path = "Output.mp4";
            if (pathInput.Path == pathOutput.Path) pathOutput.Path = "Output.mp4";

            PrintHelper.DrawDivisionFullWidth();

            if (string.IsNullOrEmpty(pathInput.Path))
            {
                Console.WriteLine("Keine Eingabedatei angegeben.");
                return;
            }


            Console.WriteLine(
                sizeCommand.HasTargetSize
                    ? sizeCommand.ToIntendedSizePrintingString()
                    : PrintHelper.SplitStringBy("Standardbitrate von", ' ', $"{bitRates.dc / 100.0f:F} kbit/s")
            );


            MediaFile inputFile = new MediaFile(pathInput.Path);
            MediaFile outputFile = new MediaFile(pathOutput.Path);

            if (!inputFile.FileInfo.Exists)
            {
                Console.WriteLine("Die angegebene Datei existiert nicht.");
                return;
            }

            outputFile = await base.Compress(inputFile, outputFile, sizeCommand, bitRates, 1);
            
            Console.WriteLine();
            Console.WriteLine();
            
            PrintHelper.WriteLineStringBy("Die Größe des neues Videos beträgt:", ' ',
                (outputFile.FileInfo.Length / (double) 0x10_0000).ToString("F") + "MB");

        }
    }
}