using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Animations;

public class AddEveryRotationConstraint : EditorWindow
{
    [SerializeField]
    GameObject fromJoint = null;

    [SerializeField]
    GameObject toJoint = null;

    private ConstraintSource fromConstraintSource;
    private bool includeInActive = false;

    private string parentName = "None";
    private int childrenSize = 0; 

    private enum CONSTRAINT_TYPE
    {
        RotationConstraint,
        ParentConstraint,
        PositionConstraint,
    }

    private CONSTRAINT_TYPE constraintType = CONSTRAINT_TYPE.RotationConstraint;


    [MenuItem("Window/Extension Tools/Add Every Rotation Constraint")]
    static void Open()
    {
        GetWindow<AddEveryRotationConstraint>();
    }

    private void OnGUI()
    {


        GUILayout.Label("", EditorStyles.boldLabel);



        Color defaultColor = GUI.backgroundColor;
        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUI.backgroundColor = Color.gray;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Original Avatar", EditorStyles.whiteLabel);
            }
            GUI.backgroundColor = defaultColor;

            EditorGUI.indentLevel++;
            fromJoint = EditorGUILayout.ObjectField("GameObject (Joint)", fromJoint, typeof(GameObject), true) as GameObject;
            ShowJointProperty(fromJoint);

            EditorGUI.indentLevel--;

        }


        GUILayout.Label("", EditorStyles.boldLabel);


        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUI.backgroundColor = Color.gray;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Target Avatar", EditorStyles.whiteLabel);
            }
            GUI.backgroundColor = defaultColor;

            EditorGUI.indentLevel++;
            toJoint = EditorGUILayout.ObjectField("GameObject (Joint)", toJoint, typeof(GameObject), true) as GameObject;
            ShowJointProperty(toJoint);
            EditorGUI.indentLevel--;
        }
        GUI.backgroundColor = defaultColor;


        GUILayout.Label("", EditorStyles.boldLabel);

        constraintType = (CONSTRAINT_TYPE)EditorGUILayout.EnumPopup("Constraint Type", constraintType);


        using (new GUILayout.HorizontalScope(GUI.skin.box))
        {
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Copy"))
            {
                Copy(fromJoint, toJoint);   
            }
            GUI.backgroundColor = defaultColor;


            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Reset"))  
            {
                Reset(toJoint);
            }
            GUI.backgroundColor = defaultColor;
        }
    }

    void Show(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
    }

    void ShowJointProperty(GameObject obj)
    {
        parentName = GetParentName(obj);
        childrenSize = GetChildrenSize(obj);
        EditorGUILayout.LabelField("Root Name", parentName);
        EditorGUILayout.LabelField("Number of Joints", childrenSize.ToString());

    }

    string GetParentName(GameObject obj)
    {
        if (obj == null)
        {
            return "None";
        }
        
        string nameRoot = obj.transform.root.gameObject.name.ToString();
        return nameRoot;
        
    }


    int GetChildrenSize(GameObject obj)
    {
        if (obj == null)
        {
            return 0;
        }

        int childrenSize = obj.GetComponentsInChildren<Transform>(includeInActive).Length;
        return childrenSize;
    }

    void Copy(GameObject fromJoint, GameObject toJoint)
    {
        if (fromJoint == null)
        {
            Debug.LogError("GameObject(from) not found");
        }

        if (toJoint == null)
        {
            Debug.LogError("GameObject(to) not found");
        }

        Transform[] fromChildren = fromJoint.GetComponentsInChildren<Transform>(includeInActive);
        Transform[] toChildren = toJoint.GetComponentsInChildren<Transform>(includeInActive);


        if (fromChildren.Length != toChildren.Length)
        {
            Debug.LogError("Mismatch of the number of joints, (from): " + fromChildren.Length + " != (to): " + toChildren.Length);
            return;
        }

        ResetConstraintSource(fromConstraintSource);
        fromConstraintSource.weight = 1.0f;


        foreach (var (joint, index) in toChildren.Select((x, i) => (x, i)))
        {

            switch(constraintType)
            {
                case CONSTRAINT_TYPE.RotationConstraint:
                    RotationConstraint rotationConstraint = joint.gameObject.GetComponent<RotationConstraint>();
                    if (rotationConstraint == null) rotationConstraint = joint.gameObject.AddComponent<RotationConstraint>();
                    if (rotationConstraint.sourceCount > 0) continue;
                    fromConstraintSource.sourceTransform = fromChildren[index];
                    rotationConstraint.AddSource(fromConstraintSource);
                    rotationConstraint.constraintActive = true;
                    break;

                case CONSTRAINT_TYPE.ParentConstraint:
                    ParentConstraint parentConstraint = joint.gameObject.GetComponent<ParentConstraint>();
                    if (parentConstraint == null) parentConstraint = joint.gameObject.AddComponent<ParentConstraint>();
                    if (parentConstraint.sourceCount > 0) continue;
                    fromConstraintSource.sourceTransform = fromChildren[index];
                    parentConstraint.AddSource(fromConstraintSource);
                    parentConstraint.constraintActive = true;
                    break;

                case CONSTRAINT_TYPE.PositionConstraint:
                    PositionConstraint positionConstraint = joint.gameObject.GetComponent<PositionConstraint>();
                    if (positionConstraint == null) positionConstraint = joint.gameObject.AddComponent<PositionConstraint>();
                    if (positionConstraint.sourceCount > 0) continue;
                    fromConstraintSource.sourceTransform = fromChildren[index];
                    positionConstraint.AddSource(fromConstraintSource);
                    positionConstraint.constraintActive = true;
                    break;

                default:
                    break;

            }
        }

        Debug.Log($"Add {constraintType} to Every Joints");
        ResetConstraintSource(fromConstraintSource);
        return;
    }



    void ResetConstraintSource(ConstraintSource constraintSource)
    {
        constraintSource.sourceTransform = null;
    }


    void Reset(GameObject toJoints)
    {
        if (toJoints == null)
        {
            Debug.LogError("GameObject(from) not found");
            return;
        }

        switch(constraintType)
        {
            case CONSTRAINT_TYPE.RotationConstraint:
                RotationConstraint[] rotationConstraints = toJoints.gameObject.GetComponentsInChildren<RotationConstraint>(includeInActive);
                foreach (RotationConstraint rotationConstraint in rotationConstraints) GameObject.DestroyImmediate(rotationConstraint);
                break;

            case CONSTRAINT_TYPE.ParentConstraint:
                ParentConstraint[] parentConstraints = toJoints.gameObject.GetComponentsInChildren<ParentConstraint>(includeInActive);
                foreach (ParentConstraint parentConstraint in parentConstraints) GameObject.DestroyImmediate(parentConstraint);
                break;

            case CONSTRAINT_TYPE.PositionConstraint:
                PositionConstraint[] positionConstraints = toJoints.gameObject.GetComponentsInChildren<PositionConstraint>(includeInActive);
                foreach (PositionConstraint positionConstraint in positionConstraints) GameObject.DestroyImmediate(positionConstraint);
                break;

            default:
                break;

        }

        Debug.Log($"Remove {constraintType}");
    }
}
