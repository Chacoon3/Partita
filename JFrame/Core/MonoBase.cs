using UnityEngine;

namespace JFrame.Core {
    public abstract class MonoBase : MonoBehaviour {

        /// <summary>
        /// 自身持有的消息
        /// </summary>
        protected Msg selfMsg {
            get {
                if (_msg == null) {
                    _msg = new Msg(this);
                }
                return _msg;
            }
        }

        private Msg _msg;

        public virtual void Execute(EventCode eventCode, Msg message) {

        }

    }
}