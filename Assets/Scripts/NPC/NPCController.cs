using UnityEngine;
using UnityEngine.UI;

public class NPCController : MonoBehaviour
{
    public GlobalNPCController GlobalNPCController;
    public GameEvent OnNPCEvacuated;
    public GameEvent OnNPCDestroyed;

    public int SpawnedIndex;

    public int NPCurrentHealth;
    public float NPCCurrentTimeToEvacuate;
    public float NPCTotalTimeToEvacuate;

    public Image FillBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NPCCurrentTimeToEvacuate = NPCTotalTimeToEvacuate;
    }

    // Update is called once per frame
    void Update()
    {
        NPCCurrentTimeToEvacuate = Mathf.Max(0, NPCCurrentTimeToEvacuate-Time.deltaTime);
        FillBar.fillAmount = NPCCurrentTimeToEvacuate;
        if (NPCCurrentTimeToEvacuate == 0)
        {
            OnNPCEvacuated.Raise();
        }
        FillBar.fillAmount = 1 - NPCCurrentTimeToEvacuate / NPCTotalTimeToEvacuate;
    }
}
