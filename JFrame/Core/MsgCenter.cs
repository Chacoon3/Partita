using System.Collections.Generic;
using JFrame.Logic;
using JFrame.Net;
using JFrame.UI;
using UnityEngine;

namespace JFrame.Core {
    public class MsgCenter : MonoBase {
        public static MsgCenter Instance = null;

        public readonly Dictionary<UIBase, bool> uiBases = new Dictionary<UIBase, bool>();
        public readonly Dictionary<LogicBase, bool> logicBases = new Dictionary<LogicBase, bool>();


        void Awake() {
            Instance = this;
            gameObject.AddComponent<UIManager>();
            gameObject.AddComponent<NetManager>();
            gameObject.AddComponent<LogicManager>();
            DontDestroyOnLoad(gameObject);
        }
        private void Start() {
            Application.targetFrameRate = 60;
        }

        private static object obj = new object();
        public void Dispatch(AreaCode areaCode, EventCode eventCode, Msg message) {
            lock (obj) {
                switch (areaCode) {
                    case AreaCode.Net://处理通讯的模块
                        NetManager.Instance.Execute(eventCode, message);
                        break;
                    case AreaCode.GAME://Game
                        GameManager.Instance.Execute(eventCode, message);
                        break;
                    case AreaCode.UI://UI
                        UIManager.Instance.Execute(eventCode, message);
                        break;
                    case AreaCode.LOGIC://主逻辑
                        LogicManager.Instance.Execute(eventCode, message);
                        break;
                    default:
                        break;
                }
            }
        }

        public void Register(UIBase ui) {
            if (!uiBases.ContainsKey(ui)) {
                uiBases.Add(ui, false);
            }
        }

        public void Register(LogicBase logic) {
            if (!logicBases.ContainsKey(logic)) {
                logicBases.Add(logic, false);
            }
        }

        public void ReportInitialized(UIBase ui) {
            if (uiBases.ContainsKey(ui)) {
                uiBases[ui] = true;
            }
        }

        public void ReportInitialized(LogicBase logic) {
            if (logicBases.ContainsKey(logic)) {
                logicBases[logic] = true;
            }
        }
    }
}