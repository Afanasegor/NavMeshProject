using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "ScriptableObjects/PlayerSettings")]
public class Settings : ScriptableObject
{
    public FireMode fireMode; // force of shooting
    [Range(2, 6)] public int playerSpeed = 3;
    [Range(0.2f, 0.4f)] public float shootingTimeout = 0.2f;    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fireMode"></param>
    /// <param name="powerIndex">Needs to multiply fireMode. Default value: 1000f (in PlayerController)</param>
    /// <returns></returns>
    public int GetFireMode(FireMode fireMode, int powerIndex)
    {
        if (fireMode == FireMode.Low)
            return 1*powerIndex;
        else if (fireMode == FireMode.Middle)
            return 3*powerIndex;
        else
            return 5*powerIndex;
    }

    public enum FireMode : byte
    {
        Low,
        Middle,
        High
    }
}