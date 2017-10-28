using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioAdjuster : MonoBehaviour
{
    [SerializeField]
    private List<AdjustmentInfo> adjustmentInfo;
    [Space(5)]
    [SerializeField]
    private GameObject adjustableObject; // TODO Use transform if there's just one game object here

    private void Awake()
    {
        float cameraAspect = Camera.main.aspect;

        for (int i = 0; i < adjustmentInfo.Count; i++)
        {
            if (cameraAspect == adjustmentInfo[i].aspect)
            {
                adjustableObject.transform.position = adjustmentInfo[i].position;
                adjustableObject.transform.localScale = adjustmentInfo[i].scale;
                break;
            }
        }
    }
}

[System.Serializable]
public struct AdjustmentInfo
{
    public string aspectString;
    public float aspect;
    public Vector3 position;
    public Vector3 scale;
}
