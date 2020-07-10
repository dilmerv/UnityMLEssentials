using System.Collections;
using Unity.MLAgents;
using UnityEngine;


public class BaseAgent : Agent
{
    [SerializeField]
    protected MeshRenderer groundMeshRenderer;

    [SerializeField]
    protected Material successMaterial;

    [SerializeField]
    protected Material failureMaterial;

    [SerializeField]
    protected Material defaultMaterial;

    protected IEnumerator SwapGroundMaterial(Material mat, float time)
    {
        groundMeshRenderer.material = mat;
        yield return new WaitForSeconds(time);
        groundMeshRenderer.material = defaultMaterial;
    }
}
