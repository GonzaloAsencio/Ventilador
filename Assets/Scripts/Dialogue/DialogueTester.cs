using UnityEngine;
using UnityEngine.InputSystem;

namespace Fan.Dialogue
{
    /// <summary>
    /// Script temporal para probar el DialogueManager. Apretá T en Play Mode.
    /// Sacalo de la escena cuando termines de testear.
    /// </summary>
    public class DialogueTester : MonoBehaviour
    {
        [SerializeField] private Key _teclaTest = Key.T;
        [SerializeField] private string _texto = "Esto es una prueba de diálogo en Screen Space - Camera.";
        [SerializeField] private float _duracion = 3f;

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current[_teclaTest].wasPressedThisFrame)
            {
                if (DialogueManager.Instance != null)
                    DialogueManager.Instance.MostrarDialogo(_texto, _duracion);
                else
                    Debug.LogWarning("[DialogueTester] No hay DialogueManager en escena.");
            }
        }
    }
}
