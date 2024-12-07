using TMPro;
using UnityEngine;

public class BuffDisplayer : MonoBehaviour
{
    public GlobalBuffList GlobalBuffList;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string text = "Active buffs:\n";
        string.Format(text, GlobalBuffList);
        foreach (Buff buff in GlobalBuffList.Value)
        {
            text += buff.Name + ": " + string.Format("{0:0.00}", buff.CurrentBuffDuration) + "\n";
        }
        GetComponent<TextMeshProUGUI>().text = text;
    }
}
