using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GlobalScene", menuName = "Scriptable Objects/GlobalScene")]
public class GlobalScene : ScriptableObject
{
    public Object Value;

    public void SetValue(Object obj)
    {
        Value = obj;
    }
}
