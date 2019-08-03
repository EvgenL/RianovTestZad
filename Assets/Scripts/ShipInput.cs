using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(ShipController))]
    public class ShipInput : MonoBehaviour
    {
        private ShipController _controller;

        
        Vector2 firstPressPos;
        Vector2 secondPressPos;
        Vector2 currentSwipe;

        private void Update()
        {
            CheckKeyboardInput();
            CheckSwipeInput();
        }

        private void Awake()
        {
            _controller = GetComponent<ShipController>();
        }

        private void CheckKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                // up
                _controller.Move(0, 1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                // dn
                _controller.Move(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                // rt
                _controller.Move(1, 0);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                // lt
                _controller.Move(-1, 0);
            }
        }

        private void CheckSwipeInput()
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    //save began touch 2d point
                    firstPressPos = new Vector2(t.position.x, t.position.y);
                }
                if (t.phase == TouchPhase.Ended)
                {
                    //save ended touch 2d point
                    secondPressPos = new Vector2(t.position.x, t.position.y);

                    //create vector from the two points
                    currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                    //normalize the 2d vector
                    currentSwipe.Normalize();

                    //swipe upwards
                    if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        // up
                        _controller.Move(0, 1);
                    }
                    //swipe down
                    if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        // dn
                        _controller.Move(0, -1);
                    }
                    //swipe left
                    if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        // lt
                        _controller.Move(-1, 0);
                    }
                    //swipe right
                    if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        // rt
                        _controller.Move(1, 0);
                    }
                }
            }
        }
    }
}
