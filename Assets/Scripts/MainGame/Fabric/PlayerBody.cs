using Fusion;

namespace MainGame
{
    public struct PlayerBody : INetworkStruct
    {
        public NetworkBehaviourId WeaponId;
        public NetworkBehaviourId SpriteId;
    }
}