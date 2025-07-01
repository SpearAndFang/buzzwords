using ProtoBuf;

namespace BuzzWords.ModSystem
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SyncClientPacket
    {
        public int BuzzRadius;
        public int BuzzFontSize;
        public string BuzzFontColor;
    }
}