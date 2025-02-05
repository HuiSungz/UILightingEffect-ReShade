
using UnityEditor;
using UnityEngine;

namespace LightingEffect.Editor
{
    [CustomEditor(typeof(LightingEffector))]
    public class LightingEffectorEditor : UnityEditor.Editor
    {
        private static Texture2D _iconTexture;
        private static string IconPath => "LightingEffectorIcon";
        
        private SerializedProperty _speed;
        private SerializedProperty _cooldownTime;
        private SerializedProperty _color;
        private SerializedProperty _width;
        private SerializedProperty _intensity;
        private SerializedProperty _angle;
        private SerializedProperty _useChildApply;
        private SerializedProperty _playOnStart;

        private void OnEnable()
        {
            LoadIcon();
            SetComponentIcon();
            
            _speed = serializedObject.FindProperty("Speed");
            _cooldownTime = serializedObject.FindProperty("CooldownTime");
            _color = serializedObject.FindProperty("Color");
            _width = serializedObject.FindProperty("Width");
            _intensity = serializedObject.FindProperty("Intensity");
            _angle = serializedObject.FindProperty("Angle");
            _useChildApply = serializedObject.FindProperty("UseChildApply");
            _playOnStart = serializedObject.FindProperty("PlayOnStart");
        }
        
        private void LoadIcon()
        {
            if (!_iconTexture)
            {
                _iconTexture = Resources.Load<Texture2D>(IconPath);
            }
        }
        
        private void SetComponentIcon()
        {
            if (_iconTexture)
            {
                var script = MonoScript.FromMonoBehaviour((LightingEffector)target);
                EditorGUIUtility.SetIconForObject(script, _iconTexture);
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(10);
            
            // Title Header
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Lighting Effect Settings", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Configure the lighting effect parameters", EditorStyles.miniLabel);
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(10);

            // Animation Settings
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Animation Parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_speed, new GUIContent("Animation Speed", "Controls how fast the lighting effect moves"));
            EditorGUILayout.PropertyField(_cooldownTime, new GUIContent("Cooldown Time", "Time to wait before playing the effect again"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);

            // Visual Settings
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Visual Parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_color, new GUIContent("Light Color", "The color of the lighting effect"));
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_width, new GUIContent("Light Width", "Width of the lighting beam"));
            if (_width.floatValue < 0.01f || _width.floatValue > 1f)
            {
                EditorGUILayout.HelpBox("Width should be between 0.01 and 1", MessageType.Warning);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(_intensity, new GUIContent("Light Intensity", "Brightness of the lighting effect"));
            EditorGUILayout.PropertyField(_angle, new GUIContent("Light Angle", "Angle of the lighting beam (-89 to 89 degrees)"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);

            // Behavior Settings 부분만 수정
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Behavior Settings", EditorStyles.boldLabel);

            // Explain Box
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var explainStyle = new GUIStyle(EditorStyles.boldLabel);
            explainStyle.normal.textColor = new Color(0.8f, 0.8f, 0.16f); // 노란색
            EditorGUILayout.LabelField("Explain", explainStyle);

            var descriptionStyle = new GUIStyle(EditorStyles.label);
            descriptionStyle.wordWrap = true;
            descriptionStyle.fontSize = EditorStyles.label.fontSize - 2; // 기본 폰트 크기에서 2pt 감소

            EditorGUILayout.LabelField(
                "Apply To Children\n" +
                " - Determines whether to apply the effect to Image components on child objects.\n" +
                "Play On Start\n" +
                " - Sets whether the effect starts automatically when the object is enabled.", 
                descriptionStyle
            );
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);

            // Settings with colored labels
            var applyChildrenStyle = new GUIStyle(EditorStyles.label);
            applyChildrenStyle.normal.textColor = new Color(0.3f, 0.75f, 0.9f); // 하늘색
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Apply To Children", applyChildrenStyle, GUILayout.Width(120));
            _useChildApply.boolValue = EditorGUILayout.Toggle(_useChildApply.boolValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Play On Start", applyChildrenStyle, GUILayout.Width(120));
            _playOnStart.boolValue = EditorGUILayout.Toggle(_playOnStart.boolValue);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();

            // Dependencies Info
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Required Components", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "This component requires:\n" +
                "- RectTransform\n" +
                "- Image\n" +
                "- LightingEffectView\n" +
                "- LightingEffectLifetimeScope", 
                MessageType.Info);
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}