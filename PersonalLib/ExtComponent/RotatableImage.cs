/*
 * Title:
 *
 * Description: 
 *  	
 * Date: 2022/2/11
 */

using DG.Tweening;

using UniRx;

using UnityEngine;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// ����תImage
    /// </summary>
    public class RotatableImage : Image {

        /// <summary>
        /// �Զ���ת����ΪTRUEʱ����ָ����ÿ����ٶȳ�����ת��
        /// </summary>
        public bool autoRotate {
            get => _autoRotate; 
            set {
                if (value == _autoRotate) {
                    return;
                }

                if (_isManualRotating) {
                    rectTransform.DOKill();
                }
                _autoRotate = value;
            }
        }

        /// <summary>
        /// �Զ���ת�ĽǶȣ���ֵΪ˳ʱ����ת����ֵΪ��ʱ����ת
        /// </summary>
        public float anglePerSec { get => _anglePerSec; set => _anglePerSec = value; }

        Vector2 objPivot;
        float defZAxisRotation;     //Ĭ��z��Ƕ�

        float _anglePerSec;
        bool _autoRotate;
        bool _isManualRotating;

        protected override void Awake() {
            base.Awake();
            objPivot = rectTransform.pivot;
            defZAxisRotation = rectTransform.localRotation.z;
            autoRotate = false;
            _isManualRotating = false;

            Observable.EveryUpdate().       //ÿ��update
                Where(_ => autoRotate).     //���������Զ���ת
                Subscribe(_ => rectTransform.Rotate(0, 0, -anglePerSec * Time.deltaTime, Space.Self));
        }

        void ResetState() {
            autoRotate = false;
            rectTransform.DOKill();
        }

        /// <summary>
        /// ��תͼ��
        /// </summary>
        /// <param name="angle">�Ƕȣ���ֵΪ˳ʱ�룬��ֵΪ��ʱ��</param>
        /// <param name="timeInSec">ʱ��</param>
        /// <param name="pivot">��ת����</param>
        public void Rotate(int angle, float timeInSec, Vector2 pivot) {
            ResetState();

            _isManualRotating = true;
            rectTransform.pivot = pivot;
            rectTransform.DORotate(new Vector3(0, 0, -angle), timeInSec, RotateMode.LocalAxisAdd)
                .onComplete += () => {
                    rectTransform.pivot = objPivot;
                    _isManualRotating = false;
                };
        }

        /// <summary>
        /// ��תͼ��
        /// </summary>
        /// <param name="angle">�Ƕȣ���ֵΪ˳ʱ�룬��ֵΪ��ʱ��</param>
        /// <param name="timeInSec">ʱ��</param>
        public void Rotate(int angle, float timeInSec) {
            ResetState();

            _isManualRotating = true;
            rectTransform.DORotate(new Vector3(0, 0, -angle), timeInSec, RotateMode.LocalAxisAdd)
                .onComplete += () => {
                    _isManualRotating = false;
                };
        }
    }
}