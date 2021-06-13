using System;
using System.Text;
using System.Threading.Tasks;
using FFmpeg.NET;
using VideoCompressor.Commands;
using VideoCompressor.Utils;

namespace VideoCompressor.Extensions
{
    public static class ffmpegClassExtension
    {
        public static async Task<ConversionOptions> BuildConversionOptions(this Engine ffmpeg, MediaFile inputFile, SizeCommand sizeCommand, BitRateHolder bitRates)
        {
            MetaData metaData = await ffmpeg.GetMetaDataAsync(inputFile);
            double inputVideoLengthInSeconds = metaData.Duration.TotalSeconds;
            
            
            int bitrateForTargetSize = sizeCommand.CalculateBitrateWithFixedTargetSize(inputVideoLengthInSeconds);

            if (!sizeCommand.HasTargetSize && inputFile.FileInfo.Length / (double) 0x10_0000 < sizeCommand.FixedTargetSize)
            {
                Console.WriteLine($"Die Datei ist bereits kleiner als die {sizeCommand.FixedTargetSize}MB");
                return null;
            }

            ConversionOptions options = new ConversionOptions
            {
                VideoBitRate = sizeCommand.HasTargetSize ? bitrateForTargetSize  : sizeCommand.BitRateFromFormatting(bitRates),
                VideoFps = (int) metaData.VideoData.Fps
            };

            Console.WriteLine();
            return options;
        }
        
        
        public static void BindProgressToConsole(this Engine ffmpeg, int consoleRow)
        {
            ffmpeg.Progress += (_, eventArgs) =>
            {
                float percentage = (float) eventArgs.ProcessedDuration.Seconds / eventArgs.TotalDuration.Seconds;

                if (percentage is >= 0.0f and <= 1.0f)
                {
                    ClearCurrentConsoleLine(consoleRow);
                    SetColorFromPercent(percentage);

                    int totalLength = 32;

                    StringBuilder builder = new StringBuilder("|");
                    
                    int amountHashes = (int) (percentage / (1f / totalLength));
                    int d = totalLength - amountHashes;

                    for (int i = 0; i < amountHashes; i++)
                        builder.Append("■");
                    
                    for (int i = 0; i < d; i++)
                        builder.Append("-");
                    
                    builder.Append("|");
                    PrintHelper.WriteStringBy(builder.ToString(), ' ', "");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write((percentage * 100.0f).ToString("F") + "%");
                }
            };
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

        private static void ClearCurrentConsoleLine(int consoleRow)
        {
            //int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, consoleRow);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, consoleRow);
        }
    }
}