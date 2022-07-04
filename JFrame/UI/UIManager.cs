using JFrame.Core;

namespace JFrame.UI {
    public class UIManager : ManagerBase {

        public static UIManager Instance = null;

        void Awake() {
            Instance = this;
        }
    }
}