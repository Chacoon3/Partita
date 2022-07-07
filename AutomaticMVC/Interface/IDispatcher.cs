namespace AutomaticMVC.Interface {
    /// <summary>
    /// 发布者
    /// </summary>
    public interface IPublisher<T> {
        void Subscribe(ISubscriber<T> subscriber);
    }
}
