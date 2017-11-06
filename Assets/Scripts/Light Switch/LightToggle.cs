using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightSwitch
{
    public abstract class LightToggle : MonoBehaviour
    {
        protected static bool areLightsOn;

        [SerializeField] protected AudioClip lightSwitchAudio;
        [SerializeField, Range(0, 100)] protected float audioVolumePercent;
        [Space(5)]
        [SerializeField] protected Material lightsOffSkybox;
        [SerializeField] protected Material lightsOnSkybox;
        [Space(5)]
        [SerializeField]
        protected GameObject ceilingMessageSender;

        protected void ToggleLights()
        {
            areLightsOn = !areLightsOn;
            AudioManager.Instance.PlayAudioClip(lightSwitchAudio, audioVolumePercent);
            RenderSettings.skybox = areLightsOn ? lightsOnSkybox : lightsOffSkybox;
            ceilingMessageSender.SetActive(areLightsOn);
        }
    }
}
