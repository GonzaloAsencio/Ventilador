using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fan.Decisions;
using Fan.Events;

namespace Fan.Level
{
    /// <summary>
    /// Suscriptor del canal de decisiones. Su única responsabilidad es cargar la escena
    /// que corresponde a la decisión recibida. 
    /// </summary>
    public class SceneTransitionHandler : MonoBehaviour
    {
        [Serializable]
        private struct SceneMapping
        {
            public DecisionType Decision;

#if UNITY_EDITOR
            public UnityEditor.SceneAsset Scene;
#endif
            [HideInInspector] public string SceneName;
        }

        [Header("Canal de eventos")]
        [SerializeField] private DecisionEventChannelSO _channel;

        [Header("Decision Map to Scene")]
        [SerializeField] private List<SceneMapping> _sceneMappings;

        private Dictionary<DecisionType, string> _sceneMap;

        private void OnValidate()
        {
#if UNITY_EDITOR
            for (int i = 0; i < _sceneMappings.Count; i++)
            {
                var m = _sceneMappings[i];
                if (m.Scene != null)
                    m.SceneName = m.Scene.name;
                _sceneMappings[i] = m;
            }
#endif
        }

        private void Awake()
        {
            _sceneMap = new Dictionary<DecisionType, string>(_sceneMappings.Count);
            foreach (var mapping in _sceneMappings)
                _sceneMap[mapping.Decision] = mapping.SceneName;
        }

        private void OnEnable()  => _channel.Register(OnDecisionTaken);
        private void OnDisable() => _channel.Unregister(OnDecisionTaken);

        private void OnDecisionTaken(DecisionType decision)
        {
            if (_sceneMap.TryGetValue(decision, out string sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning(
                    $"[SceneTransitionHandler] No hay escena mapeada para la decisión: {decision}");
            }
        }
    }
}
