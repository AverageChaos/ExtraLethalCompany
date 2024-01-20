using LC_API.Networking;

namespace ExtraLethalCompany.Extra.Furniture.ExtraItemCharger
{
    public class ExtraItemCharger(ulong id, bool broken)
    {
        public ulong ID = id;
        public bool IsBroken = broken;

        public void ResetItemCharger()
        {
            IsBroken = false;
            Network.Broadcast($"IsBroken{ID}", this);
        }
    }
}
