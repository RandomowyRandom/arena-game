﻿using System;
using System.Collections.Generic;
using System.Linq;
using Common.Attributes;
using Common.Enums;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ScriptableFactory : OdinEditorWindow
    {
        private static List<Type> _types;
        private static List<string> _names;

        [MenuItem("Redray/Scriptable Factory")]
        private static void OpenWindow()
        {
            GetWindow<ScriptableFactory>().Show();

            _types = GetDerivedTypes(typeof(SerializedScriptableObject));
            _names = _types.Select(t => t.Name).ToList();
        }

        [VerticalGroup]
        [ValueDropdown(nameof(_names))]
        [ShowInInspector]
        private string _selectedType;
        
        [VerticalGroup]
        [ShowInInspector]
        private string _objectName;
        
        [VerticalGroup]
        [ShowInInspector]
        private ScriptableType _scriptableType;
        
        [Space(20)]
        [ShowInInspector]
        private bool _focusOnInstantiate = true;
        
        [HorizontalGroup]
        [Button(ButtonSizes.Large)]
        public void Instantiate()
        {
            var instance = CreateInstance(_selectedType);

            if (_objectName.IsNullOrWhitespace())
            {
                Debug.LogError("Object name is empty");
                return;
            }
            
            var path = GetPath(_scriptableType, _objectName);  
            
            var directory = path[..path.LastIndexOf('/')];
            
            if (!AssetDatabase.IsValidFolder(directory))
                AssetDatabase.CreateFolder("Assets/Scriptables", _scriptableType.ToString());

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.SaveAssets();

            if (!_focusOnInstantiate) 
                return;
            
            Selection.activeObject = instance;
            EditorGUIUtility.PingObject(instance);
        }

        private static string GetPath(ScriptableType type, string objectName)
        {
            return $"Assets/Scriptables/{type.ToString()}/{objectName}.asset";
        }
        
        private static List<Type> GetDerivedTypes(Type baseType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return (from assembly in assemblies from type in assembly.GetTypes() 
                where type.IsSubclassOf(baseType) && 
                      !type.IsAbstract &&
                      HasAttribute(type) &&
                      !type.IsInterface select type).ToList();
        }
        
        private static bool HasAttribute(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(ScriptableFactoryElementAttribute), false);
            return attributes.Length > 0;
        }
    }
}