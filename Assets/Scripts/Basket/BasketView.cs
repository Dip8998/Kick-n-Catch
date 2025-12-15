using UnityEngine;
using KNC.Ball;
using KNC.Player;

public class BasketView : MonoBehaviour
{
    private PlayerController controller;

    public void Initialize(PlayerController pc) => controller = pc;

    private void OnTriggerEnter2D(Collider2D other)
    {
        BallView ballView = other.GetComponent<BallView>();
        if (ballView != null)
        {
            if (ballView.Controller.CanBeCaught())
            {
                ballView.Controller.Catch();
            }
        }
    }
}
