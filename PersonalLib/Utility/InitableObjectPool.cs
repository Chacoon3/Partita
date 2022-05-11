using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Partita.Utility {

    /// <summary>
    /// ����T�Ķ���أ�ί��ʵ������
    /// </summary>
    public class InitableObjectPool<T> : IEnumerable<KeyValuePair<T, bool>> {

        Dictionary<T, bool> pool;      //�洢�����Ѵ�����ʵ����boolΪTRUE��ʾitem����
        Func<T> creator;

        public InitableObjectPool(Func<T> creator) {
            pool = new Dictionary<T, bool>();
            this.creator = creator;
        }

        /// <summary>
        /// ��ȡ
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
        /// �ͷ�
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

        /// <returns>������д��ڼ���״̬��Ԫ��</returns>
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