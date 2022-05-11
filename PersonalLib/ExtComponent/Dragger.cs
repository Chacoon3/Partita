using UnityEngine;
using UnityEngine.EventSystems;

namespace Partita.ExtComponent {
    /// <summary>
    /// 实现UI物体的拖拽功能
    /// </summary>
   public class Dragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        /// <summary>
        /// 激活时是否重置默认位置
        /// </summary>
        public bool resetPosOnEnable { get; set; }
        Vector2 defaultPos;
        Vector2 mousePosLastFrame;
        RectTransform parentTransform;

        bool isStarted;
        bool isDragging;

        void Start() {
            isStarted = true;
            parentTransform = (RectTransform)gameObject.transform.parent;
            defaultPos = parentTransform.anchoredPosition;
        }

        void OnEnable() {
            if (!isStarted) {
                return;
            }

            if (resetPosOnEnable) {
                parentTransform.anchoredPosition = defaultPos;
            }
        }

        void Update() {

            if (!isDragging) {
                return;
            }

            Vector2 mousePos = Input.mousePosition;
            parentTransform.anchoredPosition += mousePos - mousePosLastFrame;
            mousePosLastFrame = mousePos;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
            isDragging = true;
            mousePosLastFrame = Input.mousePosition;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
            isDragging = false;
        }
    }
}