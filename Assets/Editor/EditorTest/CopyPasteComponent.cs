using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using System.Collections.Generic;


public class CopyPasteComponent
{
    static GameObject cacheObject;
    static bool isCut = false;

    [MenuItem("Edit/Component/Copy %#C")]
    static void Copy()
    {
        isCut = false;
        cacheObject = Selection.activeGameObject;
    }

    [MenuItem("Edit/Component/Paste %#X")]
    static void Cut()
    {
        isCut = true;
        cacheObject = Selection.activeGameObject;
    }

    [MenuItem("Edit/Component/Paste %#V")]
    static void Paste()
    {
        if (cacheObject == null)
        {
            Debug.LogError("Lost Original GameObject");
            return;
        }
        CopyComponents();

        if (isCut == true)
        {
            RemoveComponents();
        }
    }

    static void RemoveComponents()
    {
        var targetComponents = cacheObject.GetComponents<Component>();
        foreach (var component in targetComponents)
        {
            if (component is Transform == false)
            {
                GameObject.DestroyImmediate(component);
            }
        }
    }

    static void CopyComponents()
    {
        var currentGameObject = Selection.activeGameObject;

        var components = cacheObject.GetComponents<Component>();
        var targetComponents = currentGameObject.GetComponents<Component>();
        Dictionary<System.Type, int> currentComponentCount = new Dictionary<System.Type, int>();

        foreach (var component in components)
        {
            var componentCount = targetComponents.Count(c => c.GetType() == component.GetType());
            ComponentUtility.CopyComponent(component);
            if (componentCount == 0)
            {
                ComponentUtility.PasteComponentAsNew(currentGameObject);
            }
            else if (componentCount == 1)
            {
                var targetComponent = targetComponents.First(c => c.GetType() == component.GetType());
                ComponentUtility.PasteComponentValues(targetComponent);
            }
            else
            {
                if (currentComponentCount.ContainsKey(component.GetType()) == false)
                {
                    currentComponentCount.Add(component.GetType(), 0);
                }
                var count = currentComponentCount[component.GetType()];
                var targetComponentsWithType = targetComponents.Where(c => c.GetType() == component.GetType());
                if (count < targetComponentsWithType.Count())
                {
                    var targetComponent = targetComponents.Where(c => c.GetType() == component.GetType()).ElementAt(count);
                    currentComponentCount[component.GetType()] += 1;
                    ComponentUtility.PasteComponentValues(targetComponent);
                }
                else
                {
                    ComponentUtility.PasteComponentAsNew(currentGameObject);
                }
            }
        }
    }
}