using UnityEngine;

public class LineController : MonoBehaviour 
{
    [SerializeField]
    Vector3 m_cranePosition = new Vector3(-0.15f, 2.1f);

    [SerializeField]
    LineRenderer m_lineRenderer;

    private void Update()
    {
        m_lineRenderer.SetPosition(1, transform.InverseTransformPoint(m_cranePosition));
    }

}
