using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fan.Core
{
    /// <summary>
    /// Singleton persistente. Centraliza el avance de nivel y la salida del juego
    /// para que otros sistemas (Ventilador, Menú, etc.) no dependan de SceneManager directamente.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>Carga la siguiente escena según el orden definido en Build Settings.</summary>
        public void CargarSiguienteNivel()
        {
            int siguienteIndice = SceneManager.GetActiveScene().buildIndex + 1;

            if (siguienteIndice < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(siguienteIndice);
            else
                Debug.LogWarning("[GameManager] No hay más niveles configurados en Build Settings.");
        }

        public void CargarEscena(string nombreEscena) => SceneManager.LoadScene(nombreEscena);

        public void SalirDelJuego()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
