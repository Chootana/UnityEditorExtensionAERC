using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EditorWindowSample : EditorWindow
{
    [MenuItem("Editor/Sample")]
    private static void Create()
    {
        GetWindow<EditorWindowSample>("サンプル");
    }
}
