using UnityEditor;
using UnityEngine;
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

    private string fromParentName = "None";
    private string toNameRoot = "None";
    private int fromChildrenSize = 0;
    private int toChildrenSize = 0;

    [MenuItem("Window/VRChat")]
    static void Open()
    {
        GetWindow<AddEveryRotationConstraint>();
    }

    private void OnGUI()
    {
        fromJoint = EditorGUILayout.ObjectField("from", fromJoint, typeof(GameObject), true) as GameObject;


        // [TODO] ここを一つの関数にまとめる
        fromParentName = GetParentName(fromJoint);
        fromChildrenSize = GetChildrenSize(fromJoint);
        EditorGUILayout.LabelField("Name", fromParentName);
        EditorGUILayout.LabelField("Size ", fromChildrenSize.ToString());

        GUILayout.Label("", EditorStyles.boldLabel);

        toJoint = EditorGUILayout.ObjectField("to", toJoint, typeof(GameObject), true) as GameObject;

        toNameRoot = GetParentName(toJoint);
        toChildrenSize = GetChildrenSize(toJoint);
        EditorGUILayout.LabelField("Name", toNameRoot);
        EditorGUILayout.LabelField("Size ", toChildrenSize.ToString());


        GUILayout.Label("", EditorStyles.boldLabel);
        if (GUILayout.Button("Copy"))
        {
            Copy(fromJoint, toJoint);   
        }

        GUILayout.Label("", EditorStyles.boldLabel);
 
        if (GUILayout.Button("Reset"))
        {
            Reset(toJoint);
        }
    }

    void Show(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
    }

    string GetParentName(GameObject obj)
    {
        if (obj == null)
        {
            return "None";
        }
        
        string nameRoot = obj.name.ToString();
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
            RotationConstraint rotationConstraint = joint.gameObject.GetComponent<RotationConstraint>();
            if (rotationConstraint == null)
            {
                rotationConstraint = joint.gameObject.AddComponent<RotationConstraint>();
            }

            fromConstraintSource.sourceTransform = fromChildren[index];

            rotationConstraint.AddSource(fromConstraintSource);
            rotationConstraint.constraintActive = true;
        }

        Debug.Log("Add Rotation Constraint to Every Joints");
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

        RotationConstraint[] rotationConstraints = toJoints.gameObject.GetComponentsInChildren<RotationConstraint>(includeInActive);
        foreach (RotationConstraint rotationConstraint in rotationConstraints)
        {
            Debug.Log(rotationConstraint);
            Debug.Log("Delete: " + rotationConstraint.name + "'s Rotation Constraint");
            GameObject.DestroyImmediate(rotationConstraint);
        }
    }
}
