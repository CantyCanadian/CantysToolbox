///====================================================================================================
///
///     FileBrowserManager by
///     - CantyCanadian
///     - gkngkx
///
///====================================================================================================

using System;

namespace Canty.Managers
{
    public class FileBrowserManager : Singleton<FileBrowserManager>
    {
        private static IStandaloneFileBrowser m_PlatformWrapper = null;

        /// <summary>
        /// Open native file browser
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extension">Allowed extension</param>
        /// <param name="multiselect">Allow multiple file selection</param>
        /// <returns>Returns array of chosen paths. Zero length array when cancelled</returns>
        public static string[] OpenFilePanel(string title, string directory, string extension, bool multiselect)
        {
            ExtensionFilter[] extensions = string.IsNullOrEmpty(extension) ? null : new ExtensionFilter[] { new ExtensionFilter("", extension) };
            return OpenFilePanel(title, directory, extensions, multiselect);
        }

        /// <summary>
        /// Native open file dialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="multiselect">Allow multiple file selection</param>
        /// <returns>Returns array of chosen paths. Zero length array when cancelled</returns>
        public static string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect)
        {
            if (m_PlatformWrapper == null)
            {
                Initalize();
            }

            return m_PlatformWrapper.OpenFilePanel(title, directory, extensions, multiselect);
        }

        /// <summary>
        /// Native open file dialog async
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extension">Allowed extension</param>
        /// <param name="multiselect">Allow multiple file selection</param>
        /// <param name="cb">Callback")</param>
        public static void OpenFilePanelAsync(string title, string directory, string extension, bool multiselect, Action<string[]> cb)
        {
            ExtensionFilter[] extensions = string.IsNullOrEmpty(extension) ? null : new[] { new ExtensionFilter("", extension) };
            OpenFilePanelAsync(title, directory, extensions, multiselect, cb);
        }

        /// <summary>
        /// Native open file dialog async
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="multiselect">Allow multiple file selection</param>
        /// <param name="cb">Callback")</param>
        public static void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb)
        {
            if (m_PlatformWrapper == null)
            {
                Initalize();
            }

            m_PlatformWrapper.OpenFilePanelAsync(title, directory, extensions, multiselect, cb);
        }

        /// <summary>
        /// Native open folder dialog
        /// <para>NOTE: Multiple folder selection isn't supported on Windows</para>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="directory">Root directory</param>
        /// <param name="multiselect"></param>
        /// <returns>Returns array of chosen paths. Zero length array when cancelled</returns>
        public static string[] OpenFolderPanel(string title, string directory, bool multiselect)
        {
            if (m_PlatformWrapper == null)
            {
                Initalize();
            }

            return m_PlatformWrapper.OpenFolderPanel(title, directory, multiselect);
        }

        /// <summary>
        /// Native open folder dialog async
        /// <para>NOTE: Multiple folder selection isn't supported on Windows</para>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="directory">Root directory</param>
        /// <param name="multiselect"></param>
        /// <param name="cb">Callback")</param>
        public static void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb)
        {
            m_PlatformWrapper.OpenFolderPanelAsync(title, directory, multiselect, cb);
        }

        /// <summary>
        /// Native save file dialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extension">File extension</param>
        /// <returns>Returns chosen path. Empty string when cancelled</returns>
        public static string SaveFilePanel(string title, string directory, string defaultName, string extension)
        {
            ExtensionFilter[] extensions = string.IsNullOrEmpty(extension) ? null : new ExtensionFilter[] { new ExtensionFilter("", extension) };

            return SaveFilePanel(title, directory, defaultName, extensions);
        }

        /// <summary>
        /// Native save file dialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <returns>Returns chosen path. Empty string when canceled.</returns>
        public static string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions)
        {
            if (m_PlatformWrapper == null)
            {
                Initalize();
            }

            return m_PlatformWrapper.SaveFilePanel(title, directory, defaultName, extensions);
        }

        /// <summary>
        /// Native save file dialog async
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extension">File extension</param>
        /// <param name="cb">Callback")</param>
        public static void SaveFilePanelAsync(string title, string directory, string defaultName, string extension, Action<string> cb)
        {
            ExtensionFilter[] extensions = string.IsNullOrEmpty(extension) ? null : new[] { new ExtensionFilter("", extension) };
            SaveFilePanelAsync(title, directory, defaultName, extensions, cb);
        }

        /// <summary>
        /// Native save file dialog async
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="cb">Callback")</param>
        public static void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb)
        {
            if (m_PlatformWrapper == null)
            {
                Initalize();
            }

            m_PlatformWrapper.SaveFilePanelAsync(title, directory, defaultName, extensions, cb);
        }

        /// <summary>
        /// Select which type of file browser to use depending on the environment.
        /// </summary>
        private static void Initalize()
        {
#if UNITY_STANDALONE_OSX
            m_PlatformWrapper = new StandaloneFileBrowserMac();
#elif UNITY_STANDALONE_WIN
            m_PlatformWrapper = new StandaloneFileBrowserWindows();
#elif UNITY_STANDALONE_LINUX
            m_PlatformWrapper = new StandaloneFileBrowserLinux();
#elif UNITY_EDITOR
            m_PlatformWrapper = new StandaloneFileBrowserEditor();
#endif
        }
    }

    public struct ExtensionFilter
    {
        public string Name;
        public string[] Extensions;

        public ExtensionFilter(string filterName, params string[] filterExtensions)
        {
            Name = filterName;
            Extensions = filterExtensions;
        }

        public static ExtensionFilter[] GetImageFileFilter()
        {
            ExtensionFilter png = new ExtensionFilter("png", "png");
            ExtensionFilter jpg = new ExtensionFilter("jpg", "jpg");

            return new ExtensionFilter[] { png, jpg };
        }
    }
}