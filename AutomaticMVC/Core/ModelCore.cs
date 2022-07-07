using AutomaticMVC.Business;
using System.Collections.Generic;

namespace AutomaticMVC.Core {
    /// <summary>
    /// 数据层核心
    /// </summary>
    internal class ModelCore {

        internal static ModelCore instance {
            get {
                if (_instance == null) {
                    _instance = new ModelCore();
                }
                return _instance;
            }
        }

        static ModelCore _instance;

        readonly Dictionary<string, Model> dataDict;

        private ModelCore() {
            dataDict = new Dictionary<string, Model>();
        }

        internal bool Register(Model data) {
            if (!dataDict.ContainsKey(data.uid)) {
                dataDict.Add(data.uid, data);
                return true;
            }
            return false;
        }

        internal bool Unregister(Model data) {
            return dataDict.Remove(data.uid);
        }

        internal void Subscribe(View subscriber, string key,  params string[] interests) {
            dataDict[key].Subscribe(subscriber, interests);
        }

        internal void Unsubscribe(View subscriber, string key,  params string[] interests) {
            dataDict[key].Unsubscribe(subscriber, interests);
        }
    }
}
