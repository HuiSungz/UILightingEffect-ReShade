
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.Rendering;

namespace LightingEffect.Editor
{
    public class LightingEffectShaderPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string SHADER_NAME = "Custom/LightingEffectUnlit";

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            AddShaderToAlwaysIncluded();
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            AddShaderToAlwaysIncluded();
        }

        private static void AddShaderToAlwaysIncluded()
        {
            var shader = Shader.Find(SHADER_NAME);
            if (!shader)
            {
                Debug.LogError($"[LightingEffect] Cannot find shader: {SHADER_NAME}");
                return;
            }

            var graphicsSettings = GraphicsSettings.GetGraphicsSettings();
            var serializedObject = new SerializedObject(graphicsSettings);
            var alwaysIncludedShaders = serializedObject.FindProperty("m_AlwaysIncludedShaders");

            bool found = false;
            for (int i = 0; i < alwaysIncludedShaders.arraySize; i++)
            {
                var shaderProperty = alwaysIncludedShaders.GetArrayElementAtIndex(i);
                if (shaderProperty.objectReferenceValue == shader)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                alwaysIncludedShaders.arraySize++;
                var newElement = alwaysIncludedShaders.GetArrayElementAtIndex(alwaysIncludedShaders.arraySize - 1);
                newElement.objectReferenceValue = shader;
                serializedObject.ApplyModifiedProperties();
                
                Debug.Log($"[LightingEffect] Added shader to Always Included Shaders: {SHADER_NAME}");
            }
        }
    }
}