using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Partita.Utility {

    /// <summary>
    /// 关于T的对象池（委托实例化）
    /// </summary>
    public class InitableObjectPool<T> : IEnumerable<KeyValuePair<T, bool>> {

        Dictionary<T, bool> pool;      //存储所有已创建的实例，bool为TRUE表示item闲置
        Func<T> creator;

        public InitableObjectPool(Func<T> creator) {
            pool = new Dictionary<T, bool>();
            this.creator = creator;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public T Get() {
            foreach (var item in pool) {
                if (item.Value) {

                    pool[item.Key] = false;
                    return item.Key;

                }
            }

            T t = creator();
            pool.Add(t, false);
            return t;
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="t"></param>
        public void Dispose(T t) {
            try {
                pool[t] = true;
            }
            catch (KeyNotFoundException) {
                pool.Add(t, true);
            }
        }

        /// <returns>对象池中处于激活状态的元素</returns>
        public T[] GetActiveElements() {
            return pool.Where(item => !item.Value).Select(item => item.Key).ToArray();
        }

        public IEnumerator<KeyValuePair<T, bool>> GetEnumerator() {
            return ((IEnumerable<KeyValuePair<T, bool>>)pool).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return ((IEnumerable)pool).GetEnumerator();
        }
    }
}