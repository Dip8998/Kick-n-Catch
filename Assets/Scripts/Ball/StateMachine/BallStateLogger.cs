using UnityEngine;

namespace KNC.Ball.StateMachine
{
    public static class BallStateLogger
    {
        public static void Enter(BallState state)
        {
            Debug.Log($"[BallFSM] Enter → {state}");
        }

        public static void Exit(BallState state)
        {
            Debug.Log($"[BallFSM] Exit  → {state}");
        }
    }
}
