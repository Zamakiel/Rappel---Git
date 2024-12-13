using UnityEngine;
using UnityEngine.UI;

public class NPCEvacuationDisplayer : MonoBehaviour
{
    public Image ImageToFill;
    
    public void SetImageFillRatio(float ratio)
    {
        ImageToFill.fillAmount = ratio;
    }

    public void CalculateAndFillRation(float currentValue, float totalValue)
    {
        SetImageFillRatio(totalValue / currentValue);
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
