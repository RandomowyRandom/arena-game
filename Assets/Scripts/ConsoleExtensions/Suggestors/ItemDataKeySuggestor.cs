using System.Collections.Generic;
using System.Linq;
using Items;
using QFSW.QC;

namespace ConsoleExtensions.Suggestors
{
    public struct ItemDataKeyTag : IQcSuggestorTag
    {
        
    }

    public sealed class ItemDataKeyAttribute : SuggestorTagAttribute
    {
        private readonly IQcSuggestorTag[] _tags = { new ItemDataKeyTag() };

        public override IQcSuggestorTag[] GetSuggestorTags()
        {
            return _tags;
        }
    }
    
    public class ItemDataKeySuggestor : BasicCachedQcSuggestor<string>
    {
        private const string ITEM_DATABASE_PATH = "Assets/Scriptables/Database/DefaultItemDatabase.asset";
        private ItemDatabase _itemDatabase;
        
        private string[] _itemDataKeys;
        
        protected override bool CanProvideSuggestions(SuggestionContext context, SuggestorOptions options)
        {
            return context.HasTag<ItemDataKeyTag>();
        }

        protected override IQcSuggestion ItemToSuggestion(string item)
        {
            return new RawSuggestion(item, true);
        }

        protected override IEnumerable<string> GetItems(SuggestionContext context, SuggestorOptions options)
        {
            if (_itemDatabase != null) 
                return _itemDataKeys;
            
            _itemDatabase = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemDatabase>(ITEM_DATABASE_PATH);

            var items = _itemDatabase.GetItemData();
            var itemDataKeys = items.Select(itemData => itemData.Key).ToList();
            _itemDataKeys = itemDataKeys.ToArray();

            return _itemDataKeys;
        }
    }
}