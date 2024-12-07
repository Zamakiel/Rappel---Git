using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public GlobalInt CurrentHealth;
    public GlobalInt MaxHealth;

    public GameEvent OnGameOverEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentHealth.Value = MaxHealth.Value;
    }

    void TakeDamage(int damage)
    {
        CurrentHealth.Value = Mathf.Max(0, CurrentHealth.Value-damage);
        if (CurrentHealth.Value == 0)
        {
            // Game over
            OnGameOverEvent.Raise();
        }        
    }
}
