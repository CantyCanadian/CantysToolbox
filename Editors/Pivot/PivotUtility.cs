﻿///====================================================================================================
///
///     PivotUtilities by
///     - CantyCanadian
///     - Talecrafter
///
///====================================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Canty.Editors
{
    public static class PivotUtilities
    {
		/// <summary>
		/// Creates a pivot game object and attaches the selected game object to it.
		/// </summary>
        [MenuItem("GameObject/Pivot/Create Pivot", false, 0)]
        private static void CreatePivotObjectMenuItem()
        {
            if (Selection.activeGameObject != null)
            {
                GameObject pivot = CreatePivotObject(Selection.activeGameObject);
                Selection.activeGameObject = pivot;
            }
        }

		/// <summary>
		/// Creates a pivot game object, sets its local transform to 0 and attaches the selected game object to it.
		/// </summary>
        [MenuItem("GameObject/Pivot/Create Pivot (Parent Zero)", false, 0)]
        private static void CreatePivotObjectParentZeroMenuItem()
        {
            if (Selection.activeGameObject != null)
            {
                GameObject pivot = CreatePivotObjectParentZero(Selection.activeGameObject);
                Selection.activeGameObject = pivot;
            }
        }

		/// <summary>
		/// Creates a pivot game object, sets its world transform to 0 and attaches the selected game object to it.
		/// </summary>
        [MenuItem("GameObject/Pivot/Create Pivot (World Zero)", false, 0)]
        private static void CreatePivotObjectWorldZeroMenuItem()
        {
            if (Selection.activeGameObject != null)
            {
                GameObject pivot = CreatePivotObjectWorldZero(Selection.activeGameObject);
                Selection.activeGameObject = pivot;
            }
        }

		/// <summary>
		/// Deletes the selected object and reattaches its children to its parents.
		/// </summary>
        [MenuItem("GameObject/Pivot/Delete Pivot", false, 0)]
        private static void DeletePivotObjectMenuItem()
        {
            GameObject objSelectionAfter = null;

            if (Selection.activeGameObject != null)
            {
                if (Selection.activeGameObject.transform.childCount > 0)
                {
                    objSelectionAfter = Selection.activeGameObject.transform.GetChild(0).gameObject;
                }
                else if (Selection.activeGameObject.transform.parent != null)
                {
                    objSelectionAfter = Selection.activeGameObject.transform.parent.gameObject;
                }

                DeletePivotObject(Selection.activeGameObject);

                Selection.activeGameObject = objSelectionAfter;
            }
        }

		/// <summary>
		/// Function to create a pivot with the same transform as the parent.
		/// </summary>
        private static GameObject CreatePivotObject(GameObject current)
        {
            if (current == null)
            {
                return null;
            }

            int siblingIndex = current.transform.GetSiblingIndex();

            GameObject newObject = new GameObject("Pivot");
            newObject.transform.SetParent(current.transform.parent);

            newObject.transform.position = current.transform.position;
            newObject.transform.localScale = current.transform.localScale;
            newObject.transform.rotation = current.transform.rotation;

            newObject.transform.SetSiblingIndex(siblingIndex);

            current.transform.SetParent(newObject.transform);

            return newObject;
        }
		
		/// <summary>
		/// Function to create a pivot with its local transform set to 0.
		/// </summary>
        private static GameObject CreatePivotObjectParentZero(GameObject current)
        {
            if (current == null)
            {
                return null;
            }

            int siblingIndex = current.transform.GetSiblingIndex();

            GameObject newObject = new GameObject("Pivot");
            newObject.transform.SetParent(current.transform.parent);

            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localScale = Vector3.one;
            newObject.transform.localRotation = Quaternion.identity;

            newObject.transform.SetSiblingIndex(siblingIndex);

            current.transform.SetParent(newObject.transform);

            return newObject;
        }

		/// <summary>
		/// Function to create a pivot with its world transform set to 0.
		/// </summary>
        private static GameObject CreatePivotObjectWorldZero(GameObject current)
        {
            if (current == null)
            {
                return null;
            }

            int siblingIndex = current.transform.GetSiblingIndex();

            GameObject newObject = new GameObject("Pivot");

            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localScale = Vector3.one;
            newObject.transform.localRotation = Quaternion.identity;

            newObject.transform.SetParent(current.transform.parent);
            newObject.transform.SetSiblingIndex(siblingIndex);

            current.transform.SetParent(newObject.transform);

            return newObject;
        }

		/// <summary>
		/// Function to create a pivot with its world transform set to 0.
		/// </summary>
        private static GameObject DeletePivotObject(GameObject current)
        {
            Transform parent = current.transform.parent;
            int childrenCount = current.transform.childCount;
            int siblingIndex = current.transform.GetSiblingIndex();

            Transform[] children = new Transform[childrenCount];
            for (int i = 0; i < childrenCount; i++)
            {
                children[i] = current.transform.GetChild(i);
            }

            for (int i = 0; i < childrenCount; i++)
            {
                children[i].SetParent(parent);
                children[i].SetSiblingIndex(siblingIndex + i);
            }

            if (Application.isPlaying)
            {
                GameObject.Destroy(current);
            }
            else
            {
                GameObject.DestroyImmediate(current);
            }

            if (children.Length > 0)
            {
                return children[0].gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}
