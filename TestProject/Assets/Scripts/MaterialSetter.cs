using UnityEngine;
using Utility;

public class MaterialSetter : MonoBehaviourLogger
{
    [SerializeField] private MeshRenderer[] meshRenderers = new MeshRenderer[0];
    [SerializeField] private Material[] materials = new Material[0];

    public void SetMaterial(int index)
    {
        if (index >= 0 && materials.Length > index)
        {
            foreach (MeshRenderer mesh in meshRenderers)
            {
                mesh.material = materials[index];
            }
        }
        else
        {
            LogError("Out of range materials array!");
        }
    }

    private void Awake()
    {
        SetLogPrefix(nameof(MaterialSetter));
    }
}
