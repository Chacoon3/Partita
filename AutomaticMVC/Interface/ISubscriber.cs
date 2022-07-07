namespace AutomaticMVC.Interface {
    /// <summary>
    /// 订阅者
    /// </summary>
    public interface ISubscriber<T> {
        void Refresh(T message);
    }
}
