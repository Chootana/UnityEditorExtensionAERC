using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using System;
using System.Collections.Generic;
using System.Linq;


public class CopyComponent
{
    static GameObject cacheObject;
    static ConstraintSource tgtConstraintSource;
    static Transform[] childrenJoint;

    [MenuItem("Edit/Test/Select")]
    static void SelectObject()
    {
        cacheObject = Selection.activeGameObject;

        Debug.Log("This is " + cacheObject.name);
    }

    [MenuItem("Edit/Test/Add Rotation Constraint")]
    static void Add()
    {
        if (cacheObject == null)
        {
            Debug.LogError("Lost Original GameObject");
            return;
        }

        AddRotationConstraint("Shoulder.L", ".001");
        AddRotationConstraint("Shoulder.R", ".001");
    }

    [MenuItem("Edit/Test/Remove Rotation Constraint")]
    static void RemoveRotationConstraint()
    {
        if (cacheObject == null)
        {
            Debug.LogError("Lost Original GameObject");
        }

        RotationConstraint[] rotationConstraints = cacheObject.GetComponentsInChildren<RotationConstraint>();
        foreach (RotationConstraint rotationConstraint in rotationConstraints)
        {
            Debug.Log("Delete: " + rotationConstraint.gameObject.name + "'s Rotation Constraint");
            GameObject.DestroyImmediate(rotationConstraint);
        }

    }

    static GameObject FindDeep(GameObject obj, string name, bool includeInactive = false)
    {
        Transform[] children = obj.GetComponentsInChildren<Transform>(includeInactive);

        foreach (Transform transform in children)
        {
            if (transform.name == name)
            {
                return transform.gameObject;
            }
        }

        return null;
    }

    static Transform[] GeTransformChildren(GameObject parent)
    {
        Transform[] allGameObject = parent.GetComponentsInChildren<Transform>();
        return allGameObject;
    }

    static void AddRotationConstraint(string name, string suffix)
    {
        string tgtName = name + suffix;

        GameObject srcJoint = FindDeep(cacheObject, name);
        childrenJoint = GeTransformChildren(srcJoint);

        GameObject tgtJoint = FindDeep(cacheObject, tgtName);
        Transform[] tgtChildrenJoint = GeTransformChildren(tgtJoint);

        if (childrenJoint.Length != tgtChildrenJoint.Length)
        {
            Debug.LogError("Size mismatch, src joint number = " + childrenJoint.Length + " target joint number = " + tgtChildrenJoint.Length);
        }

        foreach (var (item, index) in childrenJoint.Select((x, i) => (x, i)))
        {


            GameObject childJoint = item.gameObject;
            RotationConstraint rotationConstraint = childJoint.AddComponent<RotationConstraint>();

            tgtConstraintSource.sourceTransform = tgtChildrenJoint[index];
            tgtConstraintSource.weight = 1.0f;

            rotationConstraint.AddSource(tgtConstraintSource);
            rotationConstraint.constraintActive = true;

        }

    }
}