namespace BuzzWords.ModSystem
{
    using BuzzWords.ModConfig;
    using Vintagestory.API.Client;
    using Vintagestory.API.Common;
    using Vintagestory.API.Server;

    public class BuzzWordsSystem : ModSystem
    {
        
        private readonly string thisModID = "buzzwords";
        private IServerNetworkChannel serverChannel;
        private ICoreAPI api;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return true;
        }

        public override void Start(ICoreAPI api)
        {
            this.api = api;
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

        public override void StartClientSide(ICoreClientAPI capi)
        {
            capi.Network.RegisterChannel("buzzwords")
                .RegisterMessageType<SyncClientPacket>()
                .SetMessageHandler<SyncClientPacket>(packet =>
                {

                    ModConfig.Loaded.BuzzRadius = packet.BuzzRadius;
                    this.Mod.Logger.Event($"Received BuzzRadius of {packet.BuzzRadius} from server");
                    ModConfig.Loaded.BuzzFontSize = packet.BuzzFontSize;
                    this.Mod.Logger.Event($"Received BuzzFontSize of {packet.BuzzFontSize} from server");
                    ModConfig.Loaded.BuzzFontColor = packet.BuzzFontColor;
                    this.Mod.Logger.Event($"Received BuzzFontColor of {packet.BuzzFontColor} from server");
                });

            
        }

        public override void StartServerSide(ICoreServerAPI sapi)
        {
            // send connecting players the config settings
            sapi.Event.PlayerJoin += this.OnPlayerJoin; // add method so we can remove it in dispose to prevent memory leaks
            // register network channel to send data to clients
            this.serverChannel = sapi.Network.RegisterChannel("buzzwords")
                .RegisterMessageType<SyncClientPacket>()
                .SetMessageHandler<SyncClientPacket>((player, packet) => { /* do nothing. idk why this handler is even needed, but it is */ });
        }

        private void OnPlayerJoin(IServerPlayer player)
        {
            // send the connecting player the settings it needs to be synced
            this.serverChannel.SendPacket(new SyncClientPacket
            {
                BuzzRadius = ModConfig.Loaded.BuzzRadius,
                BuzzFontSize = ModConfig.Loaded.BuzzFontSize,
                BuzzFontColor = ModConfig.Loaded.BuzzFontColor
            }, player);
        }

        public override void Dispose()
        {
            // remove our player join listener so we dont create memory leaks
            if (this.api is ICoreServerAPI sapi)
            {
                sapi.Event.PlayerJoin -= this.OnPlayerJoin;
            }
        }
    }
}
