using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CardGame
{
    public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField]
        private InputAction screenPos;

        public bool IsDragging => isDragging;

        [Header("Movement")]
        [SerializeField] private float moveSpeedLimit = 50;

        [Header("Events")]
        public UnityEvent<Card> BeginDragEvent;
        public UnityEvent<Card> EndDragEvent;

        [SerializeField]
        private CurveParameters curve;

        private float curveYOffset;
        private float curveRotationOffset;
        private bool isDragging;
        private Vector3 offset;
        private Camera cam;

        private void OnEnable()
        {
            cam = Camera.main;
            screenPos.Enable();
        }
        private void OnDisable()
        {
            screenPos.Disable();
        }
        private void Update()
        {
            ClampPosition();
            if (isDragging)
            {
                Vector2 targetPosition = cam.ScreenToWorldPoint(screenPos.ReadValue<Vector2>()) - offset;
                Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
                transform.eulerAngles = Vector3.zero;
                transform.Translate(velocity * Time.deltaTime);
            }
            else
                CardPositioning();
        }

        private void ClampPosition()
        {
            Vector2 screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
        }

        private void CardPositioning()
        {
            curveYOffset = (curve.positioning.Evaluate(NormalizedPosition()) * curve.positioningInfluence) * transform.parent.parent.childCount;
            curveYOffset = transform.parent.parent.childCount < 5 ? 0 : curveYOffset;
            curveRotationOffset = curve.rotation.Evaluate(NormalizedPosition());
            Vector3 verticalOffset = (Vector3.up * (IsDragging ? 0 : curveYOffset));
            transform.position = transform.parent.position + verticalOffset;
            var rot = transform.eulerAngles;
            rot.z = IsDragging ? transform.eulerAngles.z : (curveRotationOffset * (curve.rotationInfluence * SiblingAmount()));
            transform.eulerAngles = rot;
        }

        public int ParentIndex()
        {
            return transform.parent.CompareTag("CardSlot") ? transform.parent.GetSiblingIndex() : 0;
        }
        public int SiblingAmount()
        {
            return transform.parent.CompareTag("CardSlot") ? transform.parent.parent.childCount - 1 : 0;
        }
        public float NormalizedPosition()
        {
            return transform.parent.CompareTag("CardSlot") ? ExtensionMethods.Remap((float)ParentIndex(), 0, (float)(transform.parent.parent.childCount - 1), 0, 1) : 0;
        }

        #region InputHandling
        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDragEvent?.Invoke(this);
            isDragging = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(screenPos.ReadValue<Vector2>());
            offset = mousePosition - (Vector2)transform.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDragEvent?.Invoke(this);
            isDragging = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
        public void OnDrag(PointerEventData eventData) { }
        #endregion
    }
}
