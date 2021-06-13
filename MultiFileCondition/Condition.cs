namespace VideoCompressor.MultiFileCondition
{
    public abstract class Condition
    {
        public abstract bool TryParse(string value);
    }
}