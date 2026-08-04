using UnityEngine;

namespace Fan.UI
{
    /// <summary>
    /// Calcula el estado visual de la barra a partir de un valor 0-1.
    /// Sin dependencias de MonoBehaviour/escena -> testeable con NUnit puro.
    /// </summary>
    public struct StaminaVisualState
    {
        public Color FillColor;
        public float BladeSpinSpeed;   // grados/seg del ícono de aspa
        public bool IsCharged;         // true al llegar al umbral de "ráfaga lista"
        public float PunchIntensity;   // fuerza del squash en este pulso

        private const float ChargedThreshold = 0.95f;

        public static StaminaVisualState Evaluate(float normalizedValue, Gradient fillGradient)
        {
            normalizedValue = Mathf.Clamp01(normalizedValue);

            return new StaminaVisualState
            {
                FillColor = fillGradient.Evaluate(normalizedValue),
                BladeSpinSpeed = Mathf.Lerp(60f, 720f, normalizedValue),
                IsCharged = normalizedValue >= ChargedThreshold,
                PunchIntensity = Mathf.Lerp(0.03f, 0.12f, normalizedValue)
            };
        }
    }
}