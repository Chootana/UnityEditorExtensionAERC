using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations;

public class AddEveryRotationConstraint : EditorWindow
{
    [SerializeField]
    GameObject fromJoints = null;

    [SerializeField]
    GameObject toJoints = null;

    private ConstraintSource fromConstraintSource;

    [MenuItem("Window/VRChat")]
    static void Open()
    {
        GetWindow<AddEveryRotationConstraint>();
    }

    private void OnGUI()
    {
        fromJoints = EditorGUILayout.ObjectField("from", fromJoints, typeof(GameObject), true) as GameObject;
        toJoints = EditorGUILayout.ObjectField("to", toJoints, typeof(GameObject), true) as GameObject;

        GUILayout.Label("", EditorStyles.boldLabel);
        if (GUILayout.Button("Copy"))
        {
            Copy(fromJoints, toJoints);
        }

        GUILayout.Label("", EditorStyles.boldLabel);
        if (GUILayout.Button("Reset"))
        {
            Reset(toJoints);
        }
    }

    void Show(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
    }

    void Copy(GameObject fromJoints, GameObject toJoints)
    {
        if (fromJoints == null)
        {
            Debug.LogError("GameObject(from) not found");
            return;
        }
            
        if (toJoints == null)
        {
            Debug.LogError("GameObject(to) not found");
            return;
        }

        ResetConstraintSource(fromConstraintSource);
        fromConstraintSource.weight = 1.0f;

        Transform[] fromChildren = fromJoints.GetComponentsInChildren<Transform>();
        Transform[] toChildren = toJoints.GetComponentsInChildren<Transform>();

        // [TODO] 本当に一致しているか？
        // [TODO] デフォルトジョイントと一致したものだけ（トリガーで無理やり交換も可能にする，）

        if (fromChildren.Length != toChildren.Length)
        {
            Debug.LogError("Mismatch of the number of joints, (from): " + fromChildren.Length + " != (to): " + toChildren.Length);
            return;
        }

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

        RotationConstraint[] rotationConstraints = toJoints.gameObject.GetComponentsInChildren<RotationConstraint>();
        foreach (RotationConstraint rotationConstraint in rotationConstraints)
        {
            Debug.Log(rotationConstraint);
            Debug.Log("Delete: " + rotationConstraint.name + "'s Rotation Constraint");
            GameObject.DestroyImmediate(rotationConstraint);
        }
    }
}
