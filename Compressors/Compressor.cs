using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FFmpeg.NET;
using VideoCompressor.Commands;
using VideoCompressor.Extensions;
using VideoCompressor.MultiFileCondition;
using VideoCompressor.Utils;

namespace VideoCompressor.Compressors
{
    public abstract class Compressor
    {
        public abstract Task Run(List<Command> commands);

        protected MediaFile[] FindMp4s(string path, Condition condition = null)
        {
            string[] paths = Directory.EnumerateFiles(path)
                .Where((string file) => file.ToLower().EndsWith(".mp4")).ToArray();

            MediaFile[] inputFiles = new MediaFile[paths.Length];

            for (int i = 0; i < inputFiles.Length; i++)
                inputFiles[i] = new MediaFile(paths[i]);
            
            return inputFiles;
        }

        protected MediaFile[] GenerateOutputFiles(int count)
        {
            MediaFile[] outputFiles = new MediaFile[count];

            for (int i = 0; i < count; i++)
                outputFiles[i] = new MediaFile($"Output {i + 1}.mp4");

            return outputFiles;
        }

        protected async Task<MediaFile> Compress(MediaFile inputFile, MediaFile outputFile, SizeCommand sizeCommand, BitRateHolder bitRates, int row)
        {
            PrintHelper.WriteLineStringBy("Eingelesene Datei:", ' ', inputFile.FileInfo.FullName);
            PrintHelper.WriteLineStringBy("Output Datei:", ' ', outputFile.FileInfo.FullName);

            Engine ffmpeg = new Engine();
            ConversionOptions options = await ffmpeg.BuildConversionOptions(inputFile, sizeCommand, bitRates);

            if (options == null)
                return null;

            
            ffmpeg.BindProgressToConsole(row);
            await ffmpeg.ConvertAsync(inputFile, outputFile, options);


            return outputFile;
        }
    }
}