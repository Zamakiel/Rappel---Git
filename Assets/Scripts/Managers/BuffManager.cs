using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public GlobalBuffList ActiveBuffs;

    public GlobalBuff StartedGlobalBuff;
    public GlobalBuff RemovedGlobalBuff;
    public GameEventListener BuffEventListener;

    public GameEvent OnBuffEndedEvent;

    public void OnBuffStarted()
    {
        Buff startedGlobalBuff = StartedGlobalBuff.Value;
        if (ActiveBuffs.Value.Contains(startedGlobalBuff))
        {
            ActiveBuffs.Value.Remove(startedGlobalBuff);
        }        
        if (startedGlobalBuff.TotalBuffDuration > 0)
        {
            startedGlobalBuff.CurrentBuffDuration = startedGlobalBuff.TotalBuffDuration;
            ActiveBuffs.Value.Add(StartedGlobalBuff.Value);
        }        
    }

    private void Awake()
    {
        ActiveBuffs.Value.Clear();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = ActiveBuffs.Value.Count - 1; i >= 0; i--)
        {
            Buff buff = ActiveBuffs.Value[i];
            buff.CurrentBuffDuration -= Time.deltaTime;
            if (buff.CurrentBuffDuration <= 0)
            {
                RemovedGlobalBuff.Value = buff;
                OnBuffEndedEvent.Raise();
                ActiveBuffs.Value.Remove(buff);
            }            
        }        
    }
}
