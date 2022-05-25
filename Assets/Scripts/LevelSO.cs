using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Level Data")]
public class LevelSO : ScriptableObject
{
    public string key;
    public int stars = 0;
    public List<StarRequirement> starRequirements = new List<StarRequirement>();
    public LevelData levelData;
}
[System.Serializable]
public class StarRequirement
{
    public StarRequirementType type;
    [ShowIf("type", StarRequirementType.Time)] public float time;
}
public enum StarRequirementType
{
    Time,
    Hitless,
    FullHealth,
    NoDashing,
}