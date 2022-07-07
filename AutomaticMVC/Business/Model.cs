using AutomaticMVC.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutomaticMVC.Business {

    /// <summary>
    /// 数据层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Model<T> : Model, IEnumerable<KeyValuePair<string, T>>, IEnumerable where T : IEquatable<T> {

        public T this[string key] => valueDict[key];

        readonly Dictionary<string, T> valueDict;
        readonly Dictionary<string, List<View>> subscriberDict;

        public Model(string key, IEnumerable<KeyValuePair<string, T>> initializer) : base(key) {

            valueDict = new Dictionary<string, T>();
            foreach (var item in initializer) {
                valueDict.Add(item.Key, item.Value);
            }

            ModelCore.instance.Register(this);
        }

        #region 框架接口

        public override void Subscribe(View subscriber) => Subscribe(subscriber, string.Empty);

        public override void Subscribe(View subscriber, params string[] interests) {
            if (interests == null || interests.Length == 0) {
                Subscribe(subscriber, string.Empty);
                return;
            }

            foreach (var key in interests) {
                if (!subscriberDict.ContainsKey(key)) {
                    subscriberDict.Add(key, new List<View>());
                }
                if (!subscriberDict[key].Contains(subscriber)) {
                    subscriberDict[key].Add(subscriber);
                }
            }
        }

        public override void Unsubscribe(View subscriber, params string[] interests) {
            if (interests == null || interests.Length == 0) {
                Unsubscribe(subscriber, string.Empty);
                return;
            }

            foreach (var key in interests) {
                if (!subscriberDict.ContainsKey(key)) {
                    continue;
                }
                if (subscriberDict[key].Contains(subscriber)) {
                    subscriberDict[key].Remove(subscriber);
                }
            }
        }

        public override void Unsubscribe(View subscriber) => Unsubscribe(subscriber, string.Empty);

        public void SetValue(string key, T val) {
            if (valueDict[key].Equals(val))
                return;

            valueDict[key] = val;
            foreach (var item in subscriberDict[string.Empty])
                item.Refresh(this);

            foreach (var item in subscriberDict[key])
                item.Refresh(this);
        }

        #endregion

        #region C#接口
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<string, T>>)valueDict).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)valueDict).GetEnumerator();
        }
        #endregion
    }

    public abstract class Model {

        internal readonly string uid;
        protected Model(string uid) {
            this.uid = uid;
            ModelCore.instance.Register(this);
        }

        public abstract void Subscribe(View subscriber);

        public abstract void Subscribe(View subscriber, params string[] interests);

        public abstract void Unsubscribe(View subscriber, params string[] interests);

        public abstract void Unsubscribe(View subscriber);
    }

}
