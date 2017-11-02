using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioAdjuster : MonoBehaviour
{
    [SerializeField]
    private List<AdjustmentInfo> adjustmentInfo;
    private float ratioDifference;

    private void Start()
    {
        float cameraAspect = GameManager.Instance.orthographicCamera.GetComponent<Camera>().aspect;

        for (int i = 0; i < adjustmentInfo.Count; i++)
        {
            ratioDifference = Mathf.Abs(cameraAspect - adjustmentInfo[i].aspect);

            if (ratioDifference <= 0.001f)
            {
                transform.localScale = adjustmentInfo[i].scale;
                transform.position = adjustmentInfo[i].position;
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
