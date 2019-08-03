using UnityEngine;

namespace Assets.Scripts
{
    public class ShipController : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.OnShipMoved(transform.position);
        }

        public void Move(int x, int y)
        {
            var pos = transform.position;
            pos.x += x * GameManager.Instance.Step;
            pos.y += y * GameManager.Instance.Step;
            transform.position = pos;



            var cam = Camera.main;
            var rect = transform as RectTransform;

            bool isVisible = rect.IsFullyVisibleFrom(cam);

            if (!isVisible)
            {
                cam.transform.position = transform.position;
                cam.transform.position += new Vector3(0f, 0f, -1f);

                GameManager.Instance.OnCameraMoved();
            }

            GameManager.Instance.OnShipMoved(transform.position);
        }
    }
}
