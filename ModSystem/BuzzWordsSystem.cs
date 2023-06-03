namespace BuzzWords.ModSystem
{
    using Vintagestory.API.Common;

    public class BuzzWordsSystem : ModSystem
    {
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return true;
        }

        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterBlockEntityBehaviorClass("BuzzBeehive", typeof(BEBBuzzBeehive));

        }
    }
}
