using System.Collections.Generic;
using JFrame.Core;

namespace JFrame.Game {
    public class GameBase : MonoBase {
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

            GameManager.Instance.Add(list.ToArray(), this);
        }

        /// <summary>
        /// 接触绑定的消息
        /// </summary>
        protected void UnBind() {
            if (GameManager.Instance != null) {
                GameManager.Instance.Remove(list.ToArray(), this);
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


    }
}