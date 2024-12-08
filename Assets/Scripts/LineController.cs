using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField]
    LineRenderer m_lineRenderer;

    private void Update()
    {
        m_lineRenderer.SetPosition(1, transform.InverseTransformPoint(WorldManagerScript.s_instance.m_craneAnchorPoint.transform.position));
    }

}
