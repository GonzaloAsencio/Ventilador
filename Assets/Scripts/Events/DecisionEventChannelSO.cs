using UnityEngine;
using UnityEngine.Events;
using Fan.Decisions;

namespace Fan.Events
{
    [CreateAssetMenu(
        fileName = "DecisionEventChannel",
        menuName = "Fan/Decision Event Channel")]
    public class DecisionEventChannelSO : ScriptableObject
    {
        private UnityAction<DecisionType> _onDecisionTaken;

        /// <summary>Emite el evento. Llamar desde DecisionTrigger.</summary>
        public void Raise(DecisionType decision) =>
            _onDecisionTaken?.Invoke(decision);

        /// <summary>Registra un listener. Llamar en OnEnable del suscriptor.</summary>
        public void Register(UnityAction<DecisionType> listener) =>
            _onDecisionTaken += listener;

        /// <summary>Desregistra un listener. Llamar en OnDisable del suscriptor.</summary>
        public void Unregister(UnityAction<DecisionType> listener) =>
            _onDecisionTaken -= listener;
    }
}
