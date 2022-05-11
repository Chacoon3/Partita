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
    /// 可旋转Image
    /// </summary>
    public class RotatableImage : Image {

        /// <summary>
        /// 自动旋转（设为TRUE时将以指定的每秒角速度持续旋转）
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
        /// 自动旋转的角度，正值为顺时针旋转，负值为逆时针旋转
        /// </summary>
        public float anglePerSec { get => _anglePerSec; set => _anglePerSec = value; }

        Vector2 objPivot;
        float defZAxisRotation;     //默认z轴角度

        float _anglePerSec;
        bool _autoRotate;
        bool _isManualRotating;

        protected override void Awake() {
            base.Awake();
            objPivot = rectTransform.pivot;
            defZAxisRotation = rectTransform.localRotation.z;
            autoRotate = false;
            _isManualRotating = false;

            Observable.EveryUpdate().       //每个update
                Where(_ => autoRotate).     //若开启了自动旋转
                Subscribe(_ => rectTransform.Rotate(0, 0, -anglePerSec * Time.deltaTime, Space.Self));
        }

        void ResetState() {
            autoRotate = false;
            rectTransform.DOKill();
        }

        /// <summary>
        /// 旋转图像
        /// </summary>
        /// <param name="angle">角度，正值为顺时针，负值为逆时针</param>
        /// <param name="timeInSec">时长</param>
        /// <param name="pivot">旋转轴心</param>
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
        /// 旋转图像
        /// </summary>
        /// <param name="angle">角度，正值为顺时针，负值为逆时针</param>
        /// <param name="timeInSec">时长</param>
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