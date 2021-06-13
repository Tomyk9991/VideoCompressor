namespace VideoCompressor.Commands
{
    public abstract class Command
    {
        /// <summary>
        /// Extracts the command for a string. If a string can be parsed to the command, it will be returned
        /// </summary>
        /// <param name="arguments">strings to check</param>
        /// <param name="index">Index, where to start in arguments-array</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ExtractFrom<T>(string[] arguments, int index = 0) where T : Command, new()
        {
            T value = new T();
            
            for (int i = index; i < arguments.Length; i++)
            {
                // So not every argument will be parsed.
                if (value.TryParse(arguments[i]))
                {
                    return value;
                }
            }

            return value;
        }

        // Does nothing to the members, if parse isn't valid
        public abstract bool TryParse(string value);
    }
}