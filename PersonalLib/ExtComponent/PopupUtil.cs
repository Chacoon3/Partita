/*-------------------------------------------------------------------------
 * 作者：张自正
 * 创建时间：2022/6/9 11:36:25
 * 本类主要用途描述：
 *------------------------------------------------------------------------*/

using Manager.Scripts.Data;

using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Manager.Scripts.Profession.UI {
    /// <summary>
    /// 弹窗功能
    /// </summary>
    public static class PopupUtil {

        #region 辅助类
        /// <summary>
        /// 简单UI窗体
        /// </summary>
        private class SimpleWindow {
            public GameObject gameObject { get; private set; }
            public RectTransform transform { get; private set; }
            public Button[] btns;
            public TMP_Text[] texts;
            public InputField[] inputFields;

            public SimpleWindow(GameObject gameObject) {
                this.gameObject = gameObject;
                transform = (RectTransform)gameObject.transform;
                var panel = transform.Find("Panel");
                var tmpTextParent = panel.Find("TmpTexts");
                var btnParent = panel.Find("Btns");
                var inputfieldParent = panel.Find("Inputfields");
                if (tmpTextParent) {
                    texts = gameObject.transform.Find("Panel/TmpTexts").GetComponentsInChildren<TMP_Text>();
                }
                if (btnParent) {
                    btns = gameObject.transform.Find("Panel/Btns").GetComponentsInChildren<Button>();
                }
                if (inputfieldParent) {
                    inputFields = gameObject.transform.Find("Panel/Inputfields").GetComponentsInChildren<InputField>();
                }
            }

            public virtual void Reset() {
                foreach (var t in texts) {
                    t.text = null;
                }

                for (int i = 0; i < btns.Length; i++) {
                    btns[i].onClick.RemoveAllListeners();
                    btns[i].onClick.AddListener(() => gameObject.SetActive(false));
                }
            }

            public void SetActive(bool active) {
                gameObject.SetActive(active);
            }
        }

        /// <summary>
        /// 文件选择型弹窗
        /// </summary>
        private class FileSelectWindow : SimpleWindow {
            public FileSelectWindow(GameObject gameObject) : base(gameObject) {
            }

            public override void Reset() {
                foreach (var t in texts) {
                    t.text = null;
                }

                for (int i = 0; i < btns.Length; i++) {
                    btns[i].onClick.RemoveAllListeners();
                    if (i != 3) {    //3号按钮是路径选择按钮
                        btns[i].onClick.AddListener(() => gameObject.SetActive(false));
                    }
                }
            }
        }
        #endregion

        static bool isInit;
        //双按钮对话框，单按钮对话框
        static SimpleWindow dialog, monolog;
        //文件路径选择弹窗，
        static FileSelectWindow filePopType1, filePopType2;

        public static void Init(AssetBundle ab) {
            if (isInit) {
                return;
            }
            isInit = true;

            var parent = UIData.instance.canvas.Find("Scalar");

            var dialogObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("Dialog"), parent);
            var monologObj = GameObject.Instantiate(ab.LoadAsset<GameObject>("Monolog"), parent);
            var filePopObjType1 = GameObject.Instantiate(ab.LoadAsset<GameObject>("FilePopType1"), parent);
            var filePopObjType2 = GameObject.Instantiate(ab.LoadAsset<GameObject>("FilePopType2"), parent);

            dialog = new SimpleWindow(dialogObj);
            monolog = new SimpleWindow(monologObj);
            filePopType1 = new FileSelectWindow(filePopObjType1);
            filePopType2 = new FileSelectWindow(filePopObjType2);

            dialog.SetActive(false);
            monolog.SetActive(false);
            filePopType1.SetActive(false);
            filePopType2.SetActive(false);
        }

        //静态字段数据清除
        public static void Unload() {

            var self = typeof(PopupUtil);
            var staticData = self.GetFields(System.Reflection.BindingFlags.Static);
            foreach (var field in staticData) {
                field.SetValue(self, default);
            }
        }

        /// <summary>
        /// 只含路径输入域的上传/下载弹窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subtitle1"></param>
        /// <param name="onConfirmFilePath"></param>
        /// <param name="onCancel"></param>
        public static void ShowFilePop_Type1(string title, string subtitle1, UnityAction<string> onConfirmFilePath, UnityAction onCancel = null) {
            filePopType1.Reset();
            filePopType1.texts[0].text = title;
            filePopType1.texts[1].text = subtitle1;
            foreach (var field in filePopType1.inputFields) {
                field.SetTextWithoutNotify(null);
            }
            filePopType1.inputFields[1].gameObject.SetActive(false);

            FolderBrowserHelper.SelectFile(s => {
                filePopType1.inputFields[0].SetTextWithoutNotify(s);
            });

            filePopType1.btns[0].onClick.AddListener(() => onConfirmFilePath?.Invoke(filePopType1.inputFields[0].text));
            if (onCancel != null) {
                filePopType1.btns[1].onClick.AddListener(onCancel);
            }
            filePopType1.btns[3].onClick.AddListener(() => {
                FolderBrowserHelper.SelectFile(s => {
                    filePopType1.inputFields[0].SetTextWithoutNotify(s);
                });
            });

            filePopType1.transform.SetAsLastSibling();
            filePopType1.SetActive(true);
        }

        /// <summary>
        /// 只含路径输入域的上传/下载弹窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subtitle1"></param>
        /// <param name="onConfirmFilePath"></param>
        /// <param name="onCancel"></param>
        public static void ShowFilePop_Type1(string title, string subtitle1, string placeholder1, UnityAction<string> onConfirmFilePath, UnityAction onCancel = null) {
            filePopType1.Reset();
            filePopType1.texts[0].text = title;
            filePopType1.texts[1].text = subtitle1;
            filePopType1.inputFields[0].SetTextWithoutNotify(null);
            ((Text)filePopType1.inputFields[0].placeholder).text = placeholder1;

            foreach (var field in filePopType1.inputFields) {
                field.SetTextWithoutNotify(null);
            }
            filePopType1.inputFields[1].gameObject.SetActive(false);

            filePopType1.btns[0].onClick.AddListener(() => onConfirmFilePath?.Invoke(filePopType1.inputFields[0].text));
            if (onCancel != null) {
                filePopType1.btns[1].onClick.AddListener(onCancel);
            }
            filePopType1.btns[3].onClick.AddListener(() => {
                FolderBrowserHelper.SelectFile(s => {
                    filePopType1.inputFields[0].SetTextWithoutNotify(s);
                });
            });

            filePopType1.transform.SetAsLastSibling();
            filePopType1.SetActive(true);
        }

        /// <summary>
        /// 只含路径输入域的上传/下载弹窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subtitle1"></param>
        /// <param name="onConfirmFilePath"></param>
        /// <param name="onCancel"></param>
        public static void ShowFilePop_Type1(string title, string subtitle1, string placeholder1, string placeholder2, UnityAction<string> onConfirmFilePath, UnityAction onCancel = null) {
            filePopType1.Reset();
            filePopType1.texts[0].text = title;
            filePopType1.texts[1].text = subtitle1;
            filePopType1.inputFields[0].SetTextWithoutNotify(null);
            ((Text)filePopType1.inputFields[0].placeholder).text = placeholder1;
            filePopType1.inputFields[1].SetTextWithoutNotify(null);
            ((Text)filePopType1.inputFields[1].placeholder).text = placeholder2;

            foreach (var field in filePopType1.inputFields) {
                field.SetTextWithoutNotify(null);
            }
            filePopType1.inputFields[1].gameObject.SetActive(false);

            FolderBrowserHelper.SelectFile(s => {
                filePopType1.inputFields[0].SetTextWithoutNotify(s);
            });

            filePopType1.btns[0].onClick.AddListener(() => onConfirmFilePath?.Invoke(filePopType1.inputFields[0].text));
            if (onCancel != null) {
                filePopType1.btns[1].onClick.AddListener(onCancel);
            }
            filePopType1.btns[3].onClick.AddListener(() => {
                FolderBrowserHelper.SelectFile(s => {
                    filePopType1.inputFields[0].SetTextWithoutNotify(s);
                });
            });

            filePopType1.transform.SetAsLastSibling();
            filePopType1.SetActive(true);
        }

        /// <summary>
        /// 含路径输入域和名称输入域的上传/下载弹窗
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subtitle1"></param>
        /// <param name="subtitle2"></param>
        /// <param name="onConfirmFilePath"></param>
        /// <param name="onCancel"></param>
        public static void ShowFilePop_Type2(string title, string subtitle1, string subtitle2, UnityAction<string, string> onConfirmFilePath, UnityAction onCancel = null) {
            filePopType2.Reset();
            filePopType2.texts[0].text = title;
            filePopType2.texts[1].text = subtitle1;
            filePopType2.texts[2].text = subtitle2;
            foreach (var field in filePopType2.inputFields) {
                field.SetTextWithoutNotify(null);
            }
            filePopType2.inputFields[1].gameObject.SetActive(true);

            FolderBrowserHelper.SelectFile(s => {
                filePopType2.inputFields[0].SetTextWithoutNotify(s);
            });

            filePopType2.btns[0].onClick.AddListener(() => onConfirmFilePath?.Invoke(filePopType2.inputFields[0].text, filePopType2.inputFields[1].text));
            if (onCancel != null) {
                filePopType2.btns[1].onClick.AddListener(onCancel);
            }
            filePopType2.btns[3].onClick.AddListener(() => {
                FolderBrowserHelper.SelectFile(s => {
                    filePopType2.inputFields[0].SetTextWithoutNotify(s);
                });
            });

            filePopType2.transform.SetAsLastSibling();
            filePopType2.SetActive(true);
        }

        /// <summary>
        /// 显示单选对话框
        /// </summary>
        public static void ShowMonolog(string title, string content, UnityAction onSure = null, UnityAction onClose = null) {
            monolog.Reset();

            monolog.texts[0].text = title;
            monolog.texts[1].text = content;

            if (onSure != null) {
                monolog.btns[0].onClick.AddListener(onSure);
            }
            if (onClose != null) {
                monolog.btns[2].onClick.AddListener(onClose);
            }

            monolog.transform.SetAsLastSibling();       //层级最前
            monolog.SetActive(true);
        }

        /// <summary>
        /// 显示双选对话框
        /// </summary>
        public static void ShowDialog(string title, string content, UnityAction onSure = null, UnityAction onCancel = null, UnityAction onClose = null) {
            dialog.Reset();
            dialog.texts[0].text = title;
            dialog.texts[1].text = content;

            if (onSure != null) {
                dialog.btns[0].onClick.AddListener(onSure);
            }
            if (onCancel != null) {
                dialog.btns[1].onClick.AddListener(onCancel);
            }
            if (onCancel != null) {
                dialog.btns[2].onClick.AddListener(onClose);
            }

            dialog.transform.SetAsLastSibling();       //层级最前
            dialog.SetActive(true);
        }

        public static bool AnyPanelActive() {
            return dialog.gameObject.activeInHierarchy || monolog.gameObject.activeInHierarchy || filePopType1.gameObject.activeInHierarchy || filePopType2.gameObject.activeInHierarchy;
        }
    }
}