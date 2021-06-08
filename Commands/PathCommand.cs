namespace VideoCompressor.Commands
{
    public class PathCommand : Command
    {
        public string Path { get; set; } = "";
        public override bool TryParse(string value)
        {
            if (value.EndsWith(".mp4"))
            {
                this.Path = value;
                return true;
            }

            return false;
        }
    }
}