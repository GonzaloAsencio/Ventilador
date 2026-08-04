using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Fan.Dialogue
{
    /// <summary>
    /// Singleton simple para mostrar subtítulos/diálogo en pantalla.
    /// Cualquier evento del juego puede llamar a MostrarDialogo(texto, duracion).
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("Referencias UI")]
        [Tooltip("Panel con fondo oscuro semitransparente.")]
        [SerializeField] private GameObject _panel;
        [Tooltip("Texto blanco dentro del panel.")]
        [SerializeField] private Text _texto;

        private Coroutine _rutinaActual;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (_panel != null)
                _panel.SetActive(false);
        }

        /// <summary>Muestra el diálogo y lo oculta solo tras 'duracion' segundos.</summary>
        public void MostrarDialogo(string texto, float duracion)
        {
            if (_rutinaActual != null)
                StopCoroutine(_rutinaActual);

            _rutinaActual = StartCoroutine(RutinaMostrarDialogo(texto, duracion));
        }

        private IEnumerator RutinaMostrarDialogo(string texto, float duracion)
        {
            if (_texto != null) _texto.text = texto;
            if (_panel != null) _panel.SetActive(true);

            yield return new WaitForSeconds(duracion);

            if (_panel != null) _panel.SetActive(false);
            _rutinaActual = null;
        }
    }
}
