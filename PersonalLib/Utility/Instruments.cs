using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Partita.Utility {
    /// <summary>
    /// ������
    /// </summary>
    public static partial class Instruments {

        /// <summary>
        /// ��UI���
        /// </summary>
        private class SimplePanel {
            public GameObject gameObject { get; private set; }
            public RectTransform transform { get; private set; }
            public Button sure, cancel, close;
            public TMP_Text content;

            public SimplePanel(GameObject gameObject) {
                this.gameObject = gameObject;
                transform = (RectTransform)gameObject.transform;
                content = gameObject.FindByName<TMP_Text>("Text (TMP)");
                sure = gameObject.FindByName<Button>("Button");
                cancel = gameObject.FindByName<Button>("Button (1)");
                close = gameObject.FindByName<Button>("Close_btn");
            }

            public void Reset() {
                content.text = null;
                if (sure != null) {
                    sure.onClick.RemoveAllListeners();
                    sure.onClick.AddListener(() => gameObject.SetActive(false));
                }

                if (cancel != null) {
                    cancel.onClick.RemoveAllListeners();
                    cancel.onClick.AddListener(() => gameObject.SetActive(false));
                }

                if (close != null) {
                    close.onClick.RemoveAllListeners();
                    close.onClick.AddListener(() => gameObject.SetActive(false));
                }
            }

            public void SetActive(bool active) {
                gameObject.SetActive(active);
            }
        }

        /// <summary>
        /// �������ֲ�������������
        /// </summary>
        /// <typeparam name="T">�����</typeparam>
        /// <param name="gameObject"></param>
        /// <param name="name">���������</param>
        /// <param name="includeInactive">�Ƿ����δ��������</param>
        /// <returns></returns>
        public static T FindByName<T>(this GameObject gameObject, string name, bool includeInactive = true) where T : Component {

            Component[] children = gameObject.GetComponentsInChildren<T>(includeInactive);
            for (int i = 0; i < children.Length; i++) {
                if (children[i].name == name) {
                    return (T)children[i];
                }
            }
            return null;
        }

        public static T FindByName<T>(this Component obj, string name, bool includeInactive = true) where T : Component {
            T[] children = obj.transform.GetComponentsInChildren<T>(includeInactive);
            for (int i = 0; i < children.Length; i++) {
                if (children[i].name == name) {
                    return children[i];
                }
            }
            return null;
        }

        /// <summary>
        /// ������Ԫ�ص����ֵ
        /// </summary>
        /// <typeparam name="T">һ��������ȫ���ϵ��Ԫ������</typeparam>
        /// <param name="first"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static T GetMax<T>(T first, params T[] ts) where T : IComparable<T> {
            T res = first;
            for (int i = 0; i < ts.Length; i++) {
                if (res.CompareTo(ts[i]) < 0) {
                    res = ts[i];
                }
            }
            return res;
        }

        /// <summary>
        /// ������Ԫ�ص���Сֵ
        /// </summary>
        /// <typeparam name="T">һ��������ȫ���ϵ��Ԫ������</typeparam>
        /// <param name="first"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static T GetMin<T>(T first, params T[] ts) where T : IComparable<T> {
            T res = first;
            for (int i = 0; i < ts.Length; i++) {
                if (res.CompareTo(ts[i]) > 0) {
                    res = ts[i];
                }
            }
            return res;
        }

        /// <summary>
        /// ���ݸ����ļӷ�������������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="adder"></param>
        /// <param name="zeroValue">����T����ֵ����Ϊ��͵���ʼ</param>
        /// <returns></returns>
        public static T SumBy<T>(this IEnumerable<T> container, T zeroValue, Func<T, T, T> adder) {
            T res = zeroValue;
            foreach (var element in container) {
                res = adder(res, element);
            }
            return res;
        }

        /// <summary>
        /// ƽ�Ƹ�ֵ���������ÿ��Ԫ�����ƻ�������һ���±�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="fromLeftToRight"></param>
        public static void Translation<T>(this IList<T> list, bool fromLeftToRight = true) {
            if (fromLeftToRight) {
                for (int i = list.Count - 1; i > 0; i--) {
                    list[i] = list[i - 1];
                }
            }
            else {
                for (int i = 0; i < list.Count - 1; i++) {
                    list[i] = list[i + 1];
                }
            }
        }

        public static void Translation<T>(this IList<T> list, T startVal, bool fromLeftToRight = true) {
            if (fromLeftToRight) {
                for (int i = list.Count - 1; i > 0; i--) {
                    list[i] = list[i - 1];
                }
                list[0] = startVal;
            }
            else {
                for (int i = 0; i < list.Count - 1; i++) {
                    list[i] = list[i + 1];
                }
                list[list.Count - 1] = startVal;
            }
        }

        /// <summary>
        /// ƽ�Ƹ�ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="fromLeftToRight"></param>
        public static void Translation<T>(this IList<T> list, int startInclusive, int endInclusive, bool fromLeftToRight = true) {
            if (fromLeftToRight) {
                for (int i = endInclusive; i >= startInclusive; i--) {
                    list[i] = list[i - 1];
                }
            }
            else {
                for (int i = startInclusive; i <= endInclusive; i++) {
                    list[i] = list[i + 1];
                }
            }
        }

        public static TOut[] Project<T, TOut>(IList<T> container1, IList<T> container2, IList<T> container3, Func<T, T, T, TOut> projectionMethod) {

            int minLength = GetMin(container1.Count, container2.Count, container3.Count);

            TOut[] res = new TOut[minLength];

            for (int i = 0; i < minLength; i++) {
                res[i] = projectionMethod(container1[i], container2[i], container3[i]);
            }

            return res;
        }

        public static TOut[] Project<T, TOut>(IList<T> container1, IList<T> container2, Func<T, T, TOut> projectionMethod) {

            int minLength = GetMin(container1.Count, container2.Count);

            TOut[] res = new TOut[minLength];

            for (int i = 0; i < minLength; i++) {
                res[i] = projectionMethod(container1[i], container2[i]);
            }

            return res;
        }

        public static TOut[] Project<T, TOut>(IList<T> container1, IList<T> container2, Func<T, T, TOut> projectionMethod, int fromInclusive, int endInclusive) {

            TOut[] res = new TOut[endInclusive - fromInclusive + 1];

            for (int i = fromInclusive; i <= endInclusive; i++) {
                res[i] = projectionMethod(container1[i], container2[i]);
            }

            return res;
        }

        /// <summary>
        /// �����ʽ�ĺ�
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="coefficients"></param>
        /// <returns></returns>
        public static float GetPolynomialSum(float variable, params float[] coefficients) {
            float res = 0;

            for (int i = 0; i < coefficients.Length; i++) {
                res += Mathf.Pow(variable, i) * coefficients[i];
            }

            return res;
        }

        /// <summary>
        /// �����ʽ�ĺ�
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="coefficients"></param>
        /// <returns></returns>
        public static T GetPolynomialSum<T>(T variable, Func<T, T, T> addition, Func<T, T, T> multiplication, Func<T, int, T> power, params T[] coefficients) {
            T res = default;

            for (int i = 0; i < coefficients.Length; i++) {
                res = addition(res, multiplication(power(variable, i), coefficients[i]));
            }
            return res;
        }

        /// <summary>
        /// �Ը����������
        /// </summary>
        /// <param name="floats"></param>
        /// <returns></returns>
        public static float GetSum(this IEnumerable<float> floats) {
            float res = 0;
            foreach (var num in floats) {
                res += num;
            }

            return res;
        }

        /// <summary>
        /// ��ָ���Ŀ�ʼ�������������
        /// </summary>
        /// <param name="floatList"></param>
        /// <param name="fromInclusive"></param>
        /// <param name="toInclusive"></param>
        /// <returns></returns>
        public static float GetSum(this IList<float> floatList, int fromInclusive, int toInclusive) {
            float res = 0;
            for (int i = fromInclusive; i <= toInclusive; i++) {
                res += floatList[i];
            }
            return res;
        }

        /// <param name="val"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns>�������������ڵķ�λ��</returns>
        public static float GetPercentage(this float val, float lower, float upper) {

            if (lower > upper) {
                throw new ArgumentException("���޲��ܴ�������");
            }

            return (Mathf.Clamp(val, lower, upper) - lower) / (upper - lower);
        }

        /// <param name="bound"></param>
        /// <returns>�������������ڵķ�λ��</returns>
        public static float GetPercentage(this float val, Vector2 bound) {

            if (bound.x > bound.y) {
                throw new ArgumentException("���޲��ܴ�������");
            }

            return (Mathf.Clamp(val, bound.x, bound.y) - bound.x) / (bound.y - bound.x);
        }

        public static float GetAverage(params float[] vals) {
            return vals.GetSum() / (float)vals.Length;
        }

        public static void BatchOperate<T>(this Action<T> operation, params T[] targets) {
            foreach (var tar in targets) {
                operation(tar);
            }
        }

        public static void SetValue<T>(this IList<T> container, T val) {
            for (int i = 0; i < container.Count; i++) {
                container[i] = val;
            }
        }

        public static float ToPercentage(this float val) => ToPercentage(val, 0, 100);

        /// <summary>
        /// ���ذٷ���
        /// </summary>
        /// <param name="val"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static float ToPercentage(this float val, float lower, float upper) {
            if (lower > upper) {
                return 0;
            }
            if (lower == upper) {
                return 100;
            }

            if (val < lower) {
                val = lower;
            }

            if (val > upper) {
                val = upper;
            }

            return (val - lower) / (upper - lower) * 100f;
        }

        /// <returns>��ת��Ϊʱ���ַ���</returns>
        public static string GetLogTime(int totalSec) {
            int hour = totalSec / 3600;
            int min = (totalSec / 60) % 60;
            int sec = totalSec % 60;
            return string.Format("{0}:{1}:{2}", hour.ToString("00"), min.ToString("00"), sec.ToString("00"));
        }

        /// <summary>
        /// �Ա�������Ϊ�����е㣬raduisΪ����뾶���Ӹ������ϵľ��ȷֲ����������
        /// </summary>
        /// <param name="num"></param>
        /// <param name="radius">����뾶</param>
        /// <returns></returns>
        public static float FromUniform(this float num, float radius) {
            radius = Mathf.Abs(radius);
            return UnityEngine.Random.Range(num - radius, num + radius);
        }

        /// <summary>
        /// �ð�ť�����滻��ť
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldBtn"></param>
        /// <returns></returns>
        public static T ReplaceButton<T>(this Button oldBtn) where T : Button {
            var obj = oldBtn.gameObject;

            var colorInfo = oldBtn.colors;
            var spriteInfo = oldBtn.spriteState;
            var tarImg = oldBtn.targetGraphic;
            var clickEvent = oldBtn.onClick;
            var transition = oldBtn.transition;
            Component.DestroyImmediate(oldBtn);

            var newBtn = obj.AddComponent<T>();
            newBtn.colors = colorInfo;
            newBtn.spriteState = spriteInfo;
            newBtn.targetGraphic = tarImg;
            newBtn.onClick = clickEvent;
            newBtn.transition = transition;
            return newBtn;
        }

        /// <summary>
        /// ��ͼ�������滻ͼ��
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="img"></param>
        /// <returns></returns>
        public static T ReplaceImage<T>(this Image img) where T : Image {
            var obj = img.gameObject;

            var sprite = img.sprite;
            var color = img.color;
            var mat = img.material;
            var raycast = img.raycastTarget;
            var padding = img.raycastPadding;
            var maskable = img.maskable;
            var imgType = img.type;

            Component.DestroyImmediate(img);
            var newImg = obj.AddComponent<T>();

            newImg.sprite = sprite;
            newImg.color = color;
            newImg.material = mat;
            newImg.raycastTarget = raycast;
            newImg.raycastPadding = padding;
            newImg.maskable = maskable;
            newImg.type = imgType;
            return newImg;
        }

        /// <summary>
        /// �����滻
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="replacer"></param>
        /// <returns></returns>
        public static GameObject ReplaceGameObject(this GameObject obj, GameObject replacer) {
            var parent = obj.transform.parent;
            var name = obj.name;
            var sibIndex = obj.transform.GetSiblingIndex();
            UnityEngine.Object.DestroyImmediate(obj);

            replacer.transform.SetParent(parent);
            replacer.transform.SetSiblingIndex(sibIndex);
            replacer.name = name;
            return replacer;
        }

        /// <summary>
        /// �ڸ��������ڽ�����������Ŀ��ֵ
        /// </summary>
        /// <param name="setter"></param>
        /// <param name="getter"></param>
        /// <param name="actionPerStep"></param>
        /// <param name="targetValue"></param>
        /// <param name="steps"></param>
        /// <param name="secPerStep"></param>
        /// <returns></returns>
        public static IEnumerator ConvergeBySteps(Action<float> setter, Func<float> getter, Action actionPerStep, float targetValue, int steps, float secPerStep = .2f) {
            if (steps <= 0) {
                setter(targetValue);
                yield break;
            }

            float initial = getter();
            WaitForSecondsRealtime waitor = new WaitForSecondsRealtime(secPerStep);
            int stepCount = 0;
            while (stepCount < steps) {
                yield return waitor;
                stepCount++;
                setter(Mathf.Lerp(initial, targetValue, stepCount / (float)steps));
                actionPerStep?.Invoke();
            }
        }

        /// <summary>
        /// ��ָ��ʱ���ڽ�����������ָ��ֵ
        /// </summary>
        /// <param name="setter"></param>
        /// <param name="getter"></param>
        /// <param name="actionPerStep"></param>
        /// <param name="targetGetter"></param>
        /// <param name="timespan"></param>
        /// <param name="timeGap"></param>
        /// <returns></returns>
        public static IEnumerator ConvergeByTime(Func<float> getter, Action<float> setter, Func<float> targetGetter, Action actionPerStep, float timespan, float timeGap = 0.2f) {
            if (getter() == targetGetter()) {
                yield break;
            }

            if (timespan <= 0) {
                timespan = .1f;
            }

            if (timeGap <= 0) {
                timeGap = 0.1f;
            }

            WaitForSecondsRealtime waitor = new WaitForSecondsRealtime(timeGap);
            float timeCount = 0;
            while (getter() != targetGetter()) {
                yield return waitor;
                timeCount += timeGap;
                setter(Mathf.Lerp(getter(), targetGetter(), timeCount / timespan));
                actionPerStep?.Invoke();
            }
        }

        /// <summary>
        /// �ڸ���ʱ����������Ŀ��ֵ
        /// </summary>
        /// <param name="getter"></param>
        /// <param name="setter"></param>
        /// <param name="target"></param>
        /// <param name="actionPerStep"></param>
        /// <param name="timespan"></param>
        /// <param name="timeGap"></param>
        /// <returns></returns>
        public static IEnumerator ConvergeByTime(Func<float> getter, Action<float> setter, float target, Action actionPerStep, float timespan, float timeGap = 0.2f) {
            if (getter() == target) {
                yield break;
            }

            if (timespan <= 0) {
                timespan = .1f;
            }

            if (timeGap <= 0) {
                timeGap = 0.1f;
            }

            WaitForSecondsRealtime waitor = new WaitForSecondsRealtime(timeGap);
            float timeCount = 0;
            while (getter() != target) {
                yield return waitor;
                timeCount += timeGap;
                setter(Mathf.Lerp(getter(), target, timeCount / timespan));
                actionPerStep?.Invoke();
            }
        }

        /// <summary>
        /// �������߼�������Ȼ��ö��һ�����ڵĸ��ڵ㣬��ÿ���ڵ�ִ����Ӧ����
        /// </summary>
        /// <param name="onEachStep"></param>
        /// <param name="shouldEnd"></param>
        /// <param name="stepCount"></param>
        /// <param name="stepTime"></param>
        /// <returns></returns>
        public static IEnumerator DoPeriodic(Action<int> onEachStep, Func<bool> shouldEnd, Action<int> onEnd, int stepCount, float stepTime) {
            if (stepTime <= 0) {
                stepTime = .1f;
            }
            WaitForSecondsRealtime waitor = new WaitForSecondsRealtime(stepTime);

            int current = 0;

            if (shouldEnd == null) {
                shouldEnd = () => false;
            }

            if (stepCount <= 0) {
                throw new ArgumentException();
            }

            while (!shouldEnd()) {
                onEachStep(current);
                current++;
                if (current >= stepCount) {
                    current = 0;
                }
                yield return waitor;
            }

            onEnd?.Invoke(current);
        }

        /// <summary>
        /// �������߼�������Ȼ��ö��һ�����ڵĸ��ڵ㣬��ÿ���ڵ�ִ����Ӧ����
        /// </summary>
        /// <param name="onEachStep"></param>
        /// <param name="shouldEnd"></param>
        /// <param name="stepCount"></param>
        /// <param name="stepTime"></param>
        /// <returns></returns>
        public static IEnumerator DoPeriodic(Action<int> onEachStep, Func<bool> shouldEnd, int stepCount, float stepTime) {
            if (stepTime <= 0) {
                stepTime = .1f;
            }
            WaitForSecondsRealtime waitor = new WaitForSecondsRealtime(stepTime);

            int current = 0;

            if (shouldEnd == null) {
                shouldEnd = () => false;
            }

            if (stepCount <= 0) {
                throw new ArgumentException();
            }

            while (!shouldEnd()) {
                onEachStep(current);
                current++;
                if (current >= stepCount) {
                    current = 0;
                }
                yield return waitor;
            }
        }

        public static IEnumerator DoPeriodicPermanent(Action<int> onEachStep, Func<bool> shouldHalt, Action<int> onHalt, int stepCount, float stepTime) {
            if (stepTime <= 0) {
                stepTime = .1f;
            }
            WaitForSecondsRealtime waitor = new WaitForSecondsRealtime(stepTime);

            int current = 0;

            if (shouldHalt == null) {
                shouldHalt = () => false;
            }

            if (stepCount <= 0) {
                throw new ArgumentException();
            }

            WaitUntil waitUtilShouldContinue = new WaitUntil(() => !shouldHalt());

            while (true) {

                if (shouldHalt()) {
                    onHalt(current);
                    yield return waitUtilShouldContinue;
                }
                else {
                    onEachStep(current);
                    current++;
                    yield return waitor;
                }

                if (current >= stepCount) {
                    current = 0;
                }
            }
        }

        /// <summary>
        /// ÿ֡����һ��������ֱ���������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getter"></param>
        /// <param name="endCondition"></param>
        /// <param name="acitonPerMonitor"></param>
        /// <param name="onEnd"></param>
        /// <returns></returns>
        public static IEnumerator MonitorPerFixFrame<T>(Func<T> getter, Func<bool> endCondition, Action<T> acitonPerMonitor, Action onEnd) {
            WaitForFixedUpdate waitor = new WaitForFixedUpdate();
            while (!endCondition()) {
                if (acitonPerMonitor != null) {
                    acitonPerMonitor(getter());
                }
                yield return waitor;
            }
            if (onEnd != null) {
                onEnd();
            }
        }

        /// <summary>
        /// ÿ�����һ��������ֱ���������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="getter"></param>
        /// <param name="endCondition"></param>
        /// <param name="acitonPerMonitor"></param>
        /// <param name="onEnd"></param>
        /// <returns></returns>
        public static IEnumerator MonitorPerSec<T>(Func<T> getter, Func<bool> endCondition, Action<T> acitonPerMonitor, Action onEnd, float sec = 0.2f) {
            WaitForSecondsRealtime waitor = new WaitForSecondsRealtime(sec);
            while (!endCondition()) {
                if (acitonPerMonitor != null) {
                    acitonPerMonitor(getter());
                }
                yield return waitor;
            }
            if (onEnd != null) {
                onEnd();
            }
            if (Debug.isDebugBuild) {
                Debug.Log("monitor complete");
            }
        }

        public static RectTransform AnchorTo(this RectTransform rectTransform, RectTransform parent, Vector2 pivot, Vector2 anchorMin, Vector2 anchorMax, Vector2 offset = default) {
            RectTransform oldParent = (RectTransform)rectTransform.parent;
            rectTransform.SetParent(parent);
            rectTransform.pivot = pivot;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.sizeDelta = offset;
            rectTransform.SetParent(oldParent);
            return rectTransform;
        }

        public static RectTransform AnchorTo(this RectTransform rectTransform, RectTransform parent, Vector2 pivot, Vector2 anchor, Vector2 offset = default) {
            RectTransform oldParent = (RectTransform)rectTransform.parent;
            rectTransform.SetParent(parent);
            rectTransform.pivot = pivot;
            rectTransform.anchorMin = anchor;
            rectTransform.anchorMax = anchor;
            rectTransform.localPosition = offset;
            rectTransform.SetParent(oldParent);
            return rectTransform;
        }

        public static bool IsSame<T>(ICollection<T> self, ICollection<T> another) {
            if (self.Count != another.Count) {
                return false;
            }
            Queue<T> queue = new Queue<T>(self);
            for (int i = 0; i < another.Count; i++) {
                var key = queue.Peek();
                if (another.Contains(key)) {
                    queue.Dequeue();
                }
                else {
                    return false;
                }
            }
            return queue.Count == 0;
        }
    }
}

