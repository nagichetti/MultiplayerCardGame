using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace CardGame
{
    public class CardVisual : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler //,IDragHandler
    {
        [SerializeField]
        private InputAction screenPos;
        [SerializeField]
        private Card m_card;

        private SortingGroup m_sorting;
        private Camera cam;
        private Image m_cardImage;


        [Header("Movement")]
        [SerializeField]
        private float moveSpeedLimit = 50;
        [SerializeField]
        private CurveParameters curve;
        [SerializeField]
        private Vector3 selectedOffset;

        private Vector3 offset;
        private float curveYOffset;
        private float curveRotationOffset;
        private bool isDragging;
        private bool isSelected;
        private bool canSelect;

        [Header("Events")]
        [HideInInspector] public UnityEvent<CardVisual> BeginDragEvent;
        [HideInInspector] public UnityEvent<CardVisual> EndDragEvent;

        public bool IsDragging => isDragging;
        public InputAction ScreenPos => screenPos;

        public bool CanSelect { get => canSelect; set => canSelect = value; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }

        private void OnEnable()
        {
            cam = Camera.main;
            m_sorting = GetComponent<SortingGroup>();
            m_cardImage = GetComponent<Image>();
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
            else if (!m_card.IsPlaced)
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
            transform.position = isSelected ? transform.position + selectedOffset : transform.position;
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
            if (!transform.parent.CompareTag("CardSlot"))
                return 0f;

            int count = transform.parent.parent.childCount;

            if (count <= 1)
                return 0.5f;

            return ExtensionMethods.Remap(
                ParentIndex(),
                0,
                count - 1,
                0f,
                1f
            );
        }

        #region InputHandling
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanSelect) return;

            BeginDragEvent?.Invoke(this);
            isDragging = true;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(screenPos.ReadValue<Vector2>());
            offset = mousePosition - (Vector2)transform.position;
            m_cardImage.raycastTarget = false;
            m_sorting.sortingOrder = 5;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!CanSelect) return;

            EndDragEvent?.Invoke(this);
            isDragging = false;
            m_cardImage.raycastTarget = true;
            m_sorting.sortingOrder = 1;
        }

        public void OnPointerDown(PointerEventData eventData) { }
        public void OnPointerEnter(PointerEventData eventData) { }
        public void OnPointerExit(PointerEventData eventData) { }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!CanSelect) return;
            isSelected = !isSelected;
            if (IsSelected)
                CardManager.Instance.AddCardToSelected(m_card);
            else
                CardManager.Instance.RemoveCardFromSelected(m_card);
        }

        public void SetVisual(Sprite sprite)
        {
            m_cardImage = GetComponent<Image>();
            m_cardImage.sprite = sprite;
        }
        //public void OnDrag(PointerEventData eventData) { }
        #endregion
    }
}
