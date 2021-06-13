using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFmpeg.NET;
using VideoCompressor.Commands;
using VideoCompressor.Extensions;
using VideoCompressor.Utils;

namespace VideoCompressor.Compressors
{
    public class MultiFileCompressor : Compressor
    {
        public override async Task Run(List<Command> commands)
        {
            BitRateHolder bitRates = BitRateHolder.Instance;
            // [0] VersionCommand versionCommand = Command.ExtractFrom<VersionCommand>(args);                                      // -v
            // [1] HelpCommand helpCommand = Command.ExtractFrom<HelpCommand>(args);                                               // -h
            // [2] SizeCommand sizeCommand = Command.ExtractFrom<SizeCommand>(args);                                               // -dc oder -12
            // [3] PathCommand pathInput = Command.ExtractFrom<PathCommand>(args);                                                 // 'bla.mp4'
            // [4] PathCommand pathOutput = Command.ExtractFrom<PathCommand>(args, args.Length - 1);                               // 'bla.mp4'
            // [5] MultiFileCommand multiFileCommand = Command.ExtractFrom<MultiFileCommand>(args);                                // 'mf'
            // [6] MultiFileConditionCommand multiFileConditionCommand = Command.ExtractFrom<MultiFileConditionCommand>(args, 1);  // 'condition='
            
            
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
                    : PrintHelper.SplitStringBy("Standardbitrate von", ' ', $"{bitRates.dc / 100.0f:F} kbit/s"));


            MediaFile inputFile = new MediaFile(pathInput.Path);
            MediaFile outputFile = new MediaFile(pathOutput.Path);
            
            if (!inputFile.FileInfo.Exists)
            {
                Console.WriteLine("Die angegebene Datei existiert nicht.");
                return;
            }
            
            PrintHelper.WriteLineStringBy("Eingelesene Datei:", ' ', inputFile.FileInfo.FullName);
            PrintHelper.WriteLineStringBy("Output Datei:", ' ', outputFile.FileInfo.FullName);

            Engine ffmpeg = new Engine();
            ConversionOptions options = await ffmpeg.BuildConversionOptions(inputFile, sizeCommand, bitRates);

            if (options == null)
                return;
            
            ffmpeg.BindProgressToConsole();
            
            
            await ffmpeg.ConvertAsync(inputFile, outputFile, options);

            Console.WriteLine();
            Console.WriteLine();
            
            PrintHelper.WriteLineStringBy("Die Größe des neues Videos beträgt:", ' ', (outputFile.FileInfo.Length / (double) 0x10_0000).ToString("F") + "MB");
        }
    }
}