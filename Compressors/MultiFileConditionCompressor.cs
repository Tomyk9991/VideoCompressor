using VideoCompressor.MultiFileCondition;

namespace VideoCompressor.Compressors
{
    public class MultiFileConditionCompressor : MultiFileCompressor
    {
        public Condition Condition { get; private set; }
        
        public MultiFileConditionCompressor(Condition condition)
        {
            this.Condition = condition;
        }
    }
}