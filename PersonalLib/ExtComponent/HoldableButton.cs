/*
 * Title:
 *
 * Description: 
 *  	
 * Date: 2022/2/11
 */

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// �����Ͱ�ť
    /// </summary>
    /// <remarks>onClick�¼�ֻ�ڳ��� pressLength�� �󴥷�</remarks>
    public class HoldableButton: Button {

        public float pressLength {
            get => _pressLength;
            set {
                if (value == _pressLength) {
                    return;
                }
                if (value < 0 ) {
                    return;
                }

                _pressLength = value;
            }
        }

        float timeCounter;
        float _pressLength = 2;
        bool isPresed;

        void Update() {
            if (!isPresed) {
                return;
            }

            timeCounter += Time.deltaTime;
            if (timeCounter >= pressLength) {
                OnPointerUp(null);
                onClick.Invoke();
            }
        }

        public override void OnPointerClick(PointerEventData eventData) {
            return;
        }

        public override void OnPointerDown(PointerEventData eventData) {
            timeCounter = 0;
            isPresed = true;

            DoStateTransition(SelectionState.Pressed, false);
        }

        public override void OnPointerUp(PointerEventData eventData) {
            isPresed = false;
            DoStateTransition(SelectionState.Normal, false);
        }
    }
}