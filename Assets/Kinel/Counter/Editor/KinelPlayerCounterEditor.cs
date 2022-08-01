using System;
using Kinel.Counter.Udon;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;

namespace Kinel.Counter.Editor
{
    [CustomEditor(typeof(PlayerCounter))]
    public class KinelPlayerCounterEditor : UnityEditor.Editor
    {
        
        internal const string DEBUG_LOG_PREFIX = "[<color=#58ACFA>KineL</color>]";
        internal const string DEBUG_ERROR_PREFIX = "[<color=#dc143c>KineL</color>]";

        internal const string HEADER_IMAGE_GUID = "397c114a97607cd41abaed71f036c0cb";
        
        private Texture headerTexture;
        private SerializedProperty countText, limitText, limit, animation;

        private bool isFold = false;

        public void OnEnable()
        {
            countText = serializedObject.FindProperty(nameof(PlayerCounter.countText));
            limitText = serializedObject.FindProperty(nameof(PlayerCounter.limitText));
            limit = serializedObject.FindProperty(nameof(PlayerCounter.limit));
            animation = serializedObject.FindProperty(nameof(PlayerCounter.anim));
        }

        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawConvertToUdonBehaviourButton(target) 
                || UdonSharpGUI.DrawProgramSource(target))
                return;
            
            UdonSharpGUI.DrawUILine();
            
            EditorGUILayout.Space();
            Header();
            EditorGUILayout.Space();
            DrawUserProperty();
            EditorGUILayout.Space();
            DrawInternalProperty();
        }
        
        private void Header()
        {
            if (headerTexture == null)
            {
                headerTexture = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(HEADER_IMAGE_GUID));
            }
            
            if(headerTexture != null){
                Rect rect = new Rect();
                rect.width = EditorGUIUtility.currentViewWidth - (EditorGUIUtility.currentViewWidth * 0.1f);
                rect.height = rect.width / 4f; // yoko : 975, tate 250, hiritu = 3.9
                Rect yoyaku = GUILayoutUtility.GetRect(rect.width, rect.height); // GetRect参照しろ！！！！
                rect.x = EditorGUIUtility.currentViewWidth * 0.05f;
                rect.y = yoyaku.y;
                GUI.DrawTexture(rect, headerTexture, ScaleMode.StretchToFill);
            }
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Kinel Player Counter v1.3.1", EditorStyles.boldLabel);
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
            
        }

        public void DrawUserProperty()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("基本設定 / Settings", EditorStyles.boldLabel);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(limit, new GUIContent("最大人数 / limit"));
                EditorGUILayout.HelpBox("カウントする最大人数(インスタンス最大人数等) / " +
                                       "Maximum number of people to be counted (e.g., maximum number of instances)", MessageType.Info);
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndVertical();
        }

        public void DrawInternalProperty()
        {
            isFold = EditorGUILayout.Foldout(isFold, "Object Reference");
            EditorGUI.indentLevel++;
            if (isFold)
            {
                EditorGUILayout.PropertyField(countText);
                EditorGUILayout.PropertyField(limitText);
                EditorGUILayout.PropertyField(animation);
            }
            EditorGUI.indentLevel--;
        }
        
    }
}