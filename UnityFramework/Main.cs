using System;
using ZFrame.ZException;

namespace Zframe {
    /// <summary>
    /// 程序入口
    /// </summary>
    public sealed class Main : IDisposable {

        const bool _forceSingleton = true;

        static bool isRunning;

        internal IDisposable initInfo;

        public Main(IDisposable initInfo) {
            if (_forceSingleton) {
                if (isRunning) {
                    throw new ZFrameException("已经有运行中的程序实例。");
                }
            }

            isRunning = true;
            this.initInfo = initInfo;
        }

        ~Main() {
            Dispose();
        }

        public void Dispose() {
            isRunning = false;
            initInfo.Dispose();
            initInfo = null;
        }
    }
}
