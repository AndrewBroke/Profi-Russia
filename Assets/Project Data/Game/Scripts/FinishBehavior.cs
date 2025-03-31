using UnityEngine;

namespace Watermelon
{
    public class FinishBehavior : MonoBehaviour
    {
        [SerializeField] Transform finishPoint;
        private UIGame gameUI;


        private void Start()
        {
            gameUI = UIController.GetPage<UIGame>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == (int)PhysicsLayer.Rover)
            {
                var rover = collision.GetComponent<RoverBehavior>();

                if (rover != null)
                {
                    rover.Finish(finishPoint);

                    GameController.OnLevelCompleted();
                }

            }
        }
    }
}