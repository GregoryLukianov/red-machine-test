using UnityEngine;

namespace Camera
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField][Range(0.1f, 10.0f)] private float sensitivity = 1.0f; 
        [SerializeField] private SpriteRenderer boundarySprite;
        [SerializeField] private LayerMask backgroundLayer;
        [SerializeField] private LayerMask nonMovableLayers;
    
        private UnityEngine.Camera _camera;
        private float minX, maxX, minY, maxY;
        private Vector3 touchStart;
        private bool isTouching = false;
    
    
        private void Start()
        {
            _camera = CameraHolder.Instance.MainCamera;
            CalculateBounds();
        }

        private void Update()
        {
            if (Input.touchCount != 0)
                TouchInput();
            else
                MouseInput();
        }

        private void TouchInput()
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = _camera.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began && IsTouchingBackground(touchPosition))
            {
                touchStart = touchPosition;
                isTouching = true;
            }
            else if (touch.phase == TouchPhase.Moved && isTouching)
            {
                Vector3 direction = touchStart - touchPosition;
                direction.z = 0;
                Vector3 newPosition = transform.position + direction * sensitivity;
                transform.position = ClampPosition(newPosition);

                touchStart = touchPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                isTouching = false;
            
        }

        private void MouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

                if (IsTouchingBackground(mousePosition))
                {
                    touchStart = mousePosition;
                    isTouching = true;
                }
            }
            else if (Input.GetMouseButton(0) && isTouching)
            {
                Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = touchStart - mousePosition;
                direction.z = 0;
                Vector3 newPosition = transform.position + direction * sensitivity;
                transform.position = ClampPosition(newPosition);

                touchStart = mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
                isTouching = false;
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);
            return position;
        }

        private void CalculateBounds()
        {
            if (boundarySprite == null)
                return;
            
            Bounds bounds = boundarySprite.bounds;
            minX = bounds.min.x;
            maxX = bounds.max.x;
            minY = bounds.min.y;
            maxY = bounds.max.y;
        }

        private bool IsTouchingBackground(Vector3 position)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, Mathf.Infinity, nonMovableLayers | backgroundLayer);
            if (hit.collider != null)
                return hit.collider.gameObject.layer == LayerMask.NameToLayer("Playground");
            
            return false;
        }
    }
}
