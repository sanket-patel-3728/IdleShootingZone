using UnityEditor;
using HedgehogTeam.EasyTouch;

public static class EasyTouchMenu
{
	[MenuItem ("GameObject/EasyTouch/EasyTouch", false, 0)]
	static void AddEasyTouch () => Selection.activeObject = EasyTouch.instance.gameObject;
}