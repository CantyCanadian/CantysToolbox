///====================================================================================================
///
///     StandaloneFileBrowserWindows by
///     - CantyCanadian
///     - gkngkx
///     - Ookii
///
///====================================================================================================

#if UNITY_STANDALONE_WIN

using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Ookii.Dialogs;

namespace Canty.FileBrowser
{ 
    public class WindowWrapper : IWin32Window 
    {
        private IntPtr windowHandle;

        public WindowWrapper(IntPtr handle) 
        { 
            windowHandle = handle; 
        }

        public IntPtr Handle 
        {
            get 
            { 
                return windowHandle; 
            } 
        }
    }

    public class StandaloneFileBrowserWindows : IStandaloneFileBrowser 
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();
        
        public string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect) 
        {
            VistaOpenFileDialog dialog = new VistaOpenFileDialog();
            dialog.Title = title;

            if (extensions != null) 
            {
                dialog.Filter = GetFilterFromFileExtensionList(extensions);
                dialog.FilterIndex = 1;
            }
            else 
            {
                dialog.Filter = string.Empty;
            }

            dialog.Multiselect = multiselect;

            if (!string.IsNullOrEmpty(directory)) 
            {
                dialog.FileName = GetDirectoryPath(directory);
            }

            DialogResult result = dialog.ShowDialog(new WindowWrapper(GetActiveWindow()));
            string[] filenames = result == DialogResult.OK ? dialog.FileNames : new string[0];

            dialog.Dispose();

            return filenames;
        }

        public void OpenFilePanelAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, Action<string[]> cb)
        {
            cb.Invoke(OpenFilePanel(title, directory, extensions, multiselect));
        }

        public string[] OpenFolderPanel(string title, string directory, bool multiselect) 
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.Description = title;

            if (!string.IsNullOrEmpty(directory)) 
            {
                dialog.SelectedPath = GetDirectoryPath(directory);
            }

            DialogResult result = dialog.ShowDialog(new WindowWrapper(GetActiveWindow()));
            string[] filenames = result == DialogResult.OK ? new string[] { dialog.SelectedPath } : new string[0];

            dialog.Dispose();

            return filenames;
        }

        public void OpenFolderPanelAsync(string title, string directory, bool multiselect, Action<string[]> cb)
        {
            cb.Invoke(OpenFolderPanel(title, directory, multiselect));
        }

        public string SaveFilePanel(string title, string directory, string defaultName, ExtensionFilter[] extensions) 
        {
            VistaSaveFileDialog dialog = new VistaSaveFileDialog();
            dialog.Title = title;

            string finalFilename = "";

            if (!string.IsNullOrEmpty(directory)) 
            {
                finalFilename = GetDirectoryPath(directory);
            }

            if (!string.IsNullOrEmpty(defaultName)) 
            {
                finalFilename += defaultName;
            }

            dialog.FileName = finalFilename;

            if (extensions != null) 
            {
                dialog.Filter = GetFilterFromFileExtensionList(extensions);
                dialog.FilterIndex = 1;
                dialog.DefaultExt = extensions[0].Extensions[0];
                dialog.AddExtension = true;
            }
            else 
            {
                dialog.DefaultExt = string.Empty;
                dialog.Filter = string.Empty;
                dialog.AddExtension = false;
            }

            DialogResult result = dialog.ShowDialog(new WindowWrapper(GetActiveWindow()));
            string filename = result == DialogResult.OK ? dialog.FileName : "";

            dialog.Dispose();

            return filename;
        }

        public void SaveFilePanelAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, Action<string> cb)
        {
            cb.Invoke(SaveFilePanel(title, directory, defaultName, extensions));
        }

        // .NET Framework FileDialog Filter format
        // https://msdn.microsoft.com/en-us/library/microsoft.win32.filedialog.filter
        private static string GetFilterFromFileExtensionList(ExtensionFilter[] extensions) 
        {
            string filterString = "";

            foreach (ExtensionFilter filter in extensions) 
            {
                filterString += filter.Name + "(";

                foreach (string extention in filter.Extensions) 
                {
                    filterString += "*." + extention + ",";
                }

                filterString = filterString.Remove(filterString.Length - 1);

                filterString += ") |";

                foreach (var extention in filter.Extensions) 
                {
                    filterString += "*." + extention + "; ";
                }

                filterString += "|";
            }

            filterString = filterString.Remove(filterString.Length - 1);

            return filterString;
        }

        private static string GetDirectoryPath(string directory) 
        {
            string directoryPath = Path.GetFullPath(directory);

            if (!directoryPath.EndsWith("\\")) 
            {
                directoryPath += "\\";
            }

            if (System.IO.Path.GetPathRoot(directoryPath) == directoryPath)
            {				
                return directoryPath;
            }

            return Path.GetDirectoryName(directoryPath) + Path.DirectorySeparatorChar;
        }
    }
}

#endif