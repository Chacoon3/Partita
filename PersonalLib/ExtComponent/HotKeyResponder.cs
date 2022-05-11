using System;
using System.Collections.Generic;

using UniRx;

using UnityEngine;

namespace Partita.ExtComponent {

    /// <summary>
    /// ʵ���ȼ��������Ӧ
    /// </summary>
    public class HotKeyResponder: MonoBehaviour {

        static List<HotKeyResponder> activeResponders;
        static bool isClassIntialized = false;

        /// <summary>
        /// �ö������Щ�ȼ�����Ӧ
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