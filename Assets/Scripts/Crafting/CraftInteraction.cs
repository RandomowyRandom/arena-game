using System;
using Crafting.Abstraction;
using InteractionSystem;
using InteractionSystem.Abstraction;
using Sirenix.OdinInspector;
using UI.Crafting.Abstraction;
using UnityEngine;

namespace Crafting
{
    public class CraftInteraction: SerializedMonoBehaviour, IInteractable, ICraftingRecipeProvider
    {
        [SerializeField]
        private SpriteRenderer _itemRenderer;
        
        [SerializeField]
        private InteractionTextHandler _interactionTextHandler;

        public event Action<CraftInteraction> OnSelected; 
        public event Action<CraftInteraction> OnDeselected;
        public event Action OnRecipeChanged;

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
            
            OnRecipeChanged?.Invoke();
        }
        
        public CraftingRecipe GetRecipe()
        {
            return _recipe;
        }
        
        public void Interact()
        {
            CraftingHandler.TryCraft(GetRecipe());
        }

        public void OnHandlerEnter(IInteractionHandler handler)
        {
            _outlineInteractionEffect?.Show();
            _interactionTextHandler?.Show();
            
            OnSelected?.Invoke(this);
        }

        public void OnHandlerExit(IInteractionHandler handler)
        {
            _outlineInteractionEffect?.Hide();
            _interactionTextHandler?.Hide();
            
            OnDeselected?.Invoke(this);
        }
    }
}