using InteractionSystem.Abstraction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WorldGeneration.RoomGeneration
{
    public class DoorInteractable: SerializedMonoBehaviour, IInteractable
    {
        public GameObject GameObject => gameObject;
        
        public BiomeGenerationEntryPoint ParentRoom { get; set; }
        
        public BiomeGenerationEntryPoint TargetRoom { get; set; }
        
        public OpenDoorSide OpenDoorSide { get; set; }
        public void Interact()
        {
            if(TargetRoom == null)
                return;
            
            TargetRoom.gameObject.SetActive(true);
            
            var doorToDestroy = TargetRoom.DoorInstances.Find(door => door.OpenDoorSide == GetOppositeSide(OpenDoorSide));
            
            doorToDestroy.DestroyDoor();
            DestroyDoor();
        }

        public void DestroyDoor()
        {
            Destroy(gameObject);
        }

        public void OnHandlerEnter(IInteractionHandler handler)
        {
            
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