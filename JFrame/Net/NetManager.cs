using JFrame.Core;

namespace JFrame.Net {
    public class NetManager : ManagerBase {
        public static NetManager Instance = null;

        void Awake() {
            Instance = this;
        }
    }
}