using System.Collections.Generic;
using UnityEngine;

namespace JFrame.Core {

    /// <summary>
    /// 每个模块的基类
    ///  保存自身注册的一系列消息
    /// </summary>
    public class ManagerBase : MonoBase {
        private static object obj = new object();
        /// <summary>
        /// 处理自身的消息
        /// </summary>
        /// <param name="eventCode">Event code.</param>
        /// <param name="message">Message.</param>
        public override void Execute(EventCode eventCode, Msg message) {
            lock (obj) {
                if (!dict.ContainsKey(eventCode)) {
                    this.Print(string.Format("没有注册 ：{0} @ {1} ", eventCode, this), LogType.Warning);
                    return;
                }

                List<MonoBase> list = dict[eventCode];

                for (int i = 0; i < list.Count; i++) {
                    list[i].Execute(eventCode, message);
                }
            }
        }

        private Dictionary<EventCode, List<MonoBase>> dict = new Dictionary<EventCode, List<MonoBase>>();

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventCode">Event code.</param>
        /// <param name="mono">Mono.</param>
        public void Add(EventCode eventCode, MonoBase mono) {
            List<MonoBase> list = null;

            //之前没有注册过
            if (!dict.ContainsKey(eventCode)) {
                list = new List<MonoBase>();
                list.Add(mono);
                dict.Add(eventCode, list);
                return;
            }

            //之前注册过
            list = dict[eventCode];
            list.Add(mono);
        }

        /// <summary>
        /// 添加多个事件
        ///     一个脚本关心多个事件
        /// </summary>
        /// <param name="eventCode">Event code.</param>
        public void Add(EventCode[] eventCodes, MonoBase mono) {
            for (int i = 0; i < eventCodes.Length; i++) {
                Add(eventCodes[i], mono);
            }
        }


        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="eventCode">Event code.</param>
        /// <param name="mono">Mono.</param>
        public void Remove(EventCode eventCode, MonoBase mono) {
            //没注册过 没法移除 报个警告
            if (!dict.ContainsKey(eventCode)) {
                if (Debug.isDebugBuild) {
                    Debug.LogWarning("没有这个事件" + eventCode + "注册");
                }
                return;
            }

            List<MonoBase> list = dict[eventCode];

            if (list.Count == 1) {
                dict.Remove(eventCode);
            }
            else {
                list.Remove(mono);
            }
        }

        /// <summary>
        /// 移除多个
        /// </summary>
        /// <param name="eventCode">Event code.</param>
        /// <param name="mono">Mono.</param>
        public void Remove(EventCode[] eventCodes, MonoBase mono) {
            for (int i = 0; i < eventCodes.Length; i++) {
                Remove(eventCodes[i], mono);
            }
        }

    }
}