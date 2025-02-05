
using UnityEditor;

namespace LightingEffect.Editor
{
    [CustomEditor(typeof(LightingEffectView))]
    public class LightingEffectViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Dependencies: [LightingEffector]", MessageType.Info);
        }
    }
}