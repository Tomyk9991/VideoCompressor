using System;
using System.Linq;
using System.Threading.Tasks;
using VideoCompressor.Commands;
using FFmpeg.NET;

namespace VideoCompressor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            BitRateHolder bitRates = await SizeCommand.LoadFormats();

            Command[] commands = new Command[5]; 
            VersionCommand versionCommand = Command.ExtractFrom<VersionCommand>(args);
            HelpCommand helpCommand = Command.ExtractFrom<HelpCommand>(args);
            SizeCommand sizeCommand = Command.ExtractFrom<SizeCommand>(args);
            PathCommand pathInput = Command.ExtractFrom<PathCommand>(args, 1);
            PathCommand pathOutput = Command.ExtractFrom<PathCommand>(args, 2);

            commands[0] = versionCommand;
            commands[1] = helpCommand;
            commands[2] = sizeCommand;
            commands[3] = pathInput;
            commands[4] = pathOutput;
            
            if (string.IsNullOrEmpty(pathOutput.Path)) pathOutput.Path = "Output.mp4";

            if (commands.Any(command => command is ICanAbortProgram {NeedAbort: true}))
            {
                return;
            }
            
            for (int i = 0; i < Console.BufferWidth; i++) Console.Write("-");
            Console.WriteLine();
            
            if (string.IsNullOrEmpty(pathInput.Path))
            {
                Console.WriteLine("Keine Eingabedatei angegeben.");
                return;
            }
            
            Console.WriteLine(
                sizeCommand.HasTargetSize
                    ? sizeCommand.ToIntendedSizePrintingString()
                    : $"Standardbitrate von\t\t\t\t{bitRates.dc / 100.0f:F} kbit/s");


            MediaFile inputFile = new MediaFile(pathInput.Path);
            MediaFile outputFile = new MediaFile(pathOutput.Path);
            
            if (!inputFile.FileInfo.Exists)
            {
                Console.WriteLine("Die angegebene Datei existiert nicht.");
                return;
            }
            
            Console.WriteLine("Eingelesene Datei:\t\t\t\t" + inputFile.FileInfo.FullName);
            Console.WriteLine("Output Datei:\t\t\t\t\t" + outputFile.FileInfo.FullName);

            Engine ffmpeg = new Engine();
            MetaData metaData = await ffmpeg.GetMetaDataAsync(inputFile);
            double inputVideoLengthInSeconds = metaData.Duration.TotalSeconds;
            
            
            int bitrateForTargetSize = sizeCommand.CalculateBitrateWithFixedTargetSize(inputVideoLengthInSeconds);

            if (!sizeCommand.HasTargetSize && inputFile.FileInfo.Length / (double) 0x10_0000 < sizeCommand.FixedTargetSize)
            {
                Console.WriteLine($"Die Datei ist bereits kleiner als die {sizeCommand.FixedTargetSize}MB");
                return;
            }

            ConversionOptions options = new ConversionOptions
            {
                VideoBitRate = sizeCommand.HasTargetSize ? bitrateForTargetSize  : sizeCommand.BitRateFromFormatting(bitRates),
                VideoFps = (int) metaData.VideoData.Fps
            };

            Console.WriteLine();
            ffmpeg.Progress += (_, eventArgs) =>
            {
                float percentage = (float) eventArgs.ProcessedDuration.Seconds / eventArgs.TotalDuration.Seconds;
        
                if (percentage is >= 0.0f and <= 1.0f)
                {
                    ClearCurrentConsoleLine();
                    SetColorFromPercent(percentage);
                    int totalLength = 32;
                    Console.Write("|");
                    int amountHashes = (int) (percentage / (1f / totalLength));
                    int d = totalLength - amountHashes;
                    
                    for (int i = 0; i < amountHashes; i++)
                        Console.Write("■");
                    
                    for (int i = 0; i < d; i++)
                        Console.Write("-");
                    
                    Console.Write("|");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("\t\t" + (percentage * 100.0f).ToString("F") + "%");
                }
            };
            
            
            await ffmpeg.ConvertAsync(inputFile, outputFile, options);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Die Größe des neues Videos beträgt:\t\t" + (outputFile.FileInfo.Length / (double) 0x10_0000).ToString("F") + "MB");
        }

        private static void SetColorFromPercent(float percentage)
        {
            ConsoleColor[] colors = {ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green};

            for (int i = 0; i < colors.Length; i++)
            {
                float lowerLimit = i / (float) colors.Length;
                float upperLimit = (i + 1) / (float) colors.Length;
                
                if (percentage >= lowerLimit && percentage < upperLimit)
                {
                    Console.ForegroundColor = colors[i];
                    break;
                }
            }
        }

        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}