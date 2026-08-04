using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Fan.Core;
using Fan.UI;

namespace Fan.Ventilador
{
    public enum EstadoVentilador
    {
        Oscilando,
        PausaExtremo,
        ModoEleccion
    }

    public class VentiladorController : MonoBehaviour
    {
        [Header("Oscilación")]
        [SerializeField] private float _anguloMaximo = 45f;
        [SerializeField] private float _velocidadGrados = 60f;
        [SerializeField] private float _tiempoPausaExtremos = 0.3f;

        [Header("Modo Elección")]
        [SerializeField] private Key _teclaElegir = Key.E;
        [SerializeField] private Key _teclaCargar = Key.Space;
        [SerializeField] private float _incrementoPorPulsacion = 0.15f;
        [Tooltip("Cuánto baja la barra por segundo si no se presiona. 0 = no decae.")]
        [SerializeField] private float _decaimientoPorSegundo = 0f;

        [Header("Referencias")]
        [SerializeField] private WindBarUI _windBarUI;

        [Header("Evento al 100%")]
        [SerializeField] private UnityEvent _onFuerzaCompleta;
        [SerializeField] private float _delayAntesDeSiguienteNivel = 1.5f;

        private EstadoVentilador _estado = EstadoVentilador.Oscilando;
        private float _direccion = 1f;
        private float _anguloActual;
        private float _timerPausa;
        private float _fuerzaActual;
        private bool _fuerzaCompletada;
        private Vector3 _rotacionBase;

        private void Awake()
        {
            _rotacionBase = transform.localRotation.eulerAngles;
        }

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current[_teclaElegir].wasPressedThisFrame && _estado != EstadoVentilador.ModoEleccion)
            {
                EntrarModoEleccion();
                return;
            }

            switch (_estado)
            {
                case EstadoVentilador.Oscilando:
                    ActualizarOscilacion();
                    break;
                case EstadoVentilador.PausaExtremo:
                    ActualizarPausa();
                    break;
                case EstadoVentilador.ModoEleccion:
                    ActualizarModoEleccion();
                    break;
            }
        }

        private void ActualizarOscilacion()
        {
            _anguloActual += _velocidadGrados * _direccion * Time.deltaTime;
            _anguloActual = Mathf.Clamp(_anguloActual, -_anguloMaximo, _anguloMaximo);
            transform.localRotation = Quaternion.Euler(_rotacionBase.x, _rotacionBase.y, _anguloActual);
            if (Mathf.Abs(_anguloActual) >= _anguloMaximo)
            {
                _estado = EstadoVentilador.PausaExtremo;
                _timerPausa = _tiempoPausaExtremos;
            }
        }

        private void ActualizarPausa()
        {
            _timerPausa -= Time.deltaTime;
            if (_timerPausa <= 0f)
            {
                _direccion *= -1f;
                _estado = EstadoVentilador.Oscilando;
            }
        }

        private void EntrarModoEleccion()
        {
            _estado = EstadoVentilador.ModoEleccion;
            _fuerzaActual = 0f;
            _fuerzaCompletada = false;

            if (_windBarUI != null)
            {
                _windBarUI.Show();
                _windBarUI.SetFuerza(0f);
            }
        }

        private void ActualizarModoEleccion()
        {
            if (_fuerzaCompletada) return;

            if (_decaimientoPorSegundo > 0f)
                _fuerzaActual -= _decaimientoPorSegundo * Time.deltaTime;

            if (Keyboard.current != null && Keyboard.current[_teclaCargar].wasPressedThisFrame)
                _fuerzaActual += _incrementoPorPulsacion;

            _fuerzaActual = Mathf.Clamp01(_fuerzaActual);

            if (_windBarUI != null)
                _windBarUI.SetFuerza(_fuerzaActual);

            if (_fuerzaActual >= 1f)
            {
                _fuerzaCompletada = true;
                StartCoroutine(CompletarFuerza());
            }
        }

        private IEnumerator CompletarFuerza()
        {
            _onFuerzaCompleta?.Invoke();

            yield return null;
        }
    }
}
