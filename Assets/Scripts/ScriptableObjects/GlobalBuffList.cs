using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalBuffList", menuName = "Scriptable Objects/GlobalBuffList")]
public class GlobalBuffList : ScriptableObject
{
    public List<Buff> Value;
}
