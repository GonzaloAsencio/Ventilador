using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fan.Audio;
using TMPro;

namespace Fan.Menu
{
    /// <summary>
    /// Controlador del Canvas del menú principal: Iniciar, Opciones (Sonido/Idioma) y Salir.
    /// Las funciones de Sonido/Idioma quedan como placeholders lógicos hasta tener
    /// un sistema de opciones/localización real.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Header("Escena de juego")]
        [SerializeField] private string _nombreEscenaJuego = "SampleScene";

        [Header("Paneles")]
        [SerializeField] private GameObject _panelOpciones;

        [Header("Opciones - Sonido")]
        [SerializeField] private Slider _sliderVolumen;

        [Header("Opciones - Idioma")]
        [SerializeField] private TMP_Dropdown _dropdownIdioma;

        private void Awake()
        {
            if (_panelOpciones != null)
                _panelOpciones.SetActive(false);
        }

        public void Iniciar() => SceneManager.LoadScene(_nombreEscenaJuego);

        public void AbrirOpciones()
        {
            if (_panelOpciones != null)
                _panelOpciones.SetActive(true);
        }

        public void CerrarOpciones()
        {
            if (_panelOpciones != null)
                _panelOpciones.SetActive(false);
        }

        public void Salir()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>Enganchar al evento OnValueChanged del slider de volumen.</summary>
        public void OnVolumenCambiado(float valor01)
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.SetVolumenGeneral(valor01);
        }

        /// <summary>Enganchar al evento OnValueChanged del dropdown de idioma.</summary>
        public void OnIdiomaCambiado(int indice)
        {
            // Placeholder: conectar a un futuro sistema de localización.
            Debug.Log($"[MainMenuController] Idioma seleccionado: índice {indice}");
        }
    }
}
