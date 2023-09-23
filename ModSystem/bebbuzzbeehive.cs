namespace BuzzWords.ModSystem
{
    using System;
    using System.Linq;
    using Vintagestory.API.Common;
    using Vintagestory.API.Server;
    using Vintagestory.API.MathTools;
    using Vintagestory.API.Datastructures;
    using BuzzWords.ModConfig;


    public class BEBBuzzBeehive : BlockEntityBehavior
    {
        private readonly int buzzRadius = ModConfig.Loaded.BuzzRadius;
        private readonly int buzzFontSize = ModConfig.Loaded.BuzzFontSize;
        private readonly string buzzFontColor = ModConfig.Loaded.BuzzFontColor;

        public BEBBuzzBeehive(BlockEntity blockentity) : base(blockentity)
        { }

        public override void Initialize(ICoreAPI api, JsonObject properties)
        {
            base.Initialize(api, properties);
            if (api.Side == EnumAppSide.Server)
            {
                this.Blockentity.RegisterGameTickListener(this.Buzz, 2500);
            }
        }

        private int LimitInclusive(int value, int min, int max)
        { return Math.Min(max, Math.Max(value, min)); }

        private void Buzz(float dt)
        {
            if (this.Pos != null)
            {
                if (this.Api.World.Side == EnumAppSide.Client)
                { return; }

                var player = this.Api.World.NearestPlayer(this.Pos.X, this.Pos.Y, this.Pos.Z);
                if (player?.Entity?.ServerPos == null)
                { return; }

                var tempPos = new BlockPos((int)player.Entity?.Pos.X, (int)player.Entity?.Pos.Y, (int)player.Entity?.Pos.Z);
                if (tempPos == null)
                { return; }

                var dist = Math.Max(1, this.Pos.DistanceTo(tempPos) - 2);
                if (dist < this.buzzRadius)
                {
                    var zCount = (int)(this.buzzRadius - dist);
                    zCount = this.LimitInclusive(zCount, 3, 3 + this.buzzRadius);
                    var splr = player as IServerPlayer;
                    if (splr == null)
                    { return; }

                    var msg = "B " + string.Concat(Enumerable.Repeat("z Z ", zCount)) + "z . . .";
                    msg = "<strong><font size=\"" + this.buzzFontSize + "\" color=\"" + this.buzzFontColor + "\">" + msg + "</font></strong>";
                    splr.SendIngameError("buzz", msg);
                }
            }
        }
    }
}
