using System;
using System.Collections.Generic;
using System.IO;
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
            PathCommand pathInput = new PathCommand
            {
                Path = Directory.GetCurrentDirectory()
            };
            PrintHelper.DrawDivisionFullWidth();

            Console.WriteLine(
                sizeCommand.HasTargetSize
                    ? sizeCommand.ToIntendedSizePrintingString()
                    : PrintHelper.SplitStringBy("Standardbitrate von", ' ', $"{bitRates.dc / 100.0f:F} kbit/s")
            );

            MediaFile[] inputFiles = FindMp4s(pathInput.Path);
            MediaFile[] outputFiles = GenerateOutputFiles(inputFiles.Length);


            Task<MediaFile>[] runners = new Task<MediaFile>[inputFiles.Length];
            int top = Console.CursorTop + 2 * inputFiles.Length;
            
            int totalValue = 0;
            
            for (int i = 0; i < inputFiles.Length; i++)
            {
                runners[i] = base.Compress(inputFiles[i], outputFiles[i], sizeCommand, bitRates, 3);
            }
            
            Task.WaitAll(runners);

            totalValue = 0;
            
            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i < runners.Length; i++)
            {
                MediaFile outputFile = runners[i].Result;
                PrintHelper.WriteLineStringBy("Die Größe des neues Videos beträgt:", ' ',
                    (outputFile.FileInfo.Length / (double) 0x10_0000).ToString("F") + "MB");
            }
        }
    }
}