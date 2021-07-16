using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoCompressor.Commands;
using VideoCompressor.Compressors;

namespace VideoCompressor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await SizeCommand.LoadFormats();
            List<Command> commands = new List<Command>();

            if (args.Length == 0) return;

            VersionCommand versionCommand = Command.ExtractFrom<VersionCommand>(args);                                      // -v
            HelpCommand helpCommand = Command.ExtractFrom<HelpCommand>(args);                                               // -h
            SizeCommand sizeCommand = Command.ExtractFrom<SizeCommand>(args);                                               // -dc oder -12
            PathCommand pathInput = Command.ExtractFrom<PathCommand>(args);                                                 // 'bla.mp4'
            PathCommand pathOutput = Command.ExtractFrom<PathCommand>(args, args.Length - 1);                               // 'bla.mp4'
            MultiFileCommand multiFileCommand = Command.ExtractFrom<MultiFileCommand>(args);                                // 'mf'
            MultiFileConditionCommand multiFileConditionCommand = Command.ExtractFrom<MultiFileConditionCommand>(args, 1);  // 'condition='

            commands.Add(versionCommand);
            commands.Add(helpCommand);
            commands.Add(sizeCommand);
            commands.Add(pathInput);
            commands.Add(pathOutput);
            commands.Add(pathOutput);
            commands.Add(multiFileConditionCommand);
            
            // Abort Program, if no calculation is needed
            if (commands.Any(command => command is ICanAbortProgram {NeedAbort: true})) return;
            

            Compressor compressor = multiFileCommand.MultiFileMode switch
            {
                true when multiFileConditionCommand.Condition == null => new MultiFileCompressor(),
                true when multiFileConditionCommand.Condition != null => new MultiFileConditionCompressor(
                    multiFileConditionCommand.Condition
                    ),
                false => new SingleFileCompressor(),
                _ => null
            };

            await compressor?.Run(commands);
        }
    }
}