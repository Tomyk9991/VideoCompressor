namespace VideoCompressor.MultiFileCondition
{
    public class StartsWithCondition : Condition
    {
        public string compareString = "";
        public override bool TryParse(string value)
        {
            value = value.ToLower();
            if (value.StartsWith("startswith="))
            {
                this.compareString = value.Substring(11);

                return true;
            }

            return false;
        }
    }
}