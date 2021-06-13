using VideoCompressor.MultiFileCondition;

namespace VideoCompressor.Commands
{
    public class MultiFileConditionCommand : Command
    {
        public Condition Condition { get; private set; }
        
        public override bool TryParse(string value)
        {
            Condition[] conditions =
            {
                new StartsWithCondition(),
                new EndsWithCondition()
            };

            foreach (Condition condition in conditions)
            {
                if (condition.TryParse(value))
                {
                    this.Condition = condition;
                    return true;
                }
            }

            return false;
        }
    }
}