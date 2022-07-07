using AutomaticMVC.Core;
using AutomaticMVC.Interface;

namespace AutomaticMVC.Business {
    /// <summary>
    /// 视图层
    /// </summary>
    public abstract class View : IRefreshable {
        public abstract void OnInit(Model data);
        public abstract void Refresh(Model data);
        public abstract void Dispose();
        protected void Subscribe(string key, params string[] interests) {
            foreach (var interest in interests) {
                ModelCore.instance.Subscribe(this, key, interest);
            }
        }
    }
}
