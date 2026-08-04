using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fan.Events;

namespace Fan.Decisions
{
    /// <summary>
    /// Orquesta la secuencia posterior a la carga completa del ventilador:
    /// reproduce la animación de consecuencia de la decisión elegida (papel volando /
    /// botella cayendo), espera un tiempo configurable, y recién ahí confirma la
    /// decisión para que SceneTransitionHandler cargue la escena correspondiente.
    /// No conoce nombres de escena: esa responsabilidad sigue siendo de SceneTransitionHandler.
    /// </summary>
    public class DecisionOutcomeSequencer : MonoBehaviour
    {
        [Serializable]
        private struct OutcomeMapping
        {
            public DecisionType Decision;
            [Tooltip("Objeto con Animator que reproduce la consecuencia (papel, botella, etc.)")]
            public GameObject OutcomeObject;
        }

        [Header("Canales de eventos")]
        [Tooltip("El mismo canal que usa DecisionTrigger al elegir A/B.")]
        [SerializeField] private DecisionEventChannelSO _canalSeleccion;
        [Tooltip("El mismo canal que ahora escucha SceneTransitionHandler.")]
        [SerializeField] private DecisionEventChannelSO _canalConfirmacion;

        [Header("Mapeo Decisión -> Objeto de consecuencia")]
        [SerializeField] private List<OutcomeMapping> _outcomes;

        [Header("Timing")]
        [Tooltip("Segundos entre el fin de la animación de consecuencia y el cambio de escena.")]
        [SerializeField] private float _tiempoEsperaAntesDeTransicion = 1.5f;

        private Dictionary<DecisionType, GameObject> _outcomeMap;
        private DecisionType? _decisionPendiente;

        private void Awake()
        {
            _outcomeMap = new Dictionary<DecisionType, GameObject>(_outcomes.Count);
            foreach (var mapping in _outcomes)
                _outcomeMap[mapping.Decision] = mapping.OutcomeObject;
        }

        private void OnEnable() => _canalSeleccion.Register(GuardarDecisionPendiente);
        private void OnDisable() => _canalSeleccion.Unregister(GuardarDecisionPendiente);

        private void GuardarDecisionPendiente(DecisionType decision)
        {
            _decisionPendiente = decision;
        }

        /// <summary>
        /// Enganchar en el Inspector al evento _onFuerzaCompleta de VentiladorController.
        /// </summary>
        public void OnFuerzaCompletada()
        {
            if (_decisionPendiente == null)
            {
                Debug.LogWarning("[DecisionOutcomeSequencer] Fuerza completada sin decisión previa registrada.");
                return;
            }

            StartCoroutine(SecuenciaDeConsecuencia(_decisionPendiente.Value));
        }

        private IEnumerator SecuenciaDeConsecuencia(DecisionType decision)
        {
            if (_outcomeMap.TryGetValue(decision, out GameObject outcomeObject) && outcomeObject != null)
            {
                var animacion = outcomeObject.GetComponent<IOutcomeAnimation>();
                if (animacion != null)
                    animacion.Play();
                else
                    Debug.LogWarning($"[DecisionOutcomeSequencer] {outcomeObject.name} no tiene un componente IOutcomeAnimation.");
            }

            yield return new WaitForSeconds(_tiempoEsperaAntesDeTransicion);

            _decisionPendiente = null;
            _canalConfirmacion.Raise(decision);
        }
    }
}