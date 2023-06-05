using InteractionSystem.Abstraction;
using Inventory.Interfaces;
using Items;
using Items.ItemDataSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldGeneration.RoomGeneration
{
    public class DoorInteractable: SerializedMonoBehaviour, IInteractable
    {
        [SerializeField]
        private ItemData _key;
        public GameObject GameObject => gameObject;
        
        public BiomeGenerationEntryPoint ParentRoom { get; set; }
        
        public BiomeGenerationEntryPoint TargetRoom { get; set; }

        private OpenDoorSide OpenDoorSide => Door.OpenDoorSide;
        
        public Door Door { get; set; }
        public void Interact(IInteractionHandler handler)
        {
            if(TargetRoom == null)
                return;
            
            var inventory = handler.GameObject.transform.parent.GetComponent<IInventory>();
            
            if(inventory == null)
                return;
            
            var keyItem = new Item(_key, Door.OpenCost);
            
            if(!inventory.HasItem(keyItem, out _))
                return;

            inventory.TryRemoveItem(keyItem);
            
            TargetRoom.gameObject.SetActive(true);

            if (TargetRoom != ParentRoom)
            {
                var doorToDestroy = TargetRoom.DoorInstances.Find(door => door.OpenDoorSide == GetOppositeSide(OpenDoorSide));
                doorToDestroy.DestroyDoor();
            }

            TargetRoom.gameObject.GetComponent<TilemapMerger>().Merge();
            
            DestroyDoor();
        }

        private void DestroyDoor()
        {
            Destroy(gameObject);
        }

        public void OnHandlerEnter(IInteractionHandler handler)
        {
            var inventory = handler.GameObject.GetComponent<IInventory>();
            
            if(inventory == null)
                return;
        }

        public void OnHandlerExit(IInteractionHandler handler)
        {
            
        }
        
        private OpenDoorSide GetOppositeSide(OpenDoorSide openDoorSide)
        {
            return openDoorSide switch
            {
                OpenDoorSide.Left => OpenDoorSide.Right,
                OpenDoorSide.Right => OpenDoorSide.Left,
                OpenDoorSide.Top => OpenDoorSide.Bottom,
                OpenDoorSide.Bottom => OpenDoorSide.Top,
                _ => OpenDoorSide.Bottom
            };
        }
    }
}