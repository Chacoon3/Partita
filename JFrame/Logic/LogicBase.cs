
using System.Collections;
using System.Collections.Generic;
using JFrame;
using JFrame.Core;

namespace JFrame.Logic {
    public abstract class LogicBase : MonoBase {
        /// <summary>
        /// 自身关心的消息集合
        /// </summary>
        private List<EventCode> list = new List<EventCode>();

        /// <summary>
        /// 绑定一个或多个消息
        /// </summary>
        /// <param name="eventCodes">Event codes.</param>
        protected void Bind(params EventCode[] eventCodes) {
            list.AddRange(eventCodes);

            LogicManager.Instance.Add(list.ToArray(), this);
        }

        /// <summary>
        /// 接触绑定的消息
        /// </summary>
        protected void UnBind() {
            if (LogicManager.Instance != null) {
                LogicManager.Instance.Remove(list.ToArray(), this);
                list.Clear();
            }
        }

        /// <summary>
        /// 自动移除绑定的消息
        /// </summary>
        public virtual void OnDestroy() {
            if (list != null)
                UnBind();
        }

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="areaCode">Area code.</param>
        /// <param name="eventCode">Event code.</param>
        /// <param name="message">Message.</param>
        public void Dispatch(AreaCode areaCode, EventCode eventCode, Msg message) {
            MsgCenter.Instance.Dispatch(areaCode, eventCode, message);
        }

        private void Awake() {
            MsgCenter.Instance.Register(this);
            StartCoroutine(Init());
        }

        private IEnumerator Init() {
            yield return StartCoroutine(SelfInit());
            MsgCenter.Instance.ReportInitialized(this);
        }

        /// <summary>
        /// 自身初始化
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator SelfInit();
    }
}