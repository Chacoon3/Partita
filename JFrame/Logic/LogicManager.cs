using JFrame.Core;

namespace JFrame.Logic {
    public class LogicManager : ManagerBase {
        public static LogicManager Instance = null;

        void Awake() {
            Instance = this;
        }
    }
}