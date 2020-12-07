using UnityEditor;
using UnityEngine;

public static class ToggleGameObjectActive
{
    private const int WIDTH = 16;

    [InitializeOnLoadMethod]
    private static void Example()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnGUI;
    }

    private static void OnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject == null)
        {
            return;
        }

        var pos = selectionRect;
        pos.x = pos.xMax - WIDTH;
        pos.width = WIDTH;

        var newActive = GUI.Toggle(pos, gameObject.activeSelf, string.Empty);

        if (newActive == gameObject.activeSelf)
        {
            return;
        }

        gameObject.SetActive(newActive);
    }
}
