using VideoCompressor.Commands;

namespace VideoCompressor.Utils
{
    public class BitUtils
    {
        public static int BitRateFromFormatting(BitRateHolder bitRates, string formatting)
        {
            var dictionary = bitRates.ToDictionary<int>();
            dictionary[""] = dictionary["dc"]; //Standardfall, falls keine size angegeben wurde
            return dictionary[formatting];
        }
    }
}