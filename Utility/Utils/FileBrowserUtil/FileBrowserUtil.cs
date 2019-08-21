///====================================================================================================
///
///     FileBrowserManager by
///     - CantyCanadian
///     - gkngkx
///
///====================================================================================================

using System;
using System.Collections.Generic;
using Canty.FileBrowser;

namespace Canty
{
    public class FileBrowserUtil
    {
        private static IStandaloneFileBrowser m_PlatformWrapper = null;

        /// <summary>
        /// Open native file browser
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extension">Allowed extension</param>
        /// <param name="multiselect">Allow selection of multiple files.</param>
        /// <returns>Returns array of chosen paths or zero length array when cancelled.</returns>
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
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png").</param>
        /// <param name="multiselect">Allow selection of multiple files.</param>
        /// <returns>Returns array of chosen paths or zero length array when cancelled.</returns>
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
        /// <param name="multiselect">Allow selection of multiple files.</param>
        /// <param name="callback">Callback for when the window closes.</param>
        public static void OpenFilePanelAsync(string title, string directory, string extension, bool multiselect, Action<string[]> callback)
        {
            ExtensionFilter[] extensions = string.IsNullOrEmpty(extension) ? null : new[] { new ExtensionFilter("", extension) };
            OpenFilePanelAsync(title, directory, extensions, multiselect, callback);
        }

        /// <summary>
        /// Native open file dialog async
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="multiselect">Allow selection of multiple files.</param>
        /// <param name="callback">Callback for when the window closes.</param>
        public static void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> callback)
        {
            if (m_PlatformWrapper == null)
            {
                Initalize();
            }

            m_PlatformWrapper.OpenFilePanelAsync(title, directory, extensions, multiselect, callback);
        }

        /// <summary>
        /// Native open folder dialog
        /// <para>NOTE: Multiple folder selection isn't supported on Windows</para>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="directory">Root directory</param>
        /// <param name="multiselect">Allow selection of multiple folders.</param>
        /// <returns>Returns array of chosen paths or zero length array when cancelled.</returns>
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
        /// <param name="multiselect">Allow selection of multiple folders.</param>
        /// <param name="callback">Callback for when the window closes.</param>
        public static void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> callback)
        {
            m_PlatformWrapper.OpenFolderPanelAsync(title, directory, multiselect, callback);
        }

        /// <summary>
        /// Native save file dialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extension">File extension</param>
        /// <returns>Returns chosen path or empty string when cancelled.</returns>
        public static string SaveFilePanel(string title, string directory, string defaultName, string extension)
        {
            ExtensionFilter[] extensions = string.IsNullOrEmpty(extension) ? null : new[] { new ExtensionFilter("", extension) };

            return SaveFilePanel(title, directory, defaultName, extensions);
        }

        /// <summary>
        /// Native save file dialog
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <returns>Returns chosen path or empty string when cancelled.</returns>
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
        /// <param name="callback">Callback for when the window closes.</param>
        public static void SaveFilePanelAsync(string title, string directory, string defaultName, string extension, Action<string> callback)
        {
            ExtensionFilter[] extensions = string.IsNullOrEmpty(extension) ? null : new[] { new ExtensionFilter("", extension) };
            SaveFilePanelAsync(title, directory, defaultName, extensions, callback);
        }

        /// <summary>
        /// Native save file dialog async
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png").</param>
        /// <param name="callback">Callback for when the window closes.</param>
        public static void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> callback)
        {
            if (m_PlatformWrapper == null)
            {
                Initalize();
            }

            m_PlatformWrapper.SaveFilePanelAsync(title, directory, defaultName, extensions, callback);
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

    /// <summary>
    /// Simple utility struct in order to store file filters and quickly obtain presets of filters.
    /// </summary>
    public struct ExtensionFilter
    {
        public string Name;
        public string[] Extensions;

        public ExtensionFilter(string filterName, params string[] filterExtensions)
        {
            Name = filterName;
            Extensions = filterExtensions;
        }

        /// <summary>
        /// Returns the given filter strings as filter objects for the FileBrowser. Ex. "png" will filter for '*.png' files.
        /// </summary>
        public static ExtensionFilter[] GetExtensionFilters(params string[] filters)
        {
            List<ExtensionFilter> filterList = new List<ExtensionFilter>();

            foreach (string filter in filters)
            {
                filterList.Add(new ExtensionFilter(filter, filter));
            }

            return filterList.ToArray();
        }
    }
}