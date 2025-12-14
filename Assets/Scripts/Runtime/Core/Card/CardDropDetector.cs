using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame
{
    public class CardDropDetector : MonoBehaviour
    {
        [SerializeField] private string boardTargetTag = "Board";
        [SerializeField] private string handTargetTag = "Hand";
        [SerializeField] private CardVisual cardVisual;

        private Card m_parentCard;
        private EventSystem eventSystem;

        void Awake()
        {
            eventSystem = EventSystem.current;
            m_parentCard = GetComponent<Card>();
        }
        private void OnEnable()
        {
            cardVisual.EndDragEvent.AddListener(TryAttachToSlot);
        }
        private void OnDisable()
        {
            cardVisual.EndDragEvent.RemoveListener(TryAttachToSlot);
        }
        public void TryAttachToSlot(CardVisual cardVisual)
        {
            PointerEventData pointerData = new PointerEventData(eventSystem)
            {
                position = cardVisual.ScreenPos.ReadValue<Vector2>()
            };

            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject == gameObject || result.gameObject.transform.IsChildOf(transform) || result.gameObject.TryGetComponent<Card>(out Card card))
                    continue;

                Image image = result.gameObject.GetComponent<Image>();
                if (image == null) continue;

                //Debug.Log($"Image {image.gameObject.name}");

                if (image.CompareTag(handTargetTag))
                {
                    AttachTo(image.rectTransform, false);
                    return;
                }
                else if (image.CompareTag(boardTargetTag))
                {
                    AttachTo(image.rectTransform, true);
                    return;
                }

                return;
            }
        }

        void AttachTo(RectTransform parent, bool isPlaced)
        {
            m_parentCard.IsPlaced = isPlaced;
            transform.parent.SetParent(parent, false);
            transform.eulerAngles = Vector3.zero;
        }
    }

}
