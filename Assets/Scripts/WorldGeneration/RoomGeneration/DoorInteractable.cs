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
        
        public OpenDoorSide OpenDoorSide => Door.OpenDoorSide;
        
        public Door Door { get; set; }
        public void Interact()
        {
            if(TargetRoom == null)
                return;
            
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