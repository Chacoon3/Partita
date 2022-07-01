/*-------------------------------------------------------------------------
 * 作者：张自正
 * 创建时间：2022/6/9 16:33:7
 * 本类主要用途描述：
 *------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Partita.ExtComponent {

    /// <summary>
    /// 时间戮
    /// </summary>
    /// <remarks>类依赖：ScrollableTag</remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class TimeSelector : Selectable, IPointerClickHandler {

        enum TagType {
            Year, Month, Day, Hour, Minute, Second,
        }

        public DateTime value {
            get => _value;
            set {
                if (value == _value) {
                    return;
                }

                _value = DatetimeClamp(value);
                valueText.text = _value.ToString("F");
                SetWithoutNotify(_value);
                onValueChanged.Invoke(value);
            }
        }

        public DateTime min {
            get => _min;
            set {
                if (value == _min) {
                    return;
                }
                if (value > max) {
                    value = max;
                }

                tagDict[TagType.Year].min = value.Year;
                tagDict[TagType.Month].min = value.Month;
                tagDict[TagType.Day].min = value.Day;
                tagDict[TagType.Hour].min = value.Hour;
                tagDict[TagType.Minute].min = value.Minute;
                tagDict[TagType.Second].min = value.Second;
                _min = value;
            }
        }

        public DateTime max {
            get => _max;
            set {
                if (value == _max) {
                    return;
                }
                if (value < min) {
                    value = min;
                }

                tagDict[TagType.Year].max = value.Year;
                tagDict[TagType.Month].max = value.Month;
                tagDict[TagType.Day].max = value.Day;
                tagDict[TagType.Hour].max = value.Hour;
                tagDict[TagType.Minute].max = value.Minute;
                tagDict[TagType.Second].max = value.Second;
                _max = value;
            }
        }

        public UnityEvent<DateTime> onValueChanged { get; } = new UnityEvent<DateTime>();

        public bool isOpen {
            get => _isOpen;
            set {
                _isOpen = value;
                //foreach (var item in tagDict) {
                //    item.Value.gameObject.SetActive(value);
                //}
                tagParent.SetActive(_isOpen);
            }
        }

        Dictionary<TagType, ScrollableTag> tagDict = new Dictionary<TagType, ScrollableTag>();
        TMP_Text valueText;
        DateTime _value = DateTime.MinValue;
        DateTime _min = DateTime.MinValue;
        DateTime _max = DateTime.MaxValue;
        GameObject tagParent;

        bool _isOpen;

        #region 生命周期
        protected override void Awake() {
            base.Awake();
            #region 配置自身
            enabled = true;
            valueText = transform.Find("ValueText").GetComponent<TMP_Text>();
            #endregion

            #region 配置时间戮
            tagParent = transform.Find("Tags").gameObject;
            var scrollables = tagParent.GetComponentsInChildren<ScrollableTag>();
            int tagType = 0;
            foreach (var tag in scrollables) {
                TagType type = (TagType)tagType;
                int min = 0;
                int max = 0;
                switch (type) {
                    case TagType.Year:
                        min = 1;
                        max = 9999;
                        break;
                    case TagType.Month:
                        min = 1;
                        max = 12;
                        break;
                    case TagType.Day:
                        min = 1;
                        max = 31;
                        break;
                    case TagType.Hour:
                        min = 0;
                        max = 23;
                        break;
                    case TagType.Minute:
                    case TagType.Second:
                        min = 0;
                        max = 59;
                        break;
                }
                tag.min = min;
                tag.max = max;
                tagDict.Add(type, tag);
                tagType++;
            }

            foreach (var item in tagDict) {
                var key = item.Key;
                item.Value.onValueChanged.AddListener(v => OnScrollableValueChange(v, key));
            }
            #endregion

            isOpen = false;
            SetWithoutNotify(value);
        }

        protected override void Start() {
            base.Start();
            isOpen = false;
        }

        void Update() {
            if (gameObject.activeInHierarchy) {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
                    if (!RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, Input.mousePosition) && !RectTransformUtility.RectangleContainsScreenPoint((RectTransform)tagParent.transform, Input.mousePosition)) {
                        isOpen = false;
                    }
                }
            }
        }
        #endregion

        void OnScrollableValueChange(int val, TagType type) {
            DateTime newValue = GetValueFromTags();
            this.value = DatetimeClamp(newValue);

            if (this.value.Month == 2) {
                if (DateTime.IsLeapYear(this.value.Year)) {        //2月闰年
                    tagDict[TagType.Day].min = 1;
                    tagDict[TagType.Day].max = 29;
                }
                else {//2月不闰年
                    tagDict[TagType.Day].min = 1;
                    tagDict[TagType.Day].max = 28;
                }
            }
            else if (IsBigMonth(this.value.Month)) {        //大月
                tagDict[TagType.Day].min = 1;
                tagDict[TagType.Day].max = 31;
            }
            else {      //小月
                tagDict[TagType.Day].min = 1;
                tagDict[TagType.Day].max = 30;
            }

            tagDict[type].SetWithoutNotify(val);

            valueText.text = value.ToString("F");
        }

        DateTime GetValueFromTags() {
            //从子节点ScrollableTag获取到对应的Datetime
            int year = tagDict[TagType.Year].value;
            int month = tagDict[TagType.Month].value;
            int day = tagDict[TagType.Day].value;
            int hour = tagDict[TagType.Hour].value;
            int minute = tagDict[TagType.Minute].value;
            int second = tagDict[TagType.Second].value;

            if (month == 2) {
                if (DateTime.IsLeapYear(year)) {        //2月
                    day = Mathf.Clamp(day, 1, 29);  //闰年
                }
                else {
                    day = Mathf.Clamp(day, 1, 28);    //不闰年
                }
            }
            else if (IsBigMonth(month)) {        //大月
                day = Mathf.Clamp(day, 1, 31);
            }
            else {      //小月
                day = Mathf.Clamp(day, 1, 30);
            }
            return new DateTime(year, month, day, hour, minute, second);
        }

        bool IsBigMonth(int month) {
            return (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) {
                return;
            }

            if (eventData.pointerPressRaycast.gameObject == valueText.gameObject) {
                isOpen = !isOpen;
            }
        }

        DateTime DatetimeClamp(DateTime val) {
            if (min > max) {
                if (val > min) {
                    val = min;
                }
                if (val < max) {
                    val = max;
                }

                return val;
            }
            else {
                if (val < min) {
                    val = min;
                }
                if (val > max) {
                    val = max;
                }

                return val;
            }
        }

        public void SetWithoutNotify(DateTime value) {
            _value = DatetimeClamp(value);
            tagDict[TagType.Year].SetWithoutNotify(value.Year);
            tagDict[TagType.Month].SetWithoutNotify(value.Month);
            tagDict[TagType.Day].SetWithoutNotify(value.Day);
            tagDict[TagType.Hour].SetWithoutNotify(value.Hour);
            tagDict[TagType.Minute].SetWithoutNotify(value.Minute);
            tagDict[TagType.Second].SetWithoutNotify(value.Second);
        }

    }
}