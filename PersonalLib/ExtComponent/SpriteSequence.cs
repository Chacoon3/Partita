using UnityEngine;
using UnityEngine.UI;

namespace Partita.ExtComponent {
    /// <summary>
    /// 序列帧
    /// </summary>
    public class SpriteSequence : Image {

        public Sprite defaultSprite { get; set; }
        public Sprite[] sequnce { get; set; }
        public float secPerFrame { get; set; } = .1f;
        public bool isPlaying { get; set; }

        int _current;
        float secCount;

        protected override void Awake() {
            base.Awake();

            defaultSprite = this.sprite;
        }

        private void Update() {
            DoAnimation();
        }

        void DoAnimation() {
            if (!isPlaying) {
                return;
            }

            secCount += Time.deltaTime;
            if (secCount >= secPerFrame) {
                secCount = 0;

                if (_current > sequnce.Length - 1) {
                    _current = 0;
                }

                if (_current < 0) {
                    _current = 0;
                }

                this.sprite = sequnce[_current];
                _current++;
            }
        }

        public void ToDefault() {
            this.sprite = defaultSprite;
            isPlaying = false;
            _current = 0;
        }
    }
}