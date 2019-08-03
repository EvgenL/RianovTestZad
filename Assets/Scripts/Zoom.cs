using UnityEngine;

namespace Assets.Scripts
{
    public class Zoom : MonoBehaviour
    {
        // height in game squares
        [SerializeField] private int _minHeight = 5;
        [SerializeField] private int _maxHeight = 10000;
        [SerializeField] private float _scrollZoomSpeed = 100;
        [SerializeField] private float _pinchZoomSpeed = 0.5f;      

        private Camera _cam;

        private void Start()
        {
            _cam = GetComponent<Camera>();

            _cam.orthographicSize = _minHeight * GameManager.Instance.Step;
        }

        private void Update()
        {
            CheckScrollZoom();
            CheckPinchZoom();
        }

        private void CheckPinchZoom()
        {
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
                float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                GameManager.Instance.OnCameraMoved();

                _cam.orthographicSize += deltaMagnitudeDiff * _pinchZoomSpeed;

                _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize,
                    _minHeight * GameManager.Instance.Step, _maxHeight * GameManager.Instance.Step);
            }
        }

        private void CheckScrollZoom()
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");

            if (zoom != 0f) GameManager.Instance.OnCameraMoved();

            _cam.orthographicSize += -zoom * 100;

            _cam.orthographicSize = Mathf.Clamp(_cam.orthographicSize, 
                _minHeight * GameManager.Instance.Step, _maxHeight * GameManager.Instance.Step);
        }
    }
}
