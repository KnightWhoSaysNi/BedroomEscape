using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightSwitch
{
    public class LightToggle : MonoBehaviour
    {
        [SerializeField] private AudioClip lightSwitchAudio;
        [SerializeField, Range(0, 100)] private float audioVolumePercent;
        [Space(5)]
        [SerializeField] private Material lightsOffSkybox;
        [SerializeField] private Material lightsOnSkybox;
        [Space(5)]
        [SerializeField] private GameObject ceilingMessageSender;

        [Space(10)]
        [SerializeField] private List<LightDependentView> lightDependentViews;

        private bool isLightOn;

        public void ToggleLights()
        {
            isLightOn = !isLightOn;
            AudioManager.Instance.PlayAudioClip(lightSwitchAudio, audioVolumePercent);
            RenderSettings.skybox = isLightOn ? lightsOnSkybox : lightsOffSkybox;
            ceilingMessageSender.SetActive(isLightOn);

            for (int i = 0; i < lightDependentViews.Count; i++)
            {
                lightDependentViews[i].RegisterLightChange(isLightOn);
            }
        }
    }    
}
