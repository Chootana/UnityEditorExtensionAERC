using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeCharacterData : MonoBehaviour
{
    public int hp;
    public int attackPower;
    public float speed;
    public float dashSpeed;
    public GameObject weapon;
    public GUIStyle style;
    public string testString = "Example";

    public void GetTestString()
    {
        Debug.Log(testString);
    }
}
