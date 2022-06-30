using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// 可滚动页签
    /// </summary>
    /// <remarks>每帧检测滑轮事件并滚动自身  滚动逻辑： int32枚举一切状态，mapperNormal将一个状态映射到页签上， mapperOutOfBound处理未定义的状态</remarks>
    [DisallowMultipleComponent]
    public class ScrollableTag : Image, IScrollHandler, IPointerClickHandler {

        private class TagItem {
            public Image image { get; private set; }
            public TMP_Text text { get; private set; }
            public GameObject gameObject { get; private set; }
            public TagItem(GameObject gameObject) {
                this.gameObject = gameObject;
                text = gameObject.GetComponentInChildren<TMP_Text>();
                image = gameObject.GetComponent<Image>();
            }
        }

        /// <summary>
        /// 页签物体的模板
        /// </summary>
        GameObject tagItemTemplate;

        /// <summary>
        /// 当前状态（即索引0的页签的状态）
        /// </summary>
        public int value {
            get => _value;
            set {
                value = Mathf.Clamp(value, min, max);

                if (value == _value) {
                    return;
                }

                UpdateDisplay(value);
                _value = value;
                onValueChanged?.Invoke(value);
            }
        }
        /// <summary>
        /// 最大值
        /// </summary>
        public int max {
            get => _max;
            set {
                _max = Mathf.Clamp(value, min, int.MaxValue);
            }
        }
        /// <summary>
        /// 最小值
        /// </summary>
        public int min {
            get => _min;
            set {
                _min = Mathf.Clamp(value, int.MinValue, max);
            }
        }
        /// <summary>
        /// 标签数量
        /// </summary>
        public int tagCount {
            get => _tagCount;
            set {
                value = Mathf.Clamp(value, 1, int.MaxValue);
                if (value == _tagCount) {
                    return;
                }

                OnTagSettingChanged(value);
                UpdateDisplay(value);
                _tagCount = value;
            }
        }

        public UnityEvent<int> onValueChanged { get; } = new UnityEvent<int>();

        List<TagItem> tagItems = new List<TagItem>();
        int _tagCount;
        int _value = 0;
        int _min = 0;
        int _max = int.MaxValue;

        #region 生命周期
        protected override void Awake() {
            base.Awake();

            tagItemTemplate = transform.Find("Template").gameObject;
            tagCount = 5;

            for (int i = 0; i < transform.childCount; i++) {
                tagItems.Add(new TagItem(transform.GetChild(i).gameObject));
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region 对外接口
        public virtual void SetWithoutNotify(int value) {
            _value = Mathf.Clamp(value, min, max);
            UpdateDisplay(value);
        }
        #endregion

        protected virtual void UpdateDisplay(int newValue) {

            for (int i = 0; i < tagItems.Count; i++) {
                int value = newValue + i;
                if (min <= value && value <= max) {
                    tagItems[i].text.text = value.ToString();
                }
                else {
                    tagItems[i].text.text = null;
                }
            }
        }

        protected virtual void OnTagSettingChanged(int newTagCount) {
            int differenceAbs = Mathf.Abs(newTagCount - tagCount);
            if (newTagCount > tagCount) {
                for (int i = 0; i < differenceAbs; i++) {
                    Instantiate(tagItemTemplate, transform);
                }
            }
            else {
                DestroyTags(differenceAbs);
            }
        }

        protected virtual void DestroyTags(int count) {
            count = Mathf.Clamp(count, 0, transform.childCount);
            for (int i = 0; i < count; i++) {
                Destroy(transform.GetChild(0).gameObject);
            }
        }

        #region Unity 接口
        public virtual void OnScroll(PointerEventData eventData) {
            if (eventData.scrollDelta.y < 0) {      //向下视为正方向
                value++;
            }
            else {
                value--;
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData) {
            GameObject hit = null;
            int hitIndex = 0;
            for (int i = 0; i < tagItems.Count; i++) {
                var obj = tagItems[i].gameObject;
                if (obj == eventData.pointerCurrentRaycast.gameObject) {
                    hit = obj;
                    hitIndex = i;
                    break;
                }
            }

            if (hit != null) {
                this.value += hitIndex;
            }
        }
        #endregion
    }
}