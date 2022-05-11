using UnityEngine;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// 序列帧Text
    /// </summary>
    public class TextSequence : Text {

        public string defStr { get; set; }
        public string[] recycleStrs { get; set; }
        public bool isRecycling { get; set; }
        public float secPerStep { get; set; } = .15f;

        float timer;
        int _current;

        protected override void Awake() {
            base.Awake();
            this.defStr = text;
        }

        private void Update() {
            DoRecycle();
        }

        private void DoRecycle() {
            if (!isRecycling) {
                return;
            }
            if (recycleStrs == null || recycleStrs.Length == 0) {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= secPerStep) {
                timer = 0;

                if (_current > recycleStrs.Length - 1) {
                    _current = 0;
                }

                if (_current < 0) {
                    _current = 0;
                }

                this.text = recycleStrs[_current];
                _current++;
            }
        }
    }
}