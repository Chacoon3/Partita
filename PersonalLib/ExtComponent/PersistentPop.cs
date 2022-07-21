/*-------------------------------------------------------------------------
 * 作者：张自正
 * 创建时间：2022/7/14 16:31:47
 * 本类主要用途描述：
 *------------------------------------------------------------------------*/
using System;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Manager.Scripts.Profession.UI {
    /// <summary>
    /// 持久型弹窗，显示到条件达成
    /// </summary>
    class PersistentPop : MonoBehaviour, IDragHandler {

        static ObjectPool<PersistentPop> pool;      
        static bool allowDrag;      //是否允许拖动
        const float textSwitchSpan = .5F;       //动态文本切换频率

        RectTransform panelRectTf;
        TMP_Text[] texts;
        Func<bool> showUntil;

        string[] dynamicText;
        float timeCounter;
        uint textCursor = 0;

        void Awake() {
            panelRectTf = (RectTransform)transform.Find("Panel");
            texts = panelRectTf.transform.Find("TmpTexts").GetComponentsInChildren<TMP_Text>();
        }

        void Update() {
            if (showUntil()) {
                pool.Release(this);
                return;
            }

            if (dynamicText != null && dynamicText.Length > 0) {
                timeCounter += Time.deltaTime;
                if (timeCounter >= textSwitchSpan) {
                    timeCounter = 0;
                    texts[1].text = dynamicText[++textCursor % dynamicText.Length];
                }
            }
        }

        //重置自身状态
        void SelfReset() {
            showUntil = null;
            dynamicText = null;
            timeCounter = 0;
            textCursor = 0;

            panelRectTf.anchoredPosition = Vector2.zero;
        }

        void IDragHandler.OnDrag(PointerEventData eventData) {
            if (!allowDrag) {
                return;
            }

            if (eventData.pointerPressRaycast.gameObject == panelRectTf.gameObject) {
                var newPos = panelRectTf.anchoredPosition + eventData.delta;
                float xMin = -0.5f * ((transform as RectTransform).rect.width - panelRectTf.sizeDelta.x);
                float xMax = 0.5f * ((transform as RectTransform).rect.width - panelRectTf.sizeDelta.x);
                float yMin = -0.5f * ((transform as RectTransform).rect.height - panelRectTf.sizeDelta.y);
                float yMax = 0.5f * ((transform as RectTransform).rect.height - panelRectTf.sizeDelta.y);
                newPos.x = Mathf.Clamp(newPos.x, xMin, xMax);
                newPos.y = Mathf.Clamp(newPos.y, yMin, yMax);
                panelRectTf.anchoredPosition = newPos;
            }
        }

        //依赖注入
        public static void Inject(GameObject protoObj, RectTransform parent, bool allowDrag = true) {
            PersistentPop.allowDrag = allowDrag;

            pool = new ObjectPool<PersistentPop>(
                    pop => { pop.gameObject.SetActive(true); },
                    pop => { pop.gameObject.SetActive(false); pop.SelfReset(); },
                    () => Instantiate(protoObj, parent).AddComponent<PersistentPop>());
        }

        public static PersistentPop Show(string title, string content, Func<bool> showUntil) {
            var res = pool.Get();
            res.texts[0].text = title;
            res.texts[1].text = content;
            res.showUntil = showUntil;
            res.panelRectTf.anchoredPosition = Vector2.zero;
            res.dynamicText = null;
            res.textCursor = 0;
            res.transform.SetAsLastSibling();
            return res;
        }

        public static PersistentPop Show(string title, string[] dynamicContent, Func<bool> showUntil) {
            var res = pool.Get();
            res.texts[0].text = title;
            res.texts[1].text = dynamicContent[0];
            res.showUntil = showUntil;
            res.panelRectTf.anchoredPosition = Vector2.zero;
            if (dynamicContent != null && dynamicContent.Length > 0) {
                res.dynamicText = dynamicContent;
            }
            res.textCursor = 0;
            res.transform.SetAsLastSibling();
            return res;
        }

        public static bool AnyPanelActive() {
            return pool.countActive > 0;
        }
    }
}