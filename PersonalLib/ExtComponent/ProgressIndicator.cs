using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// 悬浮进度条
    /// </summary>
    internal class ProgressIndicator : MonoBehaviour {

        public float progress => loadingBar.fillAmount;

        Image loadingBar;
        TMP_Text progressText;

        void Awake() {
            loadingBar = transform.Find("LoadingBar_img").GetComponent<Image>();
            progressText = transform.Find("ProgressText_tmptxt").GetComponent<TMP_Text>();

            Assert();
        }

        private void Assert() {
            if (loadingBar == null || progressText == null) {
                if (Debug.isDebugBuild) {
                    throw new NullReferenceException("组件获取失败");
                }
                else {
                    Debug.LogError("组件获取失败@" + gameObject);
                }
            }
        }

        internal void SetProgress01(float progress) {
            progress = Mathf.Clamp01(progress);
            loadingBar.fillAmount = progress;
        }

        internal void SetText(string text) {
            progressText.text = text;
        }

        internal void SetColor(Color color) {
            loadingBar.color = color;
        }
    }
}
