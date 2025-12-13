using KNC.Core.Services;
using UnityEngine;

namespace KNC.Player
{
    public class PlayerController
    {
        private PlayerView view;
        private readonly PlayerScriptableObject so;
        private Transform parent;
        private float moveInput;

        public PlayerController(PlayerScriptableObject so)
        {
            this.so = so;
        }

        public void Initialize()
        {
            parent = CreateParent("_Player");
            view = GameObject.Instantiate(so.PlayerPrefab, parent);
            view.transform.position = so.PlayerSpawnPos;
            view.InitializeView(this);
        }

        public void ReadInput()
        {
            moveInput = InputService.Instance.Horizontal;
        }

        public void FixedTick(float fixedDeltaTime)
        {
            HandleMovement(fixedDeltaTime);
        }

        private void HandleMovement(float fixedDeltaTime)
        {
            Rigidbody2D rb = view.Rigidbody;

            float targetX =
                rb.position.x +
                moveInput * so.PlayerMoveSpeed * fixedDeltaTime;

            targetX = Mathf.Clamp(targetX, so.MinX, so.MaxX);

            Vector2 targetPos = new Vector2(targetX, rb.position.y);

            rb.MovePosition(targetPos);
        }

        private Transform CreateParent(string name)
        {
            return new GameObject(name).transform;
        }
    }
}
