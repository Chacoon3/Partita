using Partita.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Partita.ExtComponent {

    /// <summary>
    /// 鼠标指针管理
    /// </summary>
    [DisallowMultipleComponent]
    internal class CursorChanger : MonoBehaviour {
        /*
         * 功能实现：鼠标悬停至按钮上时，鼠标图标变成cursorTex指定的2D贴图。鼠标不再悬停时，恢复默认鼠标图标
         */

        Texture2D cursorSelectable;
        GraphicRaycaster canvasRaycaster;
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        void Awake() {
            if (cursorSelectable == null) {
                //cursorSelectable = ABUtil.bundle.LoadAsset<Texture2D>("CursorSelected");
                throw new NotImplementedException();
            }

            canvasRaycaster = null;
            throw new NullReferenceException();
        }

        void Update() {
            SetCursor();
        }

        void SetCursor() {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            raycastResults.Clear();
            canvasRaycaster.Raycast(pointerEventData, raycastResults);

            Selectable selectable = null;
            TMP_InputField field = null;
            var target = raycastResults.FirstOrDefault(r => r.gameObject.activeInHierarchy).gameObject;
            if (target == null) {
                Cursor.SetCursor(default, Vector2.zero, CursorMode.Auto);
                return;
            }
            else {
                selectable = target.GetComponent<Selectable>();
                if (!selectable) {
                    selectable = target.GetComponentInParent<Selectable>();
                }

                field = target.GetComponent<TMP_InputField>();
                if (!field) {
                    field = target.GetComponentInParent<TMP_InputField>();
                }

                if (selectable && selectable.enabled && selectable.interactable ||
                     field && field.enabled && field.interactable) {
                    Cursor.SetCursor(cursorSelectable, Vector2.zero, CursorMode.Auto);
                }
                else {
                    Cursor.SetCursor(default, Vector2.zero, CursorMode.Auto);
                }
            }

            raycastResults.Clear();
        }
    }
}
