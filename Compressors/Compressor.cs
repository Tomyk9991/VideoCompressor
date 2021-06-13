using System.Collections.Generic;
using System.Threading.Tasks;
using VideoCompressor.Commands;

namespace VideoCompressor.Compressors
{
    public abstract class Compressor
    {
        public abstract Task Run(List<Command> commands);
    }
}