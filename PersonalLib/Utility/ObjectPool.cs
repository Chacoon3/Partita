/*-------------------------------------------------------------------------
 * 作者：Unity
 * 创建时间：2022/6/9 9:46:34
 * 本类主要用途描述：
 *------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Partita.Utility {

    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> {
        readonly Stack<T> m_Stack = new Stack<T>();
        readonly Action<T> m_ActionOnGet;
        readonly Action<T> m_ActionOnRelease;
        readonly Func<T> constructor;

        public int countAll { get; set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        public ObjectPool(Action<T> actionOnGet, Action<T> actionOnRelease, Func<T> constructor) {
            m_ActionOnGet = actionOnGet;
            m_ActionOnRelease = actionOnRelease;
            this.constructor = constructor;
        }

        public T Get() {
            T element;
            if (m_Stack.Count == 0) {
                element = constructor();
                countAll++;
            }
            else {
                element = m_Stack.Pop();
            }
            if (m_ActionOnGet != null)
                m_ActionOnGet(element);
            return element;
        }

        public void Release(T element) {
            if (m_Stack.Contains(element)) {
                return;
            }

            if (m_ActionOnRelease != null)
                m_ActionOnRelease(element);
            m_Stack.Push(element);
        }
    }
}