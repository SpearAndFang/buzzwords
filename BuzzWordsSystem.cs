namespace BuzzWords.ModSystem
{
    using Vintagestory.API.Common;
    using BuzzWords.ModConfig;

    public class BuzzWordsSystem : ModSystem
    {
        private readonly string thisModID = "buzzwords";

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return true;
        }

        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.World.Logger.Event("started 'Buzzwords' mod");
            api.RegisterBlockEntityBehaviorClass("BuzzBeehive", typeof(BEBBuzzBeehive));

        }

        public override void StartPre(ICoreAPI api)
        {
            // Load/create common config file in ..\VintageStoryData\ModConfig\thisModID
            var cfgFileName = this.thisModID + ".json";
            try
            {
                ModConfig fromDisk;
                if ((fromDisk = api.LoadModConfig<ModConfig>(cfgFileName)) == null)
                { api.StoreModConfig(ModConfig.Loaded, cfgFileName); }
                else
                { ModConfig.Loaded = fromDisk; }
            }
            catch
            {
                api.StoreModConfig(ModConfig.Loaded, cfgFileName);
            }
            base.StartPre(api);
        }

    }
}
