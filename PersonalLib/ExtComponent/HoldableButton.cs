using Partita.Utility;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// 长按型按钮
    /// </summary>
    /// <remarks>onClick事件只在长按 pressLength秒 后触发</remarks>
    /// <remarks>isPressing标识按钮目前是否被点按（包括无效点按的情况）</remarks>
    public class HoldableButton : Button {

        /*
         * isPressing 记录用户是否在长按本按钮  （但凡按钮在按压状态，isPressing都为true)
         * isValidPressing记录当前的长按是否有效（例子，长按2秒时触发onClick事件，此后若用户仍然长按本按钮，则：isPressing为true，但isValidPressing为false
         */

        static ObjectPool<ProgressIndicator> indicatorPool; //对象池管理悬浮进度条
        static GameObject indicatorPrototype;
        static Vector2? progressBarOffset;

        public float pressLength {
            get => _pressLength;
            set {
                if (value == _pressLength) {
                    return;
                }
                if (value < 0) {
                    value = float.Epsilon;
                }

                _pressLength = value;
            }
        }

        public UnityEvent<bool> onPressStateChange => _onReleaseStateChange;
        UnityEvent<bool> _onReleaseStateChange = new UnityEvent<bool>();

        float timeCounter;
        float _pressLength = 2;
        bool isPreseing, isValidPressing;

        ProgressIndicator indicator;

        protected override void Awake() {
            base.Awake();

            if (indicatorPrototype == null) {
                throw new NotImplementedException();
            }

            if (indicatorPool == null) {
                indicatorPool = new ObjectPool<ProgressIndicator>(
                    pi => pi.gameObject.SetActive(true),
                    pi => pi.gameObject.SetActive(false),
                    () => {
                        throw new NotImplementedException();
                    });
            }

            if (!progressBarOffset.HasValue) {
                progressBarOffset = new Vector2(0, -25);
            }
        }

        void Update() {
            if (!isPreseing) {
                return;
            }

            if (isValidPressing) {
                timeCounter += Time.deltaTime;
                indicator.SetProgress01(timeCounter / pressLength);
                indicator.SetText(string.Format("{0}/{1}秒", Mathf.Clamp(timeCounter, 0, pressLength).ToString("0.0"), pressLength.ToString("0.0")));

                if (timeCounter >= pressLength) {
                    InvokeClickEvent();
                }
            }
        }

        void SetIndicatorPos() {
            if (!indicator) {
                return;
            }

            RectTransform rectTf = (RectTransform)indicator.transform;
            rectTf.AnchorTo((RectTransform)transform, new Vector2(0.5f, 1), new Vector2(0.5f, 0), progressBarOffset.Value);
        }

        void InvokeClickEvent() {
            isValidPressing = false;
            timeCounter = 0;
            indicator.SetColor(Color.green);
            onClick.Invoke();
        }

        internal static void Unload() {
            indicatorPool = null;
            indicatorPrototype = null;
            progressBarOffset = null;
        }

        #region UNITY 接口
        public override void OnPointerClick(PointerEventData eventData) {
            return;
        }

        public override void OnPointerDown(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) {
                return;
            }

            if (!interactable) {
                return;
            }

            timeCounter = 0;
            isPreseing = true;
            isValidPressing = true;
            indicator = indicatorPool.Get();
            indicator.gameObject.SetActive(true);
            indicator.SetColor(Color.blue);
            SetIndicatorPos();

            onPressStateChange.Invoke(isPreseing);
            DoStateTransition(SelectionState.Pressed, false);
        }

        public override void OnPointerUp(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) {
                return;
            }

            if (interactable) {
                if (timeCounter >= pressLength) {
                    InvokeClickEvent();
                }

                indicator.gameObject.SetActive(false);
                indicatorPool.Release(indicator);
                indicator = null;

                isPreseing = false;
                onPressStateChange.Invoke(isPreseing);

                DoStateTransition(SelectionState.Normal, false);
            }
            else {
                if (indicator) {
                    indicator.gameObject.SetActive(false);
                    indicatorPool.Release(indicator);
                    indicator = null;
                }
                return;
            }
        }
        #endregion
    }
}