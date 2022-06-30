/*-------------------------------------------------------------------------
 * 作者：张自正
 * 创建时间：2022/6/17 14:11:35
 * 本类主要用途描述：抽象数据类型  (Abstract data type)
 *------------------------------------------------------------------------*/
using System;

namespace Partita.ADT {

    #region 一般对象
    /// <summary>
    /// 二元序对
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
    /// 互斥序对（同一时刻至多只有一项有值）
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
    /// 节流阀
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>封装一个关于T的方法。避免在规定时间内对T的相同参数的重复调用</remarks>
    public class Throttle<T> {
        /// <summary>
        /// 最近一次调用节流阀的时间
        /// </summary>
        public DateTime recentInvokeTime { get; private set; } = DateTime.MinValue;
        /// <summary>
        /// 时限
        /// </summary>
        public TimeSpan timeWindow;

        T recentContent;        //最近一次方法调用用到的参数
        Action<T> action;      //定义封装的方法
        Func<T, T, bool> equalityChecker;      //定义如何判断该方法所用参数的相等性

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
        public TimeSpan timeWindow;     //时间限制

        Action action;      //定义封装的方法

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