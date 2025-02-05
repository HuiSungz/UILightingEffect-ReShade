
using UnityEngine;
using UnityEditor;

namespace LightingEffect.Editor
{
    [InitializeOnLoad]
    public class EffectorHierarchyIcon
    {
        #region Fields & Init

        private static Texture2D _iconTexture;
        private static string IconPath => "LightingEffectorIcon";
        
        static EffectorHierarchyIcon()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
            
            LoadIcon();
        }

        #endregion
        
        private static void LoadIcon()
        {
            if (!_iconTexture)
            {
                _iconTexture = Resources.Load<Texture2D>(IconPath);
            }
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (!gameObject || !gameObject.GetComponent<LightingEffector>())
            {
                return;
            }
            
            var iconRect = new Rect(selectionRect)
            {
                x = selectionRect.x - 20,
                width = 16,
                height = 16
            };

            iconRect.y += (selectionRect.height - 16) * 0.5f;
            if (_iconTexture)
            {
                GUI.DrawTexture(iconRect, _iconTexture);
            }
        }
    }
}