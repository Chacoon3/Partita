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
    public class TimeSelector : Button {

        enum TagType {
            Year, Month, Day, Hour, Minute, Second,
        }

        internal DateTime value {
            get => _value;
            set {
                if (value == _value) {
                    return;
                }
                if (value < min) {
                    value = min;
                }
                if (value > max) {
                    value = max;
                }

                _value = value;
                onValueChanged.Invoke(value);
            }
        }

        internal DateTime min {
            get => _min;
            set {
                if (value == _min) {
                    return;
                }
                if (value > max) {
                    value = max;
                }

                OnSetRange(value, false);
                _min = value;
            }
        }

        internal DateTime max {
            get => _max;
            set {
                if (value == _max) {
                    return;
                }
                if (value < min) {
                    value = min;
                }
                OnSetRange(value, true);
                _max = value;
            }
        }

        internal UnityEvent<DateTime> onValueChanged { get; } = new UnityEvent<DateTime>();

        Dictionary<TagType, ScrollableTag> tagDict=new Dictionary<TagType, ScrollableTag>();
        Dictionary<TagType, TMP_Text> textDict = new Dictionary<TagType, TMP_Text>();
        DateTime _value;
        DateTime _min = DateTime.MinValue;
        DateTime _max = DateTime.MaxValue;

        bool isDisplaying;

        protected override void Awake() {
            base.Awake();            
            Init();
        }

        protected override void OnEnable() {
            base.OnEnable();
            SetScrollableState();
        }

        protected override void OnDisable() {
            base.OnDisable();
            SetScrollableState();
        }

        void Init() {
            var scrollables = GetComponentsInChildren<ScrollableTag>();
            int tagType = 0;
            var tagProto = Resources.Load<GameObject>("DummyPrefab");
            foreach (var scrollable in scrollables) {
                TagType type = (TagType)tagType;
                Vector2Int range = default;
                switch (type) {
                    case TagType.Year:
                        range = new Vector2Int(1, 9999);
                        break;
                    case TagType.Month:
                        range = new Vector2Int(1, 12);
                        break;
                    case TagType.Day:
                        range = new Vector2Int(1, 31);
                        break;
                    case TagType.Hour:
                        range = new Vector2Int(0, 23);
                        break;
                    case TagType.Minute:
                        range = new Vector2Int(0, 59);
                        break;
                    case TagType.Second:
                        range = new Vector2Int(0, 59);
                        break;
                }
                scrollable.Init(5, 0, Mapper_Normal, Mapper_OutOfBound, tagProto, range);
                tagDict.Add(type, scrollable);

                tagType++;
            }

            var tmptexts = transform.Find("DisplayTexts").GetComponentsInChildren<TMP_Text>(true);
            for (int i = 0; i < tmptexts.Length; i++) {
                TagType type = (TagType)i;
                textDict.Add(type, tmptexts[i]);
            }

            foreach (var item in tagDict) {
                item.Value.onValueChanged.AddListener(v => OnScrollableValueChange(v, item.Key));
            }

            isDisplaying = false;
            SetScrollableState();
        }

        void SetScrollableState() {
            foreach (var item in tagDict) {
                item.Value.gameObject.SetActive(isDisplaying);
            }
        }

        void OnSetRange(DateTime value, bool isSettingMax) {
            if (isSettingMax) {
                var range = tagDict[TagType.Year].range;
                range.y = value.Year;
                tagDict[TagType.Year].range = range;

                range = tagDict[TagType.Month].range;
                range.y = value.Month;
                tagDict[TagType.Month].range = range;

                range = tagDict[TagType.Day].range;
                range.y = value.Day;
                tagDict[TagType.Day].range = range;

                range = tagDict[TagType.Hour].range;
                range.y = value.Hour;
                tagDict[TagType.Hour].range = range;

                range = tagDict[TagType.Minute].range;
                range.y = value.Minute;
                tagDict[TagType.Minute].range = range;

                range = tagDict[TagType.Second].range;
                range.y = value.Second;
                tagDict[TagType.Second].range = range;
            }
            else {
                var range = tagDict[TagType.Year].range;
                range.x = value.Year;
                tagDict[TagType.Year].range = range;

                range = tagDict[TagType.Month].range;
                range.x = value.Month;
                tagDict[TagType.Month].range = range;

                range = tagDict[TagType.Day].range;
                range.x = value.Day;
                tagDict[TagType.Day].range = range;

                range = tagDict[TagType.Hour].range;
                range.x = value.Hour;
                tagDict[TagType.Hour].range = range;

                range = tagDict[TagType.Minute].range;
                range.x = value.Minute;
                tagDict[TagType.Minute].range = range;

                range = tagDict[TagType.Second].range;
                range.x = value.Second;
                tagDict[TagType.Second].range = range;
            }
        }

        void OnScrollableValueChange(int val, TagType type) {
            DateTime newValue = GetCurrentValue();
            //clamp操作
            if (newValue < min) {
                newValue = min;
            }
            if (newValue > max) {
                newValue = max;
            }

            this.value = newValue;

            if (this.value.Month == 2) {
                if (DateTime.IsLeapYear( this.value.Year)) {        //2月
                    tagDict[TagType.Day].range = new Vector2Int(1, 29);     //闰年
                }
                else {
                    tagDict[TagType.Day].range = new Vector2Int(1, 28);     //不闰年
                }
            }
            else if (IsBigMonth(this.value.Month)) {        //大月
                tagDict[TagType.Day].range = new Vector2Int(1, 31);
            }
            else {      //小月
                tagDict[TagType.Day].range = new Vector2Int(1, 30);
            }

            tagDict[type].SetWithoutNotify(val);

            textDict[type].text = tagDict[type].value.ToString();
        }

        void Mapper_Normal(int state, GameObject obj) {
            obj.GetComponent<TMP_Text>().text = state.ToString();
        }

        void Mapper_OutOfBound(int state, GameObject obj) {
            obj.GetComponent<TMP_Text>().text = null;
        }

        DateTime GetCurrentValue() {
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

        public override void OnPointerClick(PointerEventData eventData) {
            base.OnPointerClick(eventData);
            isDisplaying = !isDisplaying;
            SetScrollableState();
        }
    }
}