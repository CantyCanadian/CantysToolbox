///====================================================================================================
///
///     StandaloneFileBrowserLinux by
///     - CantyCanadian
///     - gkngkx
///     - Ookii
///     - RicardoEPRodrigues
///
///====================================================================================================

#if UNITY_STANDALONE_LINUX

using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Canty.FileBrowser
{
    public class StandaloneFileBrowserLinux : IStandaloneFileBrowser 
    {
        private static Action<string[]> m_OpenFileCallback;
        private static Action<string[]> m_OpenFolderCallback;
        private static Action<string> m_SaveFileCallback;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AsyncCallback(string path);

        [DllImport("StandaloneFileBrowser")]
        private static extern void DialogInit();
        [DllImport("StandaloneFileBrowser")]
        private static extern IntPtr DialogOpenFilePanel(string title, string directory, string extension, bool multiselect);
        [DllImport("StandaloneFileBrowser")]
        private static extern void DialogOpenFilePanelAsync(string title, string directory, string extension, bool multiselect, AsyncCallback callback);
        [DllImport("StandaloneFileBrowser")]
        private static extern IntPtr DialogOpenFolderPanel(string title, string directory, bool multiselect);
        [DllImport("StandaloneFileBrowser")]
        private static extern void DialogOpenFolderPanelAsync(string title, string directory, bool multiselect, AsyncCallback callback);
        [DllImport("StandaloneFileBrowser")]
        private static extern IntPtr DialogSaveFilePanel(string title, string directory, string defaultName, string extension);
        [DllImport("StandaloneFileBrowser")]
        private static extern void DialogSaveFilePanelAsync(string title, string directory, string defaultName, string extension, AsyncCallback callback);

        public StandaloneFileBrowserLinux()
        {
            DialogInit();
        }

        public string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect)
        {
            string paths = Marshal.PtrToStringAnsi(DialogOpenFilePanel(title, directory, GetFilterFromFileExtensionList(extensions), multiselect));
            return paths.Split((char)28);
        }

        public void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb)
        {
            m_OpenFileCallback = cb;
            DialogOpenFilePanelAsync(title, directory, GetFilterFromFileExtensionList(extensions), multiselect, (string result) => { m_OpenFileCallback.Invoke(result.Split((char)28)); });
        }

        public string[] OpenFolderPanel(string title, string directory, bool multiselect)
        {
            string paths = Marshal.PtrToStringAnsi(DialogOpenFolderPanel(title, directory, multiselect));
            return paths.Split((char)28);
        }

        public void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb)
        {
            m_OpenFolderCallback = cb;
            DialogOpenFolderPanelAsync(title, directory, multiselect, (string result) => { m_OpenFolderCallback.Invoke(result.Split((char)28)); });
        }

        public string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions)
        {
            return Marshal.PtrToStringAnsi(DialogSaveFilePanel(title, directory, defaultName, GetFilterFromFileExtensionList(extensions)));
        }

        public void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb) {
            m_SaveFileCallback = cb;
            DialogSaveFilePanelAsync(title, directory, defaultName, GetFilterFromFileExtensionList(extensions), (string result) => { m_SaveFileCallback.Invoke(result); });
        }

        private static string GetFilterFromFileExtensionList(ExtensionFilter[] extensions)
        {
            if (extensions == null)
            {
                return "";
            }

            string filterString = "";
            foreach (ExtensionFilter filter in extensions)
            {
                filterString += filter.Name + ";";

                foreach (string ext in filter.Extensions)
                {
                    filterString += ext + ",";
                }

                filterString = filterString.Remove(filterString.Length - 1);
                filterString += "|";
            }

            filterString = filterString.Remove(filterString.Length - 1);
            return filterString;
        }
    }
}

#endif