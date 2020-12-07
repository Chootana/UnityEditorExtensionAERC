using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class AddRotationConstraints : MonoBehaviour
{
    private ConstraintSource myConstraintSource;

    public GameObject FindDeep(GameObject obj, string name, bool includeInactive = false)
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

    // Start is called before the first frame update
    void Start()
    {
        string[] name = { "Shoulder.L" };
        GameObject avatar = GameObject.Find("test");
        GameObject jointUser = FindDeep(avatar, "Shoulder.L.001");
        myConstraintSource.sourceTransform = jointUser.transform;
        myConstraintSource.weight = 1.0f;
        
        GameObject joint = FindDeep(avatar, name[0]);
        Debug.Log(joint);
        var rotationConstraint = joint.AddComponent<RotationConstraint>();
        rotationConstraint.constraintActive = true;
        rotationConstraint.AddSource(myConstraintSource);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
