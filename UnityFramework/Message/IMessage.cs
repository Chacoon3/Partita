using ZFrame.ZFrameBehaviour;

namespace ZFrame.Message {
    internal interface IMessage {
        BehaviorBase sender { get; }
        object body { get; set; }
        bool boolean { get; set; }
        int integer { get;set; }
    }
}
