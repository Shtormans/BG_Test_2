using Fusion;
using UnityEngine;

namespace MainGame
{
    public class WeaponAnimationController : NetworkBehaviour
    {
        [SerializeField] private Animator _animator;

        public void SetAnimationTrigger(AnimationTriggers trigger)
        {
            RPC_SendAnimationTrigger(trigger);
        }

        [Rpc(RpcSources.All, RpcTargets.All)]
        private void RPC_SendAnimationTrigger(AnimationTriggers animationTrigger)
        {
            _animator.SetTrigger(animationTrigger.ToString());
        }
    }
}
