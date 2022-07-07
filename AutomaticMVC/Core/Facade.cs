using UnityEngine;

namespace AutomaticMVC.Core {
    /// <summary>
    /// 对外界接口。外观模式
    /// </summary>
    public class Facade : MonoBehaviour {
        public static Facade instance {
            get {
                if (_gameObject == null) {
                    _gameObject = new GameObject("AutomaticMVC");
                }
                if (_instance == null) {
                    _instance = _gameObject.AddComponent<Facade>();
                }
                return _instance;
            }
        }

        static Facade _instance;
        static GameObject _gameObject;

        public void Init() {

        }

        void OnDestroy() {
            if (_gameObject != null) {
                Destroy(_gameObject);
            }
        }
    }
}
