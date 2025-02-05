
using UnityEditor;

namespace LightingEffect.Editor
{
    [CustomEditor(typeof(LightingEffectLifetimeScope))]
    public class LightingEffectLifetimeScopeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Dependencies: [LightingEffector]", MessageType.Info);
        }
    }
}