namespace AutomaticMVC.Core {
    /// <summary>
    /// 视图层核心
    /// </summary>
    internal class ViewCore {
        internal static ViewCore instance {
            get {
                if (_instance == null) {
                    _instance = new ViewCore();
                }
                return _instance;
            }
        }

        static ViewCore _instance;
    }
}
