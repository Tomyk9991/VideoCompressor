namespace VideoCompressor.MultiFileCondition
{
    public class EndsWithCondition : Condition
    {
        public string compareString = "";
        public override bool TryParse(string value)
        {
            value = value.ToLower();
            if (value.StartsWith("endswith="))
            {
                this.compareString = value.Substring(9);

                return true;
            }

            return false;
        }
    }
}