using System.Linq;

using UnityEngine;
using UnityEditor;


namespace AmazingAssets.AdvancedDissolveEditor
{
    internal class VertexLitShaderGUI : ShaderGUI
    {
        static MaterialProperty _Color = null;
        static MaterialProperty _MainTex = null;
        static MaterialProperty _Cutoff = null;

        static MaterialProperty _BlendMode = null;
        static MaterialProperty _Cull = null;

        public override void OnGUI(UnityEditor.MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            FindProperties(properties);

            Material material = (Material)materialEditor.target;

            AmazingAssets.AdvancedDissolveEditor.MaterialEditor.Init(properties);
            AmazingAssets.AdvancedDissolveEditor.MaterialEditor.DrawCurvedWorldHeader(materialEditor, material);

            if (AmazingAssets.AdvancedDissolveEditor.MaterialEditor.DrawDefaultOptionsHeader("Default Shader Options", material, CallbackReset))
            {
                DrawRenderingAndBlendModes(materialEditor, material);

                DrawAlbedo(materialEditor, material);
            }


            AmazingAssets.AdvancedDissolveEditor.MaterialEditor.DrawDissolveOptions(materialEditor, false, false, true, false, false);


            GUILayout.Space(5);
            if (AmazingAssets.AdvancedDissolveEditor.MaterialEditor.DrawFooterOptionsHeader())
            {
                base.OnGUI(materialEditor, properties);
            }
        }

        void DrawRenderingAndBlendModes(UnityEditor.MaterialEditor materialEditor, Material material)
        {
            if (_BlendMode != null)
            {
                AdvancedDissolveEditor.MaterialEditor.SetupMaterialWithBlendMode(material, (AdvancedDissolveEditor.MaterialEditor.BlendMode)material.GetFloat("_Mode"));  //If blend modes are not available - use default blend mode


                EditorGUI.BeginChangeCheck();
                {
                    AdvancedDissolveEditor.MaterialEditor.DrawBlendModePopup(materialEditor, _BlendMode);
                }
                if (EditorGUI.EndChangeCheck())
                {
                    foreach (var obj in _BlendMode.targets)
                    {
                        Material mat = (Material)obj;
                        AdvancedDissolveEditor.MaterialEditor.SetupMaterialWithBlendMode(mat, (AdvancedDissolveEditor.MaterialEditor.BlendMode)mat.GetFloat("_Mode"));
                    }
                }
            }

            materialEditor.ShaderProperty(_Cull, "Render Face");
        }

        void DrawAlbedo(UnityEditor.MaterialEditor materialEditor, Material material)
        {
            using (new EditorGUIHelper.EditorGUILayoutBeginVertical(EditorStyles.helpBox))
            {
                //Anchor
                EditorGUILayout.LabelField(string.Empty);
                Rect rect = GUILayoutUtility.GetLastRect();

                if (UnityEditor.EditorGUIUtility.isProSkin)
                    EditorGUI.DrawRect(new Rect(rect.xMin - 2, rect.yMin, rect.width + 4, rect.height), Color.white * 0.35f);


                EditorGUI.LabelField(rect, "Albedo", EditorStyles.boldLabel);

                using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                {
                    materialEditor.ShaderProperty(_Color, "Tint Color");

                    materialEditor.TexturePropertySingleLine(new GUIContent("Main Map"), _MainTex);
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        materialEditor.TextureScaleOffsetProperty(_MainTex);
                    }


                    //Cutout
                    if (material.shaderKeywords.Contains("_ALPHATEST_ON"))
                    {
                        materialEditor.ShaderProperty(_Cutoff, "Alpha Cutoff");
                    }
                }
            }
        }


        void FindProperties(MaterialProperty[] properties)
        {
            _Color = AdvancedDissolveEditor.MaterialEditor.FindProperty("_Color", properties, true);
            _MainTex = AdvancedDissolveEditor.MaterialEditor.FindProperty("_MainTex", properties, true);
            _Cutoff = AdvancedDissolveEditor.MaterialEditor.FindProperty("_Cutoff", properties, true);

            
            _BlendMode = FindProperty("_Mode", properties, false);
            _Cull = FindProperty("_Cull", properties, false);
        }

        void CallbackReset(object obj)
        {
            Material material = (Material)obj;
            if (material == null)
                return;


        }

    }
}