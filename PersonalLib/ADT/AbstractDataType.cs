/*-------------------------------------------------------------------------
 * Author：Chacoon3
 * Date：2022/6/17 14:11:35
 * Description：Abstract data type
 *------------------------------------------------------------------------*/
using System;

namespace Partita.ADT {

    #region general objects
    /// <summary>
    /// binary tuple
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public struct Pair<T1, T2> {
        public T1 item1 { get; set; }
        public T2 item2 { get; set; }
        public Pair(T1 t1, T2 t2) {
            item1 = t1;
            item2 = t2;
        }
    }

    /// <summary>
    /// xor binary tuple（at most one item contains non-null value at any given time）
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public struct XOrPair<T1, T2> {
        public T1 item1 {
            get => _item1;
            set {
                _item1 = value;
                _item2 = default;
            }
        }
        public T2 item2 {
            get => _item2;
            set {
                _item1 = default;
                _item2 = value;
            }
        }

        T1 _item1;
        T2 _item2;

        public XOrPair(T1 t1) {
            _item1 = t1;
            _item2 = default;
        }

        public XOrPair(T2 t2) {
            _item1 = default;
            _item2 = t2;
        }
    }

    /// <summary>
    /// Throttle
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>encapsulate a method of T. Precent invocation of the method with the same parameters within certain period</remarks>
    public class Throttle<T> {
        /// <summary>
        /// the most recent time when the method is invoked
        /// </summary>
        public DateTime recentInvokeTime { get; private set; } = DateTime.MinValue;
        /// <summary>
        /// time limit
        /// </summary>
        public TimeSpan timeWindow;

        T recentContent;        //the parameter used in last invocation
        Action<T> action;      //method being encapsulated
        Func<T, T, bool> equalityChecker;      //delegate which gives the definition of identity of the parameter T

        public Throttle(Action<T> action, Func<T, T, bool> equalityChecker) {
            this.action = action;
            this.equalityChecker = equalityChecker;
        }

        ~Throttle() {
            action = null;
            equalityChecker = null;
        }

        public bool TryInvoke(T content) {
            if (equalityChecker != null && equalityChecker(recentContent, content)) {

                if (DateTime.Now - recentInvokeTime < timeWindow) {
                    return false;
                }
            }

            recentInvokeTime = DateTime.Now;
            recentContent = content;
            action?.Invoke(content);
            return true;
        }
    }

    public class Throttle {
        public DateTime recentInvokeTime { get; private set; } = DateTime.MinValue;
        public TimeSpan timeWindow;     //time limit 

        Action action;      //method being encapsulated

        public Throttle(Action action) {
            this.action = action;
        }

        ~Throttle() {
            action = null;
        }

        public void Execute() {
            if (DateTime.Now - recentInvokeTime < timeWindow) {
                return;
            }

            recentInvokeTime = DateTime.Now;
            action?.Invoke();
        }
    }
    #endregion
}
