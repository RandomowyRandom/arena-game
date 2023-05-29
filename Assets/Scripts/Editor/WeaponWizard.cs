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
        [HorizontalGroup("Row", 0.3f)]
        [VerticalGroup("Row/Left")]
        [ShowInInspector]
        private string _name;
        
        [LabelWidth(100)]
        [HorizontalGroup("Row", 0.3f)]
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
        [PreviewField(80, ObjectFieldAlignment.Right)]
        private Sprite _sprite;
        
        [Space(10)]
        [LabelWidth(100)]
        [HorizontalGroup("Row", 0.3f)]
        [VerticalGroup("Row/Left")]
        [ShowInInspector]
        private int _requiredLevel;
        
        [LabelWidth(100)]
        [HorizontalGroup("Row", 0.3f)]
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

        [Button("Save weapon")]
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