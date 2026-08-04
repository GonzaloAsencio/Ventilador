using UnityEngine;
using UnityEngine.UI;

namespace Fan.UI
{
    /// <summary>
    /// Barra de fuerza del Modo Elección. No conoce reglas de juego:
    /// solo muestra/oculta y actualiza un valor 0-1. VentiladorController la orquesta.
    /// </summary>
    public class WindBarUI : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private GameObject _contenedor;
        [Tooltip("Slider con Min Value = 0 y Max Value = 1.")]
        [SerializeField] private Slider _slider;

        [Header("Estilo visual")]
        [Tooltip("Image del fill del slider (Slider > Fill Area > Fill).")]
        [SerializeField] private Image _fillImage;
        [Tooltip("RectTransform sobre el que se aplica el punch/squash. Puede ser el mismo GO del fill.")]
        [SerializeField] private RectTransform _fillRoot;
        [Tooltip("Opcional: ícono de aspa que gira más rápido a mayor carga.")]
        [SerializeField] private RectTransform _bladeIcon;
        [Tooltip("Opcional: partículas de ráfaga al llegar al 100%.")]
        [SerializeField] private ParticleSystem _windBurstFX;
        [SerializeField] private Gradient _fillGradient;
        [SerializeField] private AnimationCurve _punchCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        [SerializeField] private float _punchDuration = 0.12f;

        private float _valorAnterior;
        private bool _yaCargada;
        private Coroutine _punchRoutine;

        private void Awake()
        {
            if (_contenedor != null)
                _contenedor.SetActive(false);
        }

        public void Show()
        {
            if (_contenedor != null)
                _contenedor.SetActive(true);

            // Reset defensivo: por si se reentra a Modo Elección con la barra
            // en un estado visual "cargado" de la vez anterior.
            _valorAnterior = 0f;
            _yaCargada = false;
        }

        public void Hide()
        {
            if (_contenedor != null)
                _contenedor.SetActive(false);
        }

        public void SetFuerza(float valor01)
        {
            valor01 = Mathf.Clamp01(valor01);

            if (_slider != null)
                _slider.value = valor01;

            var estado = StaminaVisualState.Evaluate(valor01, _fillGradient);

            if (_fillImage != null)
                _fillImage.color = estado.FillColor;

            if (_bladeIcon != null)
                _bladeIcon.Rotate(Vector3.forward, -estado.BladeSpinSpeed * Time.deltaTime);

            // Solo dispara el punch cuando el valor sube (click efectivo),
            // no en cada frame ni cuando decae.
            if (valor01 > _valorAnterior && _fillRoot != null && _punchRoutine == null)
                _punchRoutine = StartCoroutine(PunchRoutine(estado.PunchIntensity));

            if (estado.IsCharged && !_yaCargada)
                _windBurstFX?.Play();

            _yaCargada = estado.IsCharged;
            _valorAnterior = valor01;
        }

        private System.Collections.IEnumerator PunchRoutine(float intensity)
        {
            float t = 0f;
            Vector3 baseScale = Vector3.one;

            while (t < _punchDuration)
            {
                t += Time.deltaTime;
                float curveValue = _punchCurve.Evaluate(t / _punchDuration);
                _fillRoot.localScale = baseScale + Vector3.one * (intensity * curveValue);
                yield return null;
            }

            _fillRoot.localScale = baseScale;
            _punchRoutine = null;
        }
    }
}
