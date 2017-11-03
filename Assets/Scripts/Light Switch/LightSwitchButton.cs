using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchButton : MonoBehaviour
{
    [Header("Skyboxes")]
    [SerializeField] private Material bedroomLightsOff;
    [SerializeField] private Material bedroomLightsOn;
    private MessageSender2D messageSender2D;
    private bool isLightSwitchFixed;
    private bool areLightsOn;

    public void FixLightSwitch()
    {
        isLightSwitchFixed = true;
        messageSender2D.enabled = false;
    }

    private void Awake()
    {
        messageSender2D = GetComponent<MessageSender2D>();
    }

    private void OnMouseDown()
    {
        if (isLightSwitchFixed)
        {
            areLightsOn = !areLightsOn;
            RenderSettings.skybox = areLightsOn ? bedroomLightsOn : bedroomLightsOff;
        }
    }
}
