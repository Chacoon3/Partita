
using System.IO;
using System.Runtime.InteropServices;
using System;

namespace Partita.Utility {
    public static class FolderBrowserHelper {
        #region Window

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetSaveFileName([In, Out] OpenFileName ofn);

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] OpenDialogFile ofn);

        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool GetSaveFileName([In, Out] OpenDialogFile ofn);

        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SHBrowseForFolder([In, Out] OpenDialogDir ofn);

        [DllImport("shell32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        private static extern bool SHGetPathFromIDList([In] IntPtr pidl, [In, Out] char[] fileName);

        #endregion


        public const string IMAGEFILTER = "图片文件(*.jpg;*.png)\0*.jpg;*.png";
        public const string ALLFILTER = "所有文件(*.*)\0*.*";
        public const string EXCELFILTER = "Excel文件(*.xlsx;*.xls)\0*.xlsx;*.xls";


        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="callback">返回选择文件夹的路径</param>
        /// <param name="filter">文件类型筛选器</param>
        public static void SelectFile(Action<string> callback, string filter = EXCELFILTER) {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = filter;
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.title = "选择文件";
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
            if (GetOpenFileName(openFileName)) {
                string filepath = openFileName.file; //选择的文件路径;  
                if (File.Exists(filepath)) {
                    //Debug.Log("========" + filepath);
                    if (callback != null)
                        callback(filepath);
                    return;
                }
            }
        }

        public static void SaveFile(Action<string> callback, string filter = EXCELFILTER) {
            OpenFileName openFileName = new OpenFileName();
            openFileName.structSize = Marshal.SizeOf(openFileName);
            openFileName.filter = filter;
            openFileName.file = new string(new char[256]);
            openFileName.maxFile = openFileName.file.Length;
            openFileName.fileTitle = new string(new char[64]);
            openFileName.maxFileTitle = openFileName.fileTitle.Length;
            openFileName.title = "选择文件";
            openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
            if (GetSaveFileName(openFileName)) {
                string filepath = openFileName.file;
                if (!filepath.Contains(".xls")) {
                    filepath += ".xls"; //选择的文件路径;
                }
                //Debug.Log("========" + filepath);
                if (callback != null)
                    callback(filepath);
            }
        }
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenDialogFile {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public string filter = null;
        public string customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public string file = null;
        public int maxFile = 0;
        public string fileTitle = null;
        public int maxFileTitle = 0;
        public string initialDir = null;
        public string title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public string defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public string templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;


    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenDialogDir {
        public IntPtr hwndOwner = IntPtr.Zero;
        public IntPtr pidlRoot = IntPtr.Zero;
        public string pszDisplayName = null;
        public string lpszTitle = null;
        public uint ulFlags = 0;
        public IntPtr lpfn = IntPtr.Zero;
        public IntPtr lParam = IntPtr.Zero;
        public int iImage = 0;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public string filter = null;
        public string customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public string file = null;
        public int maxFile = 0;
        public string fileTitle = null;
        public int maxFileTitle = 0;
        public string initialDir = null;
        public string title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public string defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public string templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }
}
