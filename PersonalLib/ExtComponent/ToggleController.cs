/*-------------------------------------------------------------------------
 * 作者：张自正
 * 创建时间：2022/6/22 8:59:53
 * 本类主要用途描述：
 *------------------------------------------------------------------------*/
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// 管理一组toggle的主toggle
    /// </summary>
    /// <remarks>功能实现：子toggle全开/全关，根据子toggle状态维护自身显示</remarks>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Toggle))]
    public class ToggleController : MonoBehaviour {

        public bool isOn { get => selfToggle.isOn; set => selfToggle.isOn = value; }
        /// <summary>
        /// 全选时的sprite
        /// </summary>
        public Sprite checkMark;

        List<Toggle> subToggles = new List<Toggle>();
        Toggle selfToggle;
        Image contentImage;

        protected void Awake() {
            selfToggle = GetComponent<Toggle>();
            selfToggle.onValueChanged.AddListener(b => {
                for (int i = 0; i < subToggles.Count; i++) {
                    subToggles[i].SetIsOnWithoutNotify(b);
                }
                UpdateDisplay(b);
            });
            contentImage = transform.Find("Background/Checkmark").GetComponent<Image>();
        }

        public void RegisterToggle(params Toggle[] toggles) {
            for (int i = 0; i < toggles.Length; i++) {
                if (!subToggles.Contains(toggles[i])) {
                    subToggles.Add(toggles[i]);
                    subToggles[i].onValueChanged.AddListener(UpdateDisplay);
                }
            }
        }

        public void UnregisterToggle(params Toggle[] toggles) {
            for (int i = 0; i < toggles.Length; i++) {
                if (subToggles.Contains(toggles[i])) {
                    subToggles.Remove(toggles[i]);
                    subToggles[i].onValueChanged.RemoveListener(UpdateDisplay);
                }
            }
        }

        void UpdateDisplay(bool dummyVar) {
            bool isAllOn = true;
            bool isAllOff = true;
            foreach (var tog in subToggles) {
                if (!tog.gameObject.activeInHierarchy) {
                    continue;
                }
                isAllOn &= tog.isOn;
                isAllOff &= !tog.isOn;
            }

            if (isAllOn) {
                selfToggle.SetIsOnWithoutNotify(true);
                contentImage.sprite = checkMark;
                contentImage.color = Color.white;
            }
            else if (isAllOff) {
                selfToggle.SetIsOnWithoutNotify(false);
                contentImage.sprite = default;
                contentImage.color = default;
            }
            else {
                selfToggle.SetIsOnWithoutNotify(false);
                contentImage.sprite = default;
                contentImage.color = Color.green;
            }
        }
    }
}