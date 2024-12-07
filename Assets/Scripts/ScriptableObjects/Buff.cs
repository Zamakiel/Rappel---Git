using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Scriptable Objects/Buff")]
public class Buff : ScriptableObject
{
    public GameEvent OnBuffObtained;
    // Some buffs won't have a duration (gaining health), we can set it at 0
    public int BuffDuration;
    // If the buff has no duration, this event will never be raised (warning?)
    public GameEvent OnBuffEnded;
}
