namespace VideoCompressor.Commands
{
    public interface ICanAbortProgram
    {
        bool NeedAbort { get; set; }
    }
}