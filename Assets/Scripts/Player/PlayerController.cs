using KNC.Core.Services;
using KNC.Player.StateMachine;
using UnityEngine;

namespace KNC.Player
{
    public class PlayerController
    {
        private PlayerView view;
        private readonly PlayerScriptableObject so;
        private PlayerStateMachine stateMachine;
        private Transform parent;

        public float MoveInput { get; private set; }
        public Rigidbody2D Rigidbody => view.Rigidbody;

        public PlayerController(PlayerScriptableObject so)
        {
            this.so = so;
        }

        public void Initialize()
        {
            parent = new GameObject("_Player").transform;

            view = GameObject.Instantiate(so.PlayerPrefab, parent);
            view.transform.position = so.PlayerSpawnPos;
            view.InitializeView(this);

            stateMachine = new PlayerStateMachine(this);
        }

        public void ReadInput()
        {
            MoveInput = InputService.Instance.Horizontal;
        }

        public void Tick()
        {
            stateMachine.Update();
        }

        public void Move(float fixedDeltaTime)
        {
            float targetX =
                Rigidbody.position.x +
                MoveInput * so.PlayerMoveSpeed * fixedDeltaTime;

            targetX = Mathf.Clamp(targetX, so.MinX, so.MaxX);

            Rigidbody.MovePosition(
                new Vector2(targetX, Rigidbody.position.y)
            );
        }
    }
}
