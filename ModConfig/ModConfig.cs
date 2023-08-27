namespace BuzzWords.ModConfig
{
    public class ModConfig
    {
        public static ModConfig Loaded { get; set; } = new ModConfig();
        public int BuzzRadius { get; set; } = 12;
    }
}
