using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.Linq;
using Items;
using Items.ItemDataSystem;

public class ItemLevelWizard : OdinEditorWindow
{
    // Reference to the ItemDatabase
    [SerializeField]
    private ItemDatabase itemDatabase;

    private Dictionary<int, List<string>> levelItemKeys = new Dictionary<int, List<string>>();
    private bool showTooltip;
    private string currentTooltipText;

    [MenuItem("Redray/Item Level Wizard")]
    private static void OpenWindow()
    {
        var window = GetWindow<ItemLevelWizard>();
        window.Show();
    }

    protected override void OnGUI()
    {
        base.OnGUI();

        // Check if ItemDatabase is not null
        if (itemDatabase == null)
        {
            EditorGUILayout.HelpBox("ItemDatabase is not assigned.", MessageType.Warning);
            return;
        }

        // Get the item data from the ItemDatabase
        List<ItemData> itemDataList = itemDatabase.GetItemData();

        // Calculate the minValue and maxValue based on the lowest and highest levels
        int minValue = int.MaxValue;
        int maxValue = int.MinValue;

        foreach (ItemData itemData in itemDataList)
        {
            int requiredLevel = itemData.RequiredLevel;
            minValue = Mathf.Min(minValue, requiredLevel);
            maxValue = Mathf.Max(maxValue, requiredLevel);
        }

        // Count the occurrence of each required level and collect item keys
        Dictionary<int, int> levelCounts = CountLevelOccurrences(itemDataList);

        // Draw the timeline
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Timeline:");

        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(150));

        GUILayout.BeginHorizontal();

        // Calculate the constant x-scale
        float xScale = 50f;

        for (int i = minValue; i <= maxValue; i++)
        {
            // Check if level exists in the level counts
            bool hasLevel = levelCounts.ContainsKey(i);

            GUILayout.BeginVertical(GUILayout.Width(xScale));

            // Display timeline value
            GUILayout.Label(i.ToString());

            // Draw shape based on the presence of level and count
            if (hasLevel)
            {
                int maxCount = levelCounts.Values.Max();
                float barHeight = Mathf.Lerp(10f, 50f, levelCounts[i] / (float)maxCount);
                DrawBar(barHeight);

                // Check if the mouse is hovering over the bar
                Rect barRect = GUILayoutUtility.GetLastRect();
                if (barRect.Contains(Event.current.mousePosition))
                {
                    showTooltip = true;
                    currentTooltipText = "Level " + i + ":\n";
                    currentTooltipText += string.Join("\n", levelItemKeys[i]);
                }
            }
            else
            {
                GUILayout.Box("", GUILayout.Height(10));
            }

            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        // Display the level counts
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Level Counts:");

        foreach (var kvp in levelCounts)
        {
            EditorGUILayout.LabelField("Level " + kvp.Key + ": " + kvp.Value);
        }

        // Draw the tooltip if required
        if (showTooltip)
        {
            GUIStyle tooltipStyle = new GUIStyle(GUI.skin.box);
            tooltipStyle.normal.textColor = Color.red;
            tooltipStyle.alignment = TextAnchor.UpperLeft;
            tooltipStyle.wordWrap = true;

            Handles.BeginGUI();
            Vector2 tooltipSize = tooltipStyle.CalcSize(new GUIContent(currentTooltipText));
            Rect tooltipRect = new Rect(Event.current.mousePosition + new Vector2(10, 10), tooltipSize);
            GUI.Box(tooltipRect, currentTooltipText, tooltipStyle);
            Handles.EndGUI();

        }

        // Reset tooltip variables
        showTooltip = false;
        currentTooltipText = string.Empty;
    }

    private void DrawBar(float height)
    {
        Rect rect = GUILayoutUtility.GetLastRect();
        float startY = rect.y + rect.height - height + 125;
        Rect barRect = new Rect(rect.x + 2, startY, rect.width - 4, height);
        EditorGUI.DrawRect(barRect, Color.white);
    }

    private Dictionary<int, int> CountLevelOccurrences(List<ItemData> itemDataList)
    {
        Dictionary<int, int> levelCounts = new Dictionary<int, int>();

        // Clear the level item keys
        levelItemKeys.Clear();

        // Count the occurrence of each required level and collect item keys
        foreach (ItemData itemData in itemDataList)
        {
            int requiredLevel = itemData.RequiredLevel;

            if (levelCounts.ContainsKey(requiredLevel))
            {
                levelCounts[requiredLevel]++;
            }
            else
            {
                levelCounts[requiredLevel] = 1;
            }

            if (!levelItemKeys.ContainsKey(requiredLevel))
            {
                levelItemKeys[requiredLevel] = new List<string>();
            }

            levelItemKeys[requiredLevel].Add(itemData.Key);
        }

        return levelCounts;
    }
}