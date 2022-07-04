namespace JFrame.Core {
    /// <summary>
    /// 消息类（模块间通信）
    /// </summary>
    public class Msg {
        public readonly MonoBase sender;
        public virtual object body { get; set; }
        public virtual int intMsg { get; set; }
        public virtual float floatMsg { get; set; }
        public virtual bool boolMsg { get; set; }

        public Msg(MonoBase sender) {
            this.sender = sender;
        }

        ~Msg() {
            body = null;
        }
    }
}