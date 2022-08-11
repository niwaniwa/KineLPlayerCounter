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
        internal const string GITHUB_IMAGE_GUID = "109a0a0acaeaf4b42a8eae55a2a33bbc";
        internal const string TWITTER_IMAGE_GUID = "a9f4234e988d6ee4b957857c0f6d62c4";
        internal const string DOCUMENT_IMAGE_GUID = "dbf2c736df2bc1642b56c0d3de75fbf3";

        internal const string GITHUB_URL = "https://github.com/niwaniwa/PlayerCounter";
        internal const string TWITTER_URL = "https://twitter.com/ni_rilana";
        internal const string DOCUMENT_URL = "https://niwaniwa.github.io/PlayerCounter/#/ja-jp/";
        
        private SerializedProperty countText, limitText, limit, animation, isPlatformCountMode, isQuest;
        

        private bool isFold = false;
        
        // From AAChair
        private Texture[] textures;
        private string[] guids;
        private string[] urls;
        private Texture headerTexture;
        private Texture githubTexture;
        private Texture twitterTexture;
        private Texture documentTexture;
        // private Texture discordTexture;
        //
        

        public void OnEnable()
        {
            countText = serializedObject.FindProperty(nameof(PlayerCounter.countText));
            limitText = serializedObject.FindProperty(nameof(PlayerCounter.limitText));
            limit = serializedObject.FindProperty(nameof(PlayerCounter.limit));
            animation = serializedObject.FindProperty(nameof(PlayerCounter.anim));
            isPlatformCountMode = serializedObject.FindProperty(nameof(PlayerCounter.isPlatformCountMode));
            isQuest = serializedObject.FindProperty(nameof(PlayerCounter.isQuest));
        }

        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawConvertToUdonBehaviourButton(target) 
                || UdonSharpGUI.DrawProgramSource(target))
                return;
            
            UdonSharpGUI.DrawUILine();
            
            serializedObject.Update();
            
            EditorGUILayout.Space();
            DrawHeader();
            EditorGUILayout.Space();
            DrawUserProperty();
            EditorGUILayout.Space();
            DrawInternalProperty();
            EditorGUILayout.Space();
            DrawFooter();

            serializedObject.ApplyModifiedProperties();
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

            EditorGUILayout.PropertyField(isPlatformCountMode);
            EditorGUILayout.PropertyField(isQuest);
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
        
        public void DrawHeader()
        {
            
            EditorGUILayout.Space();
            DrawLogoTexture(HEADER_IMAGE_GUID, headerTexture);
            EditorGUILayout.Space();
            
            EditorGUILayout.Space();
            if (textures == null)
            {
                LoadTextures();
            }
        }
        
        
        public void DrawFooter()
        {
            UdonSharpGUI.DrawUILine();
            EditorGUILayout.Space();
            DrawSocialLinks(textures, guids, urls);
            EditorGUILayout.Space();
        }
        
        public void LoadTextures()
        {
            headerTexture = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(HEADER_IMAGE_GUID), typeof(Texture)) as Texture;
            githubTexture = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(GITHUB_IMAGE_GUID), typeof(Texture)) as Texture;
            twitterTexture = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(TWITTER_IMAGE_GUID), typeof(Texture)) as Texture;
            documentTexture = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(DOCUMENT_IMAGE_GUID), typeof(Texture)) as Texture;
            // discordTexture = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guidDiscordIcon), typeof(Texture)) as Texture;
            textures = new Texture[] { githubTexture, twitterTexture, documentTexture };
            guids = new string[] { GITHUB_IMAGE_GUID, TWITTER_IMAGE_GUID, DOCUMENT_IMAGE_GUID };
            urls = new string[] { GITHUB_URL, TWITTER_URL, DOCUMENT_URL };
        }
        
        /// <summary>
        /// From AAChair by Kamishiro (https://github.com/AoiKamishiro/VRChatPrefabs/blob/master/Assets/00Kamishiro/AAChair/AAChair-README_JP.md) mit license
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="texture"></param>
        private void DrawLogoTexture(string guid, Texture texture)
        {
            if (texture == null)
            {
                texture = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Texture)) as Texture;
            }
            if (texture != null)
            {
                float w = EditorGUIUtility.currentViewWidth;
                Rect rect = new Rect
                {
                    width = w - 40f
                };
                rect.height = rect.width / 4f;
                Rect rect2 = GUILayoutUtility.GetRect(rect.width, rect.height);
                rect.x = ((EditorGUIUtility.currentViewWidth - rect.width) * 0.5f) - 4.0f;
                rect.y = rect2.y;
                GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill);
            }
        }

        /// <summary>
        /// From AAChair by Kamishiro (https://github.com/AoiKamishiro/VRChatPrefabs/blob/master/Assets/00Kamishiro/AAChair/AAChair-README_JP.md) mit license
        /// </summary>
        /// <param name="textures"></param>
        /// <param name="guids"></param>
        /// <param name="urls"></param>
        private void DrawSocialLinks(Texture[] textures, string[] guids, string[] urls)
        {
            float space = 10f;
            float padding = 10f;
            float size = 40f;

            float w = size * textures.Length + space * (textures.Length - 1);
            Rect socialAreaRect = new Rect
            {
                width = w,
                height = size + padding * 2
            };
            Rect sar = GUILayoutUtility.GetRect(socialAreaRect.width, socialAreaRect.height);
            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i] == null)
                {
                    textures[i] =
                        AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[i]), typeof(Texture)) as
                            Texture;
                }

                if (textures[i] != null)
                {
                    Rect rect = new Rect
                    {
                        width = size,
                        height = size,
                        x = ((EditorGUIUtility.currentViewWidth - w) * 0.5f) - 4.0f + size * i + space * i,
                        y = sar.y + padding
                    };
                    GUI.DrawTexture(rect, textures[i], ScaleMode.StretchToFill);
                    if (GUI.Button(rect, "", new GUIStyle()))
                    {
                        Application.OpenURL(urls[i]);
                    }
                }
            }
        }
        
    }
}