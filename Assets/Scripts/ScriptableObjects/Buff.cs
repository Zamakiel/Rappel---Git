using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Scriptable Objects/Buff")]
public class Buff : ScriptableObject
{
    public string Name;

    // Some buffs won't have a duration (gaining health), we can set it at 0
    public float TotalBuffDuration;
    public float CurrentBuffDuration;
}
