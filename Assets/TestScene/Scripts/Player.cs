using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private Ball _prefabBall;
    [SerializeField] private PhysxBall _prefabPhysxBall;

    private Vector3 _forward = Vector3.forward;
    private NetworkCharacterController _cc;
    private Material _material;
    private ChangeDetector _changeDetector;

    [Networked] private TickTimer Delay { get; set; }
    [Networked] public bool IsSpawned { get; set; }


    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _material = GetComponentInChildren<MeshRenderer>().material;
    }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(IsSpawned):
                    _material.color = Color.white;
                    break;
            }
        }

        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0)
            {
                _forward = data.direction;
            }
            if (HasStateAuthority && Delay.ExpiredOrNotRunning(Runner))
            {
                if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON1))
                {
                    Delay = TickTimer.CreateFromSeconds(Runner, 0.5f);

                    Runner.Spawn(_prefabBall,
                                 transform.position + _forward,
                                 Quaternion.LookRotation(_forward),
                                 Object.InputAuthority,
                                 (runner, o) =>
                                 {
                                     // Initialize the Ball before synchronizing it
                                     o.GetComponent<Ball>().Init();
                                 });

                    IsSpawned = !IsSpawned;
                }
                else if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON2))
                {
                    Delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_prefabPhysxBall,
                                 transform.position + _forward,
                                 Quaternion.LookRotation(_forward),
                                 Object.InputAuthority,
                                 (runner, o) =>
                                 {
                                     o.GetComponent<PhysxBall>().Init(10 * _forward);
                                 });

                    IsSpawned = !IsSpawned;
                }
            }
        }
    }
}
