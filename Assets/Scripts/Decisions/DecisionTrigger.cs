using UnityEngine;
using UnityEngine.UI;
using Fan.Events;

namespace Fan.Decisions
{
    /// <summary>
    /// Muestra un botón central; al presionarlo aparecen los botones Opción A y Opción B.
    /// Al elegir una opción, emite el evento al canal sin saber nada de escenas.
    /// </summary>
    public class DecisionTrigger : MonoBehaviour
    {
        [Header("Event Channel")]
        [SerializeField] private DecisionEventChannelSO _channel;

        [Header("UI References")]
        [SerializeField] private GameObject _triggerButton;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private Button _optionA;
        [SerializeField] private Button _optionB;

        private void Awake()
        {
            _optionsPanel.SetActive(false);
            _optionA.onClick.AddListener(() => OnDecision(DecisionType.A));
            _optionB.onClick.AddListener(() => OnDecision(DecisionType.B));
        }

        private void OnDestroy()
        {
            _optionA.onClick.RemoveAllListeners();
            _optionB.onClick.RemoveAllListeners();
        }

        public void ShowOptions()
        {
            _triggerButton.SetActive(false);
            _optionsPanel.SetActive(true);
        }

        private void OnDecision(DecisionType decision)
        {
            _channel.Raise(decision);
        }
    }
}
