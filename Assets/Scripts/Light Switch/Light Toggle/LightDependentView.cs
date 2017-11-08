using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightSwitch
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LightDependentView : MonoBehaviour
    {
        [SerializeField] private Sprite lightsOffSprite;
        [SerializeField] private Sprite lightsOnSprite;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = LightToggle.IsLightOn ? lightsOnSprite : lightsOffSprite;

            LightToggle.LightsToggled += OnLightToggled;
        }

        private void OnLightToggled(bool isLightOn)
        {
            spriteRenderer.sprite = isLightOn ? lightsOnSprite : lightsOffSprite;
        }
    }
}
