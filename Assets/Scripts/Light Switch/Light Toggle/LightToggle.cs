using System;
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
        private static bool isLightOn;

        public static event Action<bool> LightsToggled;

        public static bool IsLightOn
        {
            get
            {
                return isLightOn;
            }
        }

        public void ToggleLights()
        {
            isLightOn = !isLightOn;
            AudioManager.Instance.PlayAudioClip(lightSwitchAudio, audioVolumePercent);
            RenderSettings.skybox = isLightOn ? lightsOnSkybox : lightsOffSkybox;
            ceilingMessageSender.SetActive(isLightOn);

            if (LightsToggled != null)
            {
                LightsToggled(isLightOn);
            }
        }
    }    
}
