using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// �ɹ���ҳǩ
    /// </summary>
    /// <remarks>ÿ֡��⻬���¼�����������  �����߼��� int32ö��һ��״̬��mapperNormal��һ��״̬ӳ�䵽ҳǩ�ϣ� mapperOutOfBound����δ�����״̬</remarks>
    public class ScrollableTag : Image, IScrollHandler, IPointerClickHandler {

        /// <summary>
        /// ҳǩ�����ԭ��
        /// </summary>
        public GameObject tagProto {
            get => _tagProto;
            set {
                if (_tagProto == value) {
                    return;
                }
                _tagProto = value;
            }
        }

        /// <summary>
        /// ��ǰ״̬��������0��ҳǩ��״̬��
        /// </summary>
        public int value {
            get => _value;
            set {
                value = Mathf.Clamp(value, range.x, range.y);

                if (value == _value) {
                    return;
                }

                UpdateDisplay(value);
                _value = value;
                onValueChanged?.Invoke(value);
            }
        }
        /// <summary>
        /// ��ǩ����
        /// </summary>
        public int tagCount {
            get => _tagTransform.childCount;
            set {
                value = Mathf.Clamp(value, 0, int.MaxValue);
                if (value == _tagCount) {
                    return;
                }

                OnTagCountChange(value);
                _tagCount = value;
            }
        }
        public UnityEvent<int> onValueChanged { get; } = new UnityEvent<int>();
        public Action<int, GameObject> mapper = delegate { };
        public Action<int, GameObject> outBoundMapper = delegate { };
        /// <summary>
        /// ״̬�����½磨����״̬����������Ȼ�����ɽ���˫�䣩
        /// </summary>
        public Vector2Int range {
            get=> _range;
            set {
                if (_range == value) {
                    return;
                }

                if (value.x > value.y) {
                    value.x = value.y;
                }                
                _range = value;
                _value = Mathf.Clamp(_value, range.x, range.y);
                UpdateDisplay(_value);
            }
        }

        Transform _tagTransform;
        GameObject _tagProto;

        int _tagCount;
        int _value = 0;
        Vector2Int _range = new Vector2Int(int.MinValue, int.MaxValue);

        #region ��������
        protected override void OnDestroy() {
            base.OnDestroy();
            value = 0;
            tagCount = 0;
            tagProto = null;
            mapper = delegate { };
            onValueChanged.RemoveAllListeners();
        }

        protected override void Reset() {
            base.Reset();
            value = 0;
            tagCount = 0;
            tagProto = null;
            mapper = delegate { };
            onValueChanged.RemoveAllListeners();
        }
        #endregion

        #region ����ӿ�
        /// <summary>
        /// �����ʼ��
        /// </summary>
        /// <param name="tagCount">��ǩ����</param>
        /// <param name="initiaState">��ֵ</param>
        /// <param name="mapper">ӳ�����</param>
        /// <param name="proto">��ǩ����ԭ��</param>
        public virtual void Init(int tagCount, int initiaState, Action<int, GameObject> mapper, Action<int, GameObject> outOfBoundMapper, GameObject proto, Vector2Int range) {
            _tagTransform = transform.Find("Tags");
            DestroyTags();

            this.mapper = mapper;
            this.outBoundMapper = outOfBoundMapper;
            this.tagProto = proto;
            this.tagCount = tagCount;
            this.value = initiaState;
            this.range = range;
        }

        public virtual void SetWithoutNotify(int value) {
            _value = value;
            UpdateDisplay(value);
        }
        #endregion

        protected virtual void UpdateDisplay(int newState) {
            //������ʾ
            for (int i = 0; i < tagCount; i++) {
                if (newState < range.x || newState > range.y) {
                    outBoundMapper(newState, _tagTransform.GetChild(i).gameObject);
                }
                else {
                    mapper(newState, _tagTransform.GetChild(i).gameObject);
                }
                newState++;
            }
        }

        protected virtual void OnTagCountChange(int newTagCount) {
            int differenceAbs = Mathf.Abs(newTagCount - tagCount);
            if (newTagCount > tagCount) {
                for (int i = 0; i < differenceAbs; i++) {
                    Instantiate(tagProto, _tagTransform);
                }
            }
            else {
                DestroyTags(differenceAbs);
            }

            UpdateDisplay(value);
        }

        protected virtual void DestroyTags(int count) {
            count = Mathf.Clamp(count, 0, _tagTransform.childCount);
            for (int i = 0; i < count; i++) {
                Destroy(_tagTransform.GetChild(0).gameObject);
            }
        }

        protected virtual void DestroyTags() {
            int childCount = _tagTransform.childCount;
            for (int i = 0; i < childCount; i++) {
                Destroy(_tagTransform.GetChild(0).gameObject);
            }
        }

        #region Unity �ӿ�
        public virtual void OnScroll(PointerEventData eventData) {
            if (eventData.scrollDelta.y < 0) {      //������Ϊ������
                value++;
            }
            else {
                value--;
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData) {
            GameObject hit = null;
            int hitIndex = 0;
            for (int i = 0; i < _tagTransform.childCount; i++) {
                var obj = _tagTransform.GetChild(i).gameObject;
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