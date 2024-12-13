using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalGameObjectList", menuName = "Scriptable Objects/GlobalGameObjectList")]
public class GlobalGameObjectList : ScriptableObject
{
    public List<GameObject> Value;
}
