///====================================================================================================
///
///     DeleteCallback by
///     - CantyCanadian
///     - JPBotelho
///
///====================================================================================================

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.IO;

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