using Fusion;
using UnityEngine;

namespace MainGame
{
    public class DeadCopy : NetworkBehaviour
    {
        [SerializeField] private float _timeToDisappear = 3f;

        [Networked] private TickTimer Life { get; set; }

        public override void Spawned()
        {
            Life = TickTimer.CreateFromSeconds(Runner, _timeToDisappear);
        }

        public override void FixedUpdateNetwork()
        {
            if (Life.Expired(Runner))
            {
                Runner.Despawn(Object);
            }
        }
    }
}
