using UnityEngine;
using KNC.Ball.StateMachine;

namespace KNC.Ball
{
    public class BallController
    {
        private BallView view;
        private readonly BallScriptableObject so;
        private Transform parent;

        private Rigidbody2D rb;
        private BallStateMachine stateMachine;

        private const float AirborneThreshold = 0.05f;

        public BallController(BallScriptableObject so)
        {
            this.so = so;
        }

        public void Initialize()
        {
            parent = CreateParent("_Ball");

            view = Object.Instantiate(so.BallPrefab, parent);
            view.transform.position = so.BallSpawnPos;
            view.InitializeView(this);

            rb = view.GetComponent<Rigidbody2D>();

            stateMachine = new BallStateMachine(this);
        }

        public void Tick()
        {
            stateMachine.Update();
        }

        public void ChangeState(BallState state)
        {
            stateMachine.ChangeState(state);
        }

        public void StopBall()
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        public bool IsAirborne()
        {
            return Mathf.Abs(rb.linearVelocity.y) > AirborneThreshold;
        }

        public bool HasLanded()
        {
            return Mathf.Abs(rb.linearVelocity.y) <= AirborneThreshold;
        }

        private Transform CreateParent(string name)
        {
            return new GameObject(name).transform;
        }
    }
}
