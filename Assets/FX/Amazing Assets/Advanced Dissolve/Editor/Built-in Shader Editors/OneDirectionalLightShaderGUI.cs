using System.Linq;

using UnityEngine;
using UnityEditor;


namespace AmazingAssets.AdvancedDissolveEditor
{
    internal class OneDirectionalLightShaderGUI : ShaderGUI
    {
        static MaterialProperty _AmbientLights = null;
        static MaterialProperty _PerVertexLights = null;

        static MaterialProperty _LightLookupTex = null;
        static MaterialProperty _LightLookupOffset = null;

        static MaterialProperty _IncludeVertexColor = null;
        static MaterialProperty _Color = null;
        static MaterialProperty _MainTex = null;
        static MaterialProperty _MainTex_Scroll = null;
        static MaterialProperty _Cutoff = null;

        static MaterialProperty _TextureMix = null;
        static MaterialProperty _SecondaryTex = null;
        static MaterialProperty _SecondaryTex_Scroll = null;
        static MaterialProperty _SecondaryTex_Blend = null;

        static MaterialProperty _NormalMapStrength = null;
        static MaterialProperty _NormalMap = null;
        static MaterialProperty _NormalMap_Scroll = null;
        static MaterialProperty _SecondaryNormalMap = null;
        static MaterialProperty _SecondaryNormalMap_Scroll = null;

        static MaterialProperty _SpecularColor = null;
        static MaterialProperty _Shininess = null;
        static MaterialProperty _SpecularMaskOffset = null;

        static MaterialProperty _ReflectionColor = null;
        static MaterialProperty _ReflectionMaskOffset = null;
        static MaterialProperty _ReflectionCubeMap = null;
        static MaterialProperty _ReflectionFresnelBias = null;

        static MaterialProperty _RimColor = null;
        static MaterialProperty _RimBias = null;

        static MaterialProperty _EmissionColor = null;
        static MaterialProperty _EmissionMap = null;
        static MaterialProperty _EmissionMap_Scroll = null;



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


                materialEditor.ShaderProperty(_AmbientLights, "Ambient Lights");
                materialEditor.ShaderProperty(_PerVertexLights, "Per-Vertex Lights");

              
                DrawAlbedo(materialEditor, material);
                DrawLightLookUp(materialEditor, material);
                DrawBumpMap(materialEditor, material);
                DrawSpecular(materialEditor, material);
                DrawReflection(materialEditor, material);
                DrawRimFresnel(materialEditor, material);
                DrawEmission(materialEditor, material);
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

            materialEditor.ShaderProperty(_Cull, "Render Face");
        }

        void DrawLightLookUp(UnityEditor.MaterialEditor materialEditor, Material material)
        {
            bool value = material.shaderKeywords.Contains("_LIGHTLOOKUP");

            using (new EditorGUIHelper.EditorGUILayoutBeginVertical(EditorStyles.helpBox))
            {
                //Anchor
                EditorGUILayout.LabelField(string.Empty);
                Rect rect = GUILayoutUtility.GetLastRect();

                if (UnityEditor.EditorGUIUtility.isProSkin)
                    EditorGUI.DrawRect(new Rect(rect.xMin - 2, rect.yMin, rect.width + 4, rect.height), Color.white * 0.35f);

                EditorGUI.BeginChangeCheck();
                value = EditorGUI.ToggleLeft(rect, "Light Lookup (Toon Shading)", value, EditorStyles.boldLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    if (value)
                        material.EnableKeyword("_LIGHTLOOKUP");
                    else
                        material.DisableKeyword("_LIGHTLOOKUP");
                }

                if (value)
                {
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        materialEditor.TexturePropertySingleLine(new GUIContent("Lookup Texture"), _LightLookupTex);

                        if (_LightLookupTex.textureValue != null && _LightLookupTex.textureValue.wrapMode != TextureWrapMode.Clamp)
                            EditorGUILayout.HelpBox("Make sure texture uses 'Clamp' wrap mode.", MessageType.Warning);

                        materialEditor.ShaderProperty(_LightLookupOffset, "Bias");
                    }
                }
            }
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
                    materialEditor.ShaderProperty(_IncludeVertexColor, "Vertex Color");
                    materialEditor.ShaderProperty(_Color, "Tint Color");

                    materialEditor.TexturePropertySingleLine(new GUIContent("Main Map"), _MainTex);
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        materialEditor.TextureScaleOffsetProperty(_MainTex);
                        materialEditor.ShaderProperty(_MainTex_Scroll, string.Empty);
                    }


                    //Cutout
                    if (material.shaderKeywords.Contains("_ALPHATEST_ON"))
                    {
                        materialEditor.ShaderProperty(_Cutoff, "Alpha Cutoff");
                    }


                    GUILayout.Space(10);
                    materialEditor.ShaderProperty(_TextureMix, "Texture Blend");
                    if (material.shaderKeywords.Contains("_TEXTUREMIX_BY_MAIN_ALPHA") ||
                        material.shaderKeywords.Contains("_TEXTUREMIX_BY_SECONDARY_ALPHA") ||
                        material.shaderKeywords.Contains("_TEXTUREMIX_MULTIPLE") ||
                        material.shaderKeywords.Contains("_TEXTUREMIX_ADDITIVE"))

                    {
                        materialEditor.TexturePropertySingleLine(new GUIContent("Secondary Map"), _SecondaryTex);
                        using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                        {
                            materialEditor.TextureScaleOffsetProperty(_SecondaryTex);
                            materialEditor.ShaderProperty(_SecondaryTex_Scroll, string.Empty);
                        }
                    }
                    else if (material.shaderKeywords.Contains("_TEXTUREMIX_BY_VERTEX_ALPHA"))
                    {
                        materialEditor.ShaderProperty(_SecondaryTex_Blend, "Vertex Alpha Offset");
                        materialEditor.TexturePropertySingleLine(new GUIContent("Secondary Map"), _SecondaryTex);
                        using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                        {
                            materialEditor.TextureScaleOffsetProperty(_SecondaryTex);
                            materialEditor.ShaderProperty(_SecondaryTex_Scroll, string.Empty);
                        }
                    }
                }
            }
        }

        void DrawBumpMap(UnityEditor.MaterialEditor materialEditor, Material material)
        {
            bool value = material.shaderKeywords.Contains("_NORMALMAP");

            using (new EditorGUIHelper.EditorGUILayoutBeginVertical(EditorStyles.helpBox))
            {
                //Anchor
                EditorGUILayout.LabelField(string.Empty);
                Rect rect = GUILayoutUtility.GetLastRect();

                if (UnityEditor.EditorGUIUtility.isProSkin)
                    EditorGUI.DrawRect(new Rect(rect.xMin - 2, rect.yMin, rect.width + 4, rect.height), Color.white * 0.35f);


                EditorGUI.BeginChangeCheck();
                value = EditorGUI.ToggleLeft(rect, "Bump", value, EditorStyles.boldLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    if (value)
                        material.EnableKeyword("_NORMALMAP");
                    else
                        material.DisableKeyword("_NORMALMAP");
                }

                if (value)
                {
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        materialEditor.ShaderProperty(_NormalMapStrength, "Strength");
                        materialEditor.TexturePropertySingleLine(new GUIContent("Normal Map"), _NormalMap);
                        using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                        {
                            materialEditor.TextureScaleOffsetProperty(_NormalMap);
                            materialEditor.ShaderProperty(_NormalMap_Scroll, string.Empty);
                        }

                        if (material.shaderKeywords.Contains("_TEXTUREMIX_BY_MAIN_ALPHA") ||
                            material.shaderKeywords.Contains("_TEXTUREMIX_BY_SECONDARY_ALPHA") ||
                            material.shaderKeywords.Contains("_TEXTUREMIX_MULTIPLE") ||
                            material.shaderKeywords.Contains("_TEXTUREMIX_ADDITIVE") ||
                            material.shaderKeywords.Contains("_TEXTUREMIX_BY_VERTEX_ALPHA"))
                        {
                            GUILayout.Space(5);
                            materialEditor.TexturePropertySingleLine(new GUIContent("Secondary Normal Map"), _SecondaryNormalMap);
                            using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                            {
                                materialEditor.TextureScaleOffsetProperty(_SecondaryNormalMap);
                                materialEditor.ShaderProperty(_SecondaryNormalMap_Scroll, string.Empty);
                            }
                        }
                    }
                }
            }
        }

        void DrawSpecular(UnityEditor.MaterialEditor materialEditor, Material material)
        {
            bool value = material.shaderKeywords.Contains("_SPECULAR");

            using (new EditorGUIHelper.EditorGUILayoutBeginVertical(EditorStyles.helpBox))
            {
                //Anchor
                EditorGUILayout.LabelField(string.Empty);
                Rect rect = GUILayoutUtility.GetLastRect();

                if(UnityEditor.EditorGUIUtility.isProSkin)
                    EditorGUI.DrawRect(new Rect(rect.xMin - 2, rect.yMin, rect.width + 4, rect.height), Color.white * 0.35f);


                EditorGUI.BeginChangeCheck();
                value = EditorGUI.ToggleLeft(rect, "Specular", value, EditorStyles.boldLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    if (value)
                        material.EnableKeyword("_SPECULAR");
                    else
                        material.DisableKeyword("_SPECULAR");
                }

                if (value)
                {
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        materialEditor.ShaderProperty(_SpecularColor, "Color");
                        materialEditor.ShaderProperty(_Shininess, "Shininess");
                        materialEditor.ShaderProperty(_SpecularMaskOffset, "Mask Offset");
                    }
                }
            }
        }

        void DrawReflection(UnityEditor.MaterialEditor materialEditor, Material material)
        {
            bool value = material.shaderKeywords.Contains("_REFLECTION");

            using (new EditorGUIHelper.EditorGUILayoutBeginVertical(EditorStyles.helpBox))
            { 
                //Anchor
                EditorGUILayout.LabelField(string.Empty);
                Rect rect = GUILayoutUtility.GetLastRect();

                if (UnityEditor.EditorGUIUtility.isProSkin)
                    EditorGUI.DrawRect(new Rect(rect.xMin - 2, rect.yMin, rect.width + 4, rect.height), Color.white * 0.35f);


                EditorGUI.BeginChangeCheck();
                value = EditorGUI.ToggleLeft(rect, "Reflection", value, EditorStyles.boldLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    if (value)
                        material.EnableKeyword("_REFLECTION");
                    else
                        material.DisableKeyword("_REFLECTION");
                }

                if (value)
                {
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        materialEditor.ShaderProperty(_ReflectionColor, "Color");
                        materialEditor.ShaderProperty(_ReflectionMaskOffset, "Mask Offset");
                        materialEditor.TexturePropertySingleLine(new GUIContent("CubeMap"), _ReflectionCubeMap);
                        materialEditor.ShaderProperty(_ReflectionFresnelBias, "Fresnel Bias");
                    }
                }
            }
        }

        void DrawRimFresnel(UnityEditor.MaterialEditor materialEditor, Material material)
        {
            bool value = material.shaderKeywords.Contains("_RIM");

            using (new EditorGUIHelper.EditorGUILayoutBeginVertical(EditorStyles.helpBox))
            {
                //Anchor
                EditorGUILayout.LabelField(string.Empty);
                Rect rect = GUILayoutUtility.GetLastRect();

                if (UnityEditor.EditorGUIUtility.isProSkin)
                    EditorGUI.DrawRect(new Rect(rect.xMin - 2, rect.yMin, rect.width + 4, rect.height), Color.white * 0.35f);


                EditorGUI.BeginChangeCheck();
                value = EditorGUI.ToggleLeft(rect, "Rim/Fresnel", value, EditorStyles.boldLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    if (value)
                        material.EnableKeyword("_RIM");
                    else
                        material.DisableKeyword("_RIM");
                }

                if (value)
                {
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        materialEditor.ShaderProperty(_RimColor, "Color");
                        materialEditor.ShaderProperty(_RimBias, "Bias");
                    }
                }
            }
        }

        void DrawEmission(UnityEditor.MaterialEditor materialEditor, Material material)
        {
            bool value = material.shaderKeywords.Contains("_EMISSION");

            using (new EditorGUIHelper.EditorGUILayoutBeginVertical(EditorStyles.helpBox))
            {
                //Anchor
                EditorGUILayout.LabelField(string.Empty);
                Rect rect = GUILayoutUtility.GetLastRect();

                if (UnityEditor.EditorGUIUtility.isProSkin)
                    EditorGUI.DrawRect(new Rect(rect.xMin - 2, rect.yMin, rect.width + 4, rect.height), Color.white * 0.35f);


                EditorGUI.BeginChangeCheck();
                value = EditorGUI.ToggleLeft(rect, "Emission", value, EditorStyles.boldLabel);
                if (EditorGUI.EndChangeCheck())
                {
                    if (value)
                        material.EnableKeyword("_EMISSION");
                    else
                        material.DisableKeyword("_EMISSION");
                }

                if (value)
                {
                    using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                    {
                        materialEditor.ShaderProperty(_EmissionColor, "Color");
                        materialEditor.TexturePropertySingleLine(new GUIContent("Emission Map"), _EmissionMap);
                        using (new EditorGUIHelper.EditorGUIIndentLevel(1))
                        {
                            materialEditor.TextureScaleOffsetProperty(_EmissionMap);
                            materialEditor.ShaderProperty(_EmissionMap_Scroll, string.Empty);
                        }
                    }
                }
            }
        }




        void FindProperties(MaterialProperty[] properties)
        {
            _AmbientLights = AdvancedDissolveEditor.MaterialEditor.FindProperty("_AmbientLights", properties, true);
            _PerVertexLights = AdvancedDissolveEditor.MaterialEditor.FindProperty("_PerVertexLights", properties, true);

            _LightLookupTex = AdvancedDissolveEditor.MaterialEditor.FindProperty("_LightLookupTex", properties, true);
            _LightLookupOffset = AdvancedDissolveEditor.MaterialEditor.FindProperty("_LightLookupOffset", properties, true);

            _IncludeVertexColor = AdvancedDissolveEditor.MaterialEditor.FindProperty("_IncludeVertexColor", properties, true);
            _Color = AdvancedDissolveEditor.MaterialEditor.FindProperty("_Color", properties, true);
            _MainTex = AdvancedDissolveEditor.MaterialEditor.FindProperty("_MainTex", properties, true);
            _MainTex_Scroll = AdvancedDissolveEditor.MaterialEditor.FindProperty("_MainTex_Scroll", properties, true);
            _Cutoff = AdvancedDissolveEditor.MaterialEditor.FindProperty("_Cutoff", properties, true);

            _TextureMix = AdvancedDissolveEditor.MaterialEditor.FindProperty("_TextureMix", properties, true);
            _SecondaryTex = AdvancedDissolveEditor.MaterialEditor.FindProperty("_SecondaryTex", properties, true);
            _SecondaryTex_Scroll = AdvancedDissolveEditor.MaterialEditor.FindProperty("_SecondaryTex_Scroll", properties, true);
            _SecondaryTex_Blend = AdvancedDissolveEditor.MaterialEditor.FindProperty("_SecondaryTex_Blend", properties, true);

            _NormalMapStrength = AdvancedDissolveEditor.MaterialEditor.FindProperty("_NormalMapStrength", properties, true);
            _NormalMap = AdvancedDissolveEditor.MaterialEditor.FindProperty("_NormalMap", properties, true);
            _NormalMap_Scroll = AdvancedDissolveEditor.MaterialEditor.FindProperty("_NormalMap_Scroll", properties, true);
            _SecondaryNormalMap = AdvancedDissolveEditor.MaterialEditor.FindProperty("_SecondaryNormalMap", properties, true);
            _SecondaryNormalMap_Scroll = AdvancedDissolveEditor.MaterialEditor.FindProperty("_SecondaryNormalMap_Scroll", properties, true);

            _SpecularColor = AdvancedDissolveEditor.MaterialEditor.FindProperty("_SpecularColor", properties, true);
            _Shininess = AdvancedDissolveEditor.MaterialEditor.FindProperty("_Shininess", properties, true);
            _SpecularMaskOffset = AdvancedDissolveEditor.MaterialEditor.FindProperty("_SpecularMaskOffset", properties, true);

            _ReflectionColor = AdvancedDissolveEditor.MaterialEditor.FindProperty("_ReflectionColor", properties, true);
            _ReflectionMaskOffset = AdvancedDissolveEditor.MaterialEditor.FindProperty("_ReflectionMaskOffset", properties, true);
            _ReflectionCubeMap = AdvancedDissolveEditor.MaterialEditor.FindProperty("_ReflectionCubeMap", properties, true);
            _ReflectionFresnelBias = AdvancedDissolveEditor.MaterialEditor.FindProperty("_ReflectionFresnelBias", properties, true);

            _RimColor = AdvancedDissolveEditor.MaterialEditor.FindProperty("_RimColor", properties, true);
            _RimBias = AdvancedDissolveEditor.MaterialEditor.FindProperty("_RimBias", properties, true);

            _EmissionColor = AdvancedDissolveEditor.MaterialEditor.FindProperty("_EmissionColor", properties, true);
            _EmissionMap = AdvancedDissolveEditor.MaterialEditor.FindProperty("_EmissionMap", properties, true);
            _EmissionMap_Scroll = AdvancedDissolveEditor.MaterialEditor.FindProperty("_EmissionMap_Scroll", properties, true);


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