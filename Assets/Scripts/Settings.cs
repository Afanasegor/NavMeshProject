using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "ScriptableObjects/PlayerSettings")]
public class Settings : ScriptableObject
{
    public FireMode fireMode;
    [Range(2, 6)] public int playerSpeed = 3;
    [Range(0.2f, 0.4f)] public float shootingTimeout = 0.2f;    

    public int GetFireModeIndex(FireMode fireMode, int powerIndex)
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
