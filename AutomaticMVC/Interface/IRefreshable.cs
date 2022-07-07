using AutomaticMVC.Business;
using System;

namespace AutomaticMVC.Interface {
    /// <summary>
    /// 可更新
    /// </summary>
    internal interface IRefreshable : IDisposable, ISubscriber<Model> {
        /// <summary>
        /// 初始化
        /// </summary>
        void OnInit(Model data);
        /// <summary>
        /// 更新
        /// </summary>
        new void Refresh(Model data);
        /// <summary>
        /// 销毁
        /// </summary>
        new void Dispose();
    }
}
