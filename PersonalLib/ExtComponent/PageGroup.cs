/*-------------------------------------------------------------------------
 * 作者：张自正
 * 创建时间：2022/6/9 16:33:7
 * 本类主要用途描述：
 *------------------------------------------------------------------------*/
using Partita.Utility;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// 页码组
    /// </summary>
    class PageGroup : MonoBehaviour {
        /// <summary>
        /// 是否隐藏不可用的按钮
        /// </summary>
        public bool hideDisabledPage {
            get => _hideDisabledPage;
            set {
                if (value == _hideDisabledPage) {
                    return;
                }
                _hideDisabledPage = value;
            }
        }
        /// <summary>
        /// 最小页码，限制为正整数
        /// </summary>
        public int minValue {
            get => _min;
            set {
                _min = Mathf.Clamp(value, 1, maxValue);
                extraInfoText.text = (maxValue - minValue + 1).ToString();
                UpdateDisplay();
            }
        }
        /// <summary>
        /// 最大页码
        /// </summary>
        public int maxValue {
            get => _max;
            set {
                _max = Mathf.Clamp(value, minValue, int.MaxValue);
                extraInfoText.text = (maxValue - minValue + 1).ToString();
                UpdateDisplay();
            }
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int value {
            get => _value;
            set {
                _value = Mathf.Clamp(value, minValue, maxValue);

                if (_value < _curentLeftIndex) {
                    _curentLeftIndex = _value;
                }
                else if (_value > _curentLeftIndex + indexBtnCount - 1) {
                    _curentLeftIndex = _value - indexBtnCount + 1;
                }

                UpdateDisplay();
                onValueChanged.Invoke(_value);
            }
        }
        public Text extraInfoText { get; private set; }

        /// <summary>
        /// 按钮高亮色
        /// </summary>
        public Color highlightColor;
        /// <summary>
        /// 按钮默认色
        /// </summary>
        public Color defaultColor;
        /// <summary>
        /// 默认字色
        /// </summary>
        public Color defaultTextColor;
        /// <summary>
        /// 页码改变事件
        /// </summary>
        public UnityEvent<int> onValueChanged = new UnityEvent<int>();

        Button leftArrowBtn, rightArrowBtn;
        Button[] indexBtns;
        InputField jumpPageInputfield;      //跳页输入框

        bool _hideDisabledPage = true;
        int _curentLeftIndex = 1;
        int _value = 1;
        int _min = 1;
        int _max = 99;

        public const int indexBtnCount = 9;

        void Awake() {
            indexBtns = transform.Find("pageContent").GetComponentsInChildren<Button>();
            extraInfoText = transform.Find("pageInfoCount/Text").GetComponent<Text>();
            ColorUtility.TryParseHtmlString("#0E3A5E", out highlightColor);
            ColorUtility.TryParseHtmlString("#C7DAE8", out defaultColor);
            ColorUtility.TryParseHtmlString("#656565", out defaultTextColor);

            for (int i = 0; i < indexBtns.Length; i++) {
                int btnIndex = i;
                indexBtns[btnIndex].onClick.AddListener(() => {
                    value = _curentLeftIndex + btnIndex;
                });
            }

            leftArrowBtn = transform.Find("leftBtn").GetComponent<Button>();
            leftArrowBtn.onClick.AddListener(() => {
                value--;
            });

            rightArrowBtn = transform.Find("rightBtn").GetComponent<Button>();
            rightArrowBtn.onClick.AddListener(() => {
                value++;
            });

            jumpPageInputfield = transform.FindByName<InputField>("InputField");
            jumpPageInputfield.characterValidation = InputField.CharacterValidation.Integer;
            jumpPageInputfield.onEndEdit.AddListener(s => {
                if (int.TryParse(s, out int index)) {
                    value = index;
                }
                else {
                    jumpPageInputfield.text = value.ToString();
                }
            });
        }

        void Start() {
            UpdateDisplay();
        }

        void UpdateDisplay() {
            jumpPageInputfield.SetTextWithoutNotify(value.ToString());
            extraInfoText.text = string.Format("共{0}页", (maxValue - minValue + 1).ToString());

            leftArrowBtn.interactable = value >= minValue;
            rightArrowBtn.interactable = value <= maxValue;

            int highlightIndex = value - _curentLeftIndex;          //得到高亮页码在页码按钮数组里的索引
            for (int i = 0; i < indexBtns.Length; i++) {
                int actualIndex = _curentLeftIndex + i;     //得到当前按钮实际显示的页码序号
                indexBtns[i].transform.GetChild(0).GetComponent<Text>().text = actualIndex.ToString();
                indexBtns[i].interactable = minValue <= actualIndex && actualIndex <= maxValue;

                if (i != highlightIndex) {
                    indexBtns[i].image.color = defaultColor;
                    indexBtns[i].GetComponentInChildren<Text>().color = defaultTextColor;
                }
                else {
                    indexBtns[i].image.color = highlightColor;
                    indexBtns[i].GetComponentInChildren<Text>().color = Color.white;
                }

                if (hideDisabledPage) {
                    indexBtns[i].gameObject.SetActive(indexBtns[i].interactable);
                }
            }
        }

        public void SetWithoutNotify(int index) {
            _value = Mathf.Clamp(index, minValue, maxValue);

            if (_value < _curentLeftIndex) {
                _curentLeftIndex = _value;
            }
            else if (_value > _curentLeftIndex + indexBtnCount - 1) {
                _curentLeftIndex = _value - indexBtnCount + 1;
            }

            UpdateDisplay();
        }

        public void SetPageMaxByItemCount(int itemCount, int pageCapacity) {
            if (itemCount % pageCapacity == 0) {
                maxValue = itemCount / pageCapacity;
            }
            else {
                maxValue = itemCount / pageCapacity + 1;
            }
        }
    }
}