using TMPro;
using UnityEngine;

public class TimeDisplayer : MonoBehaviour
{
    public GlobalFloat CurrentTimeToVictory;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int seconds = ((int)CurrentTimeToVictory.Value % 60);
        int minutes = ((int)CurrentTimeToVictory.Value / 60);
        GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
        // TODO: Do something when time is ending
    }
}
