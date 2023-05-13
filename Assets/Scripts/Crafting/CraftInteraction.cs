using Crafting.Abstraction;
using InteractionSystem;
using InteractionSystem.Abstraction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Crafting
{
    public class CraftInteraction: SerializedMonoBehaviour, IInteractable
    {
        [SerializeField]
        private SpriteRenderer _itemRenderer;
        
        [SerializeField]
        private InteractionTextHandler _interactionTextHandler;
        
        public ICraftingHandler CraftingHandler { get; set; }
        
        private OutlineInteractionEffect _outlineInteractionEffect;
        public GameObject GameObject => gameObject;

        private CraftingRecipe _recipe;
        
        private void Awake()
        {
            _outlineInteractionEffect = GetComponent<OutlineInteractionEffect>();
        }

        public void SetRecipe(CraftingRecipe recipe)
        {
            _recipe = recipe;
            _itemRenderer.sprite = recipe.Result.ItemData.Icon;
        }
        
        public void Interact()
        {
            CraftingHandler.TryCraft(_recipe);
        }

        public void OnHandlerEnter(IInteractionHandler handler)
        {
            _outlineInteractionEffect?.Show();
            _interactionTextHandler?.Show();
        }

        public void OnHandlerExit(IInteractionHandler handler)
        {
            _outlineInteractionEffect?.Hide();
            _interactionTextHandler?.Hide();
        }
    }
}