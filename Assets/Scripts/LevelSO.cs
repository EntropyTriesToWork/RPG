using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data")]
public class LevelSO : ScriptableObject
{
    public string key;
    public LevelData levelData;
}
