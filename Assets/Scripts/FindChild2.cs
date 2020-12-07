using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindChild2 : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GameObject result = null;

        result = this.gameObject.FindDeep("child1");

        if (result != null)
        {
            Debug.Log(result.name);
        }
        else
        {
            Debug.Log(result);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
