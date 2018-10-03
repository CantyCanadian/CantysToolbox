#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;

// Part of Recycle Bin by JPBotelho on Github : https://github.com/JPBotelho/Recycle-Bin
namespace Canty.Editor.RecycleBin
{
	[ExecuteInEditMode]
	public class DeleteCallback : UnityEditor.AssetModificationProcessor
	{
		static private AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
		{
			RecycleBinFunctions.DeleteAndCopyToRecycleBin(new FileInfo(path));

			return AssetDeleteResult.DidNotDelete;
		}
	}
}

#endif