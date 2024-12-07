using UnityEngine;
using TMPro;

public class BuffInstance : MonoBehaviour
{
    public Buff ReferenceBuff;

    public GlobalBuff StartedGlobalBuff;
    public GameEvent OnBuffStartedEvent;

    public TextMeshPro Text;



    public void OnBuffPicked()
    {
        StartedGlobalBuff.Value = ReferenceBuff;
        OnBuffStartedEvent.Raise();        
        // TODO: Destoy sequence
        Destroy(gameObject);
    }

    void OnMouseDown()
    {
        OnBuffPicked();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Text.text = ReferenceBuff.Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
