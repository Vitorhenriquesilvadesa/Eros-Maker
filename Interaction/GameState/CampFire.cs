using System;
using Interactive;
using UnityEngine;

namespace Interaction.GameState
{
    public class CampFire : InteractiveObject
    {
        [SerializeField] private GameObject campFireEffect;
        [SerializeField] private Light campFireGlow;
        private bool _isEffectEnabled;

        private float _glowIntensity;
        private const float MaxGlowIntensity = 3f;

        private void Update()
        {
            if (_isEffectEnabled && _glowIntensity < MaxGlowIntensity)
            {
                _glowIntensity += Time.deltaTime;
            }

            if (!_isEffectEnabled && _glowIntensity > 0f)
            {
                _glowIntensity -= Time.deltaTime * 9f;
            }

            campFireGlow.intensity = _glowIntensity;
        }

        public override void BeforeInteraction()
        {
        }

        public override void OnInteract()
        {
            _isEffectEnabled = !_isEffectEnabled;
            campFireEffect.SetActive(_isEffectEnabled);
        }

        public override void AfterInteraction()
        {
        }

        public override void OnInteractionRayCastEnter()
        {
        }

        public override void OnInteractionRayCastExit()
        {
        }
    }
}
