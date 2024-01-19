using Fusion;

namespace MainGame
{
    public struct EntityHitStatus : INetworkStruct
    {
        public int Damage;
        public int HealthRemained;
        public bool Died;
    }
}
