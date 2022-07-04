using System.Collections.Generic;

namespace ZFrame.ZFrameBehaviour {
    /// <summary>
    /// 消息节点
    /// </summary>
    internal abstract class MsgNode : BehaviorBase {

        protected MsgNode parent { get; private set; }
        protected int layer { get; private set; }

        readonly List<MsgNode> childNodes = new List<MsgNode>();

        void Awake() {
            parent = ConfigParent();
            if (!parent.childNodes.Contains(this)) {
                parent.childNodes.Add(this);
            }

            if (parent.parent) {
                layer = parent.layer + 1;
            }
            else {
                layer = 0;
            }

            OnAwake();
        }

        /// <summary>
        /// Awake调用
        /// </summary>
        protected virtual void OnAwake() { }

        /// <summary>
        /// 配置自己的父节点
        /// </summary>
        /// <returns></returns>
        protected abstract MsgNode ConfigParent();

        /// <summary>
        /// 获取子节点
        /// </summary>
        protected MsgNode GetChildNode(int index) => childNodes[index];

        /// <summary>
        /// 获取子节点
        /// </summary>
        protected List<MsgNode> GetChildNodes(params int[] indexes) {
            var res = new List<MsgNode>();
            for (int i = 0; i < indexes.Length; i++) {
                res.Add(childNodes[i]);
            }
            return res;
        }

        /// <summary>
        /// 获取子节点
        /// </summary>
        protected List<MsgNode> GetChildNodes(IEnumerable<int> indexes) {
            var res = new List<MsgNode>();
            foreach (var index in indexes) {
                res.Add(childNodes[index]);
            }
            return res;
        }


    }
}
