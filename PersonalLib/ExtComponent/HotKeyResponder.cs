using System;
using System.Collections.Generic;

using UniRx;

using UnityEngine;

namespace Partita.ExtComponent {

    /// <summary>
    /// 实现热键输入的响应
    /// </summary>
    public class HotKeyResponder: MonoBehaviour {

        static List<HotKeyResponder> activeResponders;
        static bool isClassIntialized = false;

        /// <summary>
        /// 该对象对哪些热键做反应
        /// </summary>
        public List<KeyCode> respondKeys { get; private set; }

        public Action<KeyCode> onHotkey;

        bool isStarted;

        void Awake() {

            respondKeys = new List<KeyCode>();

            if (!isClassIntialized) {
                isClassIntialized = true;
                Observable.EveryUpdate().Where(_ => activeResponders.Count > 0).Subscribe(OnEveryUpdate).AddTo(this);
                activeResponders = new List<HotKeyResponder>();
            }
        }

        void Start() {
            isStarted = true;
        }

        void OnEnable() {

            if (!activeResponders.Contains(this)) {
                activeResponders.Add(this);
            }
        }

        void OnDisable() {
            if (!isStarted) {
                return;
            }
            if (activeResponders.Contains(this)) {
                activeResponders.Remove(this);
            }
        }

        void OnEveryUpdate(long _) {
            if (!Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1)) {
                return;
            }

            if (activeResponders.Count == 0) {
                return;
            }

            var responder = activeResponders[activeResponders.Count - 1];

            foreach (var respondKey in responder.respondKeys) {
                if (Input.GetKeyDown(respondKey)) {
                    responder.onHotkey?.Invoke(respondKey);
                }
            }
        }

        internal static void Unload() {
            activeResponders = null;
            isClassIntialized = false;
        }
    }
}