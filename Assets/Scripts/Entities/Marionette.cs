using UnityEngine;
using System.Collections;

public class Marionette : MonoBehaviour
{
    [SerializeField]
    private Rigidbody body;
    [SerializeField]
    private SpringJoint[] limbCables;

    public uint LimbNb
    {
        get
        {
            return (uint)limbCables.Length;
        }
    }

	public void Initialize(Vector3 mainCableParams, Vector4[] limbCablesParams)
    {
        SpringJoint[] mainCables = body.GetComponents<SpringJoint>();
        foreach (SpringJoint mainCable in mainCables)
        {
            mainCable.spring = mainCableParams.x;
            mainCable.connectedAnchor += new Vector3(mainCableParams.y, 0f, mainCableParams.z);
        }

        for (int i = 0; i < limbCablesParams.Length && i < LimbNb; ++i)
        {
            limbCables[i].spring = limbCablesParams[i].x;
            limbCables[i].connectedAnchor += new Vector3(limbCablesParams[i].y, limbCablesParams[i].z, limbCablesParams[i].w);
        }
    }
}
