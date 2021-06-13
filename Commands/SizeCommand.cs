using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoCompressor.Utils;

namespace VideoCompressor.Commands
{
    public class SizeCommand : Command
    {
        public int FixedTargetSize { get; private set; }
        public bool HasTargetSize { get; private set; }

        private string _specialFormatting = "";
        private static readonly string[] SPECIAL_COMPRESSOR_FORMATS = {"dc"};

        public override bool TryParse(string value)
        {
            string withoutHyphen = value.Substring(1).ToLower();

            if (SPECIAL_COMPRESSOR_FORMATS.Contains(withoutHyphen)) // Special formatting
            {
                this._specialFormatting = withoutHyphen;
                return true;
            }

            if (withoutHyphen.All(char.IsDigit))
            {
                if (int.TryParse(withoutHyphen, out int result))
                {
                    this.FixedTargetSize = result;
                    this.HasTargetSize = true;
                    return true;
                }

                Console.WriteLine($"Aus dem gegebenen Parameter {value} konnte keine Dateigröße abgeleitet werden.");

                this.HasTargetSize = false;
                return false;
            }

            this.HasTargetSize = false;
            return false;
        }

        public int CalculateBitrateWithFixedTargetSize(double videoLengthInSeconds)
        {
            int targetBitSize = this.FixedTargetSize * 1000 * 8;
            return (int) (targetBitSize / videoLengthInSeconds);
        }

        public string ToIntendedSizePrintingString()
        {
            return PrintHelper.SplitStringBy("Die Zieldatei versucht eine Größe von:", ' ',
                $"{this.FixedTargetSize}MB einzunehmen.");
        }

        public static async Task<BitRateHolder> LoadFormats()
        {
            string STANDARDBITRATE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StandardBitrate.json");
            if (File.Exists(STANDARDBITRATE_PATH))
            {
                string jsonString = await File.ReadAllTextAsync(STANDARDBITRATE_PATH);
                BitRateHolder holder = JsonConvert.DeserializeObject<BitRateHolder>(jsonString);
                BitRateHolder.Instance = holder;
                return holder;
            }

            FileStream stream = File.Create(STANDARDBITRATE_PATH);
            BitRateHolder exampleHolder = new BitRateHolder()
            {
                dc = 4000
            };

            stream.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(exampleHolder)));
            stream.Close();

            return exampleHolder;
        }

        public int BitRateFromFormatting(BitRateHolder bitRates)
        {
            return BitUtils.BitRateFromFormatting(bitRates, this._specialFormatting);
        }
    }
}