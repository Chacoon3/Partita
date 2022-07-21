/*-------------------------------------------------------------------------
 * 作者：张自正
 * 创建时间：2022/7/14 16:31:47
 * 本类主要用途描述：
 *------------------------------------------------------------------------*/

using Manager.Scripts.Data;

using UnityEngine;
using UnityEngine.UI;

namespace Manager.Scripts.Profession.UI {
    /// <summary>
    /// 鼠标点击效果
    /// </summary>
    class ClickEffect : MonoBehaviour {

        static ObjectPool<ClickEffect> pool;
        static Vector2 defaultSizeDelta = new Vector2(30, 30);
        static Vector2 endSizeDelta = new Vector2(130, 130);

        RectTransform selfRect;
        Image image;

        float lifeTime;
        float timeCounter;

        void Awake() {
            selfRect = (RectTransform)transform;
            image = GetComponent<Image>();
        }

        void Update() {
            timeCounter -= Time.deltaTime;
            var cor = image.color;
            float portion = timeCounter / lifeTime;
            cor.a = portion;
            image.color = cor;
            selfRect.sizeDelta = Vector2.Lerp(defaultSizeDelta, endSizeDelta, 1- portion);

            if (timeCounter <= 0) {
                pool.Release(this);
            }
        }

        //重置自身状态
        void SelfReset() {
            selfRect.anchoredPosition = Vector2.zero;
        }

        //依赖注入
        public static void Inject(GameObject protoObj, Sprite sprite) {
            pool = new ObjectPool<ClickEffect>(
                    pop => { pop.gameObject.SetActive(true); },
                    pop => { pop.gameObject.SetActive(false); pop.SelfReset(); },
                    () => {
                        var obj = Instantiate(protoObj).AddComponent<ClickEffect>();
                        var img = obj.GetComponent<Image>();
                        img.sprite = sprite;
                        img.raycastTarget = false;
                        obj.transform.SetParent(UIData.instance.canvas);
                        var rectTf = ((RectTransform)obj.transform);
                        rectTf.sizeDelta = defaultSizeDelta;
                        return obj;
                    }
                    );
        }

        public static ClickEffect Show(Vector2 pos, float time) {
            var res = pool.Get();
            res.selfRect.position = pos;
            res.lifeTime = time;
            res.timeCounter = time;
            return res;
        }

        public static ClickEffect Show(Vector2 pos, float time, Color color) {
            var res = pool.Get();
            res.selfRect.position = pos;
            res.lifeTime = time;
            res.timeCounter = time;
            color.a = 1;
            res.image.color = color;
            return res;
        }


        public static bool AnyActive() {
            return pool.countActive > 0;
        }
    }
}