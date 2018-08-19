using System.Collections.Generic;
using System;
using System.IO;

// Part of Recycle Bin by JPBotelho on Github : https://github.com/JPBotelho/Recycle-Bin

/// <summary>
/// Class for handling files in the recycle bin and its subdirectories (if it's a directory).
/// </summary>
[Serializable]
public sealed class TrashFile
{
    public string path;
    public List<DirectoryInfo> directories = new List<DirectoryInfo>();
    
    public TrashFile (string s)
    {
        path = s;
    }   
}

/// <summary>
/// Compares two "trash files" by last write date.
/// </summary>
public class ComparerByDate : IComparer<TrashFile>
{
    public int Compare (TrashFile obj, TrashFile obj2)
    {
        FileInfo file = new FileInfo(obj.path);
        FileInfo otherfile = new FileInfo(obj2.path);

        return -file.LastAccessTimeUtc.CompareTo(otherfile.LastWriteTimeUtc);
    }
}