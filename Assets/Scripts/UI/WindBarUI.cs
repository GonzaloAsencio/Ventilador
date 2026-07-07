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

        private void Awake()
        {
            if (_contenedor != null)
                _contenedor.SetActive(false);
        }

        public void Show()
        {
            if (_contenedor != null)
                _contenedor.SetActive(true);
        }

        public void Hide()
        {
            if (_contenedor != null)
                _contenedor.SetActive(false);
        }

        public void SetFuerza(float valor01)
        {
            if (_slider != null)
                _slider.value = Mathf.Clamp01(valor01);
        }
    }
}
