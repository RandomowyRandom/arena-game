using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Items.Abstraction;
using Items.ItemDataSystem;
using Items.RaritySystem;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Stats;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class WeaponWizard : OdinEditorWindow
    {
        [MenuItem("Redray/Weapon Wizard")]
        private static void ShowWindow()
        {
            var window = GetWindow<WeaponWizard>();
            window.titleContent = new GUIContent("Weapon Wizard");
            window.Show();
        }
        
        [LabelWidth(100)]
        [HorizontalGroup("Row", 0.4f)]
        [VerticalGroup("Row/Left")]
        [ShowInInspector]
        private string _name;
        
        [LabelWidth(100)]
        [HorizontalGroup("Row", 0.4f)]
        [VerticalGroup("Row/Left")]
        [ShowInInspector]
        [ReadOnly]
        private string _key;

        [LabelWidth(100)]
        [VerticalGroup("Row/Middle")]
        [HorizontalGroup("Row", 0.4f)]
        [ShowInInspector] [TextArea(4, 10)]
        private string _description;
        
        [LabelWidth(100)]
        [VerticalGroup("Row/Right")]
        [HorizontalGroup("Row", 0.2f)]
        [ShowInInspector]
        [PreviewField(80, ObjectFieldAlignment.Center)]
        private Sprite _sprite;
        
        [Space(10)]
        [LabelWidth(100)]
        [HorizontalGroup("Row", 0.4f)]
        [VerticalGroup("Row/Left")]
        [ShowInInspector]
        private int _requiredLevel;
        
        [LabelWidth(100)]
        [HorizontalGroup("Row", 0.4f)]
        [VerticalGroup("Row/Left")]
        [ShowInInspector]
        private int _durability;
        
        [Space(20)]
        [OdinSerialize]
        [HorizontalGroup("Row1", 0.39f)]
        [VerticalGroup("Row1/Left")]
        private StatsData _statsData;
        
        [Header("Load template")]
        [Space(10)]
        [HorizontalGroup("Row1", 0.6f)]
        [VerticalGroup("Row1/Right")]
        [ShowInInspector]
        [HideLabel]
        private WeaponTemplate.WeaponTemplate _weaponTemplate;

        [HorizontalGroup("Row1", 0.5f)]
        [VerticalGroup("Row1/Right")]
        [ShowInInspector]
        [HideLabel]
        [Button("Load")]
        private void Load()
        {
            _name = _weaponTemplate.Name;
            _description = _weaponTemplate.Description;
            _requiredLevel = _weaponTemplate.RequiredLevel;
            _durability = _weaponTemplate.Durability;
            _effects = _weaponTemplate.ItemEffects;
            _statsData = _weaponTemplate.CommonStatsData;
            _sprite = _weaponTemplate.Icon;
        }

        [Space(20)]
        [OdinSerialize]
        private List<IItemEffect> _effects;

        [Space(50)]
        [OdinSerialize]
        [HorizontalGroup("Row2", 0.6f)]
        [VerticalGroup("Row2/Left")]
        [ReadOnly]
        List<GearRarityData> _gearRarityData = new();
        
        [HorizontalGroup("Row2", 0.6f)]
        [VerticalGroup("Row2/Left")]
        [Button("Generate rarity data")]
        private void GenerateRarityData()
        {
            _gearRarityDatabase ??= AssetDatabase.LoadAssetAtPath<GearRarityDatabase>("Assets/Scriptables/Database/DefaultGearRarityDatabase.asset");

            _gearRarityData = new();
            var rarities = _gearRarityDatabase.Rarities;
            
            _currentDamageIncrease = 0;
            _currentFireRateDecrease = _fireRateDecrease;
            
            for (var i = rarities.Count - 1; i >= 0; i--)
            {
                var rarity = rarities[i];
                var rarityData = new GearRarityData
                {
                    GearRarity = rarity,
                    StatsData = new(_statsData)
                };

                rarityData.StatsData.Damage += _currentDamageIncrease;
                _currentDamageIncrease += _damageIncrease;
                
                if (i <= 1)
                {
                    rarityData.StatsData.FireRate -= _currentFireRateDecrease;
                    _currentFireRateDecrease += _fireRateDecrease;
                }
                
                
                _gearRarityData.Add(rarityData);
            }
        }

        [Space(50)]
        [HorizontalGroup("Row2", 0.4f)]
        [VerticalGroup("Row2/Right")]
        [ShowInInspector]
        private float _damageIncrease;
        
        [HorizontalGroup("Row2", 0.4f)]
        [VerticalGroup("Row2/Right")]
        [ShowInInspector]
        private float _fireRateDecrease;

        [Button("Save weapon", ButtonSizes.Large)]
        private void Save()
        {
            // create new weapon SO
            var weapon = CreateInstance<Weapon>();
            
            weapon.DisplayName = _name;
            weapon.Description = _description;
            weapon.RequiredLevel = _requiredLevel;
            weapon.Durability = _durability;
            weapon.Icon = _sprite;
            weapon.SetEffects(_effects);
            weapon.SetRarityData(_gearRarityData);
            weapon.MaxStack = 1;
            
            // save weapon
            var path = "Assets/Scriptables/Weapon/" + _key + ".asset";
            
            // check if file already exists
            if (AssetDatabase.LoadAssetAtPath<Weapon>(path) != null)
            {
                if (!EditorUtility.DisplayDialog("File already exists", "Do you want to overwrite the existing file?", "Yes", "No"))
                {
                    return;
                }
            }
            
            AssetDatabase.CreateAsset(weapon, path);
            
            Selection.activeObject = weapon;
            EditorGUIUtility.PingObject(weapon);
            
            // mark object as dirty
            EditorUtility.SetDirty(weapon);
        }
        
        private GearRarityDatabase _gearRarityDatabase;
        private float _currentDamageIncrease;
        private float _currentFireRateDecrease;
        
        protected override void OnGUI()
        {
            base.OnGUI();
            
            _key = GetKeyForName();
            
            HandleEmptyWarnings();
            HandleSameAsTemplateWarning();
        }

        private void HandleSameAsTemplateWarning()
        {
            if(_statsData != null && _weaponTemplate != null)
            {
                if((Math.Abs(_weaponTemplate.CommonStatsData.Damage - _statsData.Damage) < 0.1f || Math.Abs(_weaponTemplate.CommonStatsData.FireRate - _statsData.FireRate) < 0.1f))
                    EditorGUILayout.HelpBox("Damage or fire rate are the same as template.", MessageType.Warning);
            }
            
            if (_weaponTemplate != null && _weaponTemplate.Name == _name)
            {
                EditorGUILayout.HelpBox("Name is the same as template.", MessageType.Info);
            }
            
            if (_weaponTemplate != null && _weaponTemplate.Description == _description)
            {
                EditorGUILayout.HelpBox("Description is the same as template.", MessageType.Info);
            }
            
            if (_weaponTemplate != null && _weaponTemplate.Icon == _sprite)
            {
                EditorGUILayout.HelpBox("Icon is the same as template.", MessageType.Info);
            }
            
            if (_weaponTemplate != null && _weaponTemplate.RequiredLevel == _requiredLevel)
            {
                EditorGUILayout.HelpBox("Required level is the same as template.", MessageType.Info);
            }
            
            if (_weaponTemplate != null && _weaponTemplate.Durability == _durability)
            {
                EditorGUILayout.HelpBox("Durability is the same as template.", MessageType.Info);
            }
        }
        private void HandleEmptyWarnings()
        {
            if (_gearRarityData == null || _gearRarityData.Count == 0)
            {
                EditorGUILayout.HelpBox("Gear rarity data is empty. Generate it first.", MessageType.Error);
            }

            if (_statsData.Damage == 0)
            {
                EditorGUILayout.HelpBox("Damage is 0. Set it first.", MessageType.Error);
            }

            if (_name.IsNullOrWhitespace() || _description.IsNullOrWhitespace() || _sprite == null)
            {
                EditorGUILayout.HelpBox("Name, description or icon is empty. Set it first.", MessageType.Error);
            }

            if (_statsData.FireRate == 0)
            {
                EditorGUILayout.HelpBox("Fire rate is 0. Set it first.", MessageType.Warning);
            }

            if (_effects == null || _effects.Count == 0)
            {
                EditorGUILayout.HelpBox("Effects are empty. Set it first.", MessageType.Warning);
            }

            if (_requiredLevel <= 0)
            {
                EditorGUILayout.HelpBox("Required level is invalid. Set it first.", MessageType.Warning);
            }

            if (_durability <= 0)
            {
                EditorGUILayout.HelpBox("Durability is invalid. Set it first.", MessageType.Warning);
            }
            
            if(_damageIncrease <= 0 || _fireRateDecrease <= 0)
            {
                EditorGUILayout.HelpBox("Damage increase or fire rate decrease is <= 0", MessageType.Info);
            }
        }

        private string GetKeyForName()
        {
            if (_name.IsNullOrWhitespace())
                return string.Empty;
            
            var key = _name.ToUpper();
            
            key = key.Replace(' ', '_');
            key = Regex.Replace(key, @"[^0-9a-zA-Z_]+", "");
            
            return key;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _effects = new List<IItemEffect>();
            _statsData = new StatsData();
            
            // load gear rarity database
            _gearRarityDatabase = AssetDatabase.LoadAssetAtPath<GearRarityDatabase>("Assets/Database/DefaultGearRarityDatabase.asset");
        }
    }
}