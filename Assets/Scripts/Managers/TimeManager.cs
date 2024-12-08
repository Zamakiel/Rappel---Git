using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GlobalFloat CurrentTimeToVictory;
    public GlobalFloat TotalTimeToVictory;

    public GameEvent OnVictoryEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentTimeToVictory.Value = TotalTimeToVictory.Value;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTimeToVictory.Value = Mathf.Max(0, CurrentTimeToVictory.Value - Time.deltaTime);
        if (CurrentTimeToVictory.Value == 0)
        {
            OnVictoryEvent.Raise();
            enabled = false;
        }
    }
}
