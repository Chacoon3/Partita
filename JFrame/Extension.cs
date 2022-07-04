using JFrame.Core;
using UnityEngine;

namespace JFrame {
    internal static class Extension {
        public static void Print(this MonoBase mb, object message, LogType logType) {
#if UNITY_EDITOR
            switch (logType) {
                case LogType.Error:
                    Debug.LogError(message);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.Exception:
                    Debug.LogException((System.Exception)message);
                    break;
            }
#endif
        }

    }
}