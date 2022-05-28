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
    [ShowIf("type", StarRequirementType.KillAmount)] public int kills;
    [ShowIf("type", StarRequirementType.FruitCollected)] public int fruitCollected;
}
public enum StarRequirementType
{
    Time = 0,
    Hitless = 1,
    FullHealth = 2,
    NoDashing = 3,
    NoJumping = 4,
    NoKilling = 5,
    KillAmount = 6,
    FruitCollected = 7,
}