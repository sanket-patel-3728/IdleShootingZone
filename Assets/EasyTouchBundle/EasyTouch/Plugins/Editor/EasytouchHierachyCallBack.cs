using UnityEngine;
using UnityEditor;
using System;
using HedgehogTeam.EasyTouch;

[InitializeOnLoad]
public class EasytouchHierachyCallBack
{
	static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;
	static Texture2D hierarchyIcon;

	static Texture2D HierarchyIcon
	{
		get
		{
			if (hierarchyIcon == null) hierarchyIcon = (Texture2D)Resources.Load ("EasyTouch_Icon");
			return hierarchyIcon;
		}
	}

	static Texture2D hierarchyEventIcon;
	static Texture2D HierarchyEventIcon
	{
		get
		{
			if (hierarchyEventIcon == null) hierarchyEventIcon = (Texture2D)Resources.Load ("EasyTouchTrigger_Icon");
			return hierarchyEventIcon;
		}
	}

	// constructor
	static EasytouchHierachyCallBack ()
	{
		hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback (EasytouchHierachyCallBack.DrawHierarchyIcon);
		EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine (EditorApplication.hierarchyWindowItemOnGUI, EasytouchHierachyCallBack.hiearchyItemCallback);
	}

	static void DrawHierarchyIcon (int instanceID, Rect selectionRect)
	{
		GameObject gameObject = EditorUtility.InstanceIDToObject (instanceID) as GameObject;

		if (gameObject != null)
		{
			Rect rect = new Rect (selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
			if (gameObject.GetComponent<EasyTouch> () != null) GUI.DrawTexture (rect, HierarchyIcon);
			else if (gameObject.GetComponent<QuickBase> () != null) GUI.DrawTexture (rect, HierarchyEventIcon);
#if FALSE
			else if (gameObject.GetComponent<EasyTouchSceneProxy> () != null) GUI.DrawTexture (rect, EasytouchHierachyCallBack.HierarchyIcon);
#endif
		}
	}
}