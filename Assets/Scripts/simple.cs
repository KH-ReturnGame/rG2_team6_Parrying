using UnityEngine;
using System;

public class AllAttackDebug : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("All_attack ENABLED at frame " + Time.frameCount);
        Debug.Log(Environment.StackTrace);
    }
}
