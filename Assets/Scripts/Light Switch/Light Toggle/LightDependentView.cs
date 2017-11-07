using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightSwitch
{
    [System.Serializable]
    public class LightDependentView
    {
        [SerializeField] private string name;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite lightsOffSprite;
        [SerializeField] private Sprite lightsOnSprite;

        public void RegisterLightChange(bool isLightOn)
        {
            spriteRenderer.sprite = isLightOn ? lightsOnSprite : lightsOffSprite;
        }
    }
}
