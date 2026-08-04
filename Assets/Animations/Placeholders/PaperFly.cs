using System.Collections;
using UnityEngine;

namespace Fan.Decisions
{
    /// <summary>
    /// Mueve el papel desde su posición actual hasta un punto de destino
    /// (la ventana), simulando que se lo lleva el viento. Movimiento simple
    /// por curva, sin física ni Animator.
    /// </summary>
    public class PaperFlyAwayAnimation : MonoBehaviour, IOutcomeAnimation
    {
        [Header("Destino")]
        [Tooltip("Punto de mundo hacia donde vuela el papel (ej. un empty en la ventana).")]
        [SerializeField] private Transform _destino;

        [Header("Timing")]
        [SerializeField] private float _duracion = 0.8f;
        [SerializeField] private AnimationCurve _curva = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Extra (opcional)")]
        [Tooltip("Rotación total (grados) durante el vuelo, para que no se vea rígido.")]
        [SerializeField] private float _rotacionDuranteVuelo = 180f;

        private Vector3 _posicionInicial;
        private Quaternion _rotacionInicial;

        private void Awake()
        {
            _posicionInicial = transform.position;
            _rotacionInicial = transform.rotation;
        }

        public void Play()
        {
            if (_destino == null)
            {
                Debug.LogWarning($"[PaperFlyAwayAnimation] {name} no tiene destino asignado.");
                return;
            }

            StopAllCoroutines();
            StartCoroutine(Volar());
        }

        private IEnumerator Volar()
        {
            float t = 0f;

            while (t < _duracion)
            {
                t += Time.deltaTime;
                float progreso = _curva.Evaluate(Mathf.Clamp01(t / _duracion));

                transform.position = Vector3.Lerp(_posicionInicial, _destino.position, progreso);
                transform.rotation = _rotacionInicial * Quaternion.Euler(0f, 0f, _rotacionDuranteVuelo * progreso);

                yield return null;
            }

            transform.position = _destino.position;
        }
    }
}