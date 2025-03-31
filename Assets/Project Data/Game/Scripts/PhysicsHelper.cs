using UnityEngine;

namespace Watermelon
{
    public class PhysicsHelper : MonoBehaviour
    {
        public static readonly string TAG_VISUALS = "Visuals";
        public static readonly string TAG_PART = "Part";


        public static int GetLayerMask(PhysicsLayer layer)
        {
            return (int)Mathf.Pow(2, (int)layer);
        }


        public static bool CompareLayer(MonoBehaviour behavior, PhysicsLayer layer)
        {
            return behavior.gameObject.layer == (int)layer;
        }


        public static bool CompareLayer(GameObject obj, PhysicsLayer layer)
        {
            return obj.layer == (int)layer;
        }
    }

    public enum PhysicsLayer
    {
        Default = 0,

        Pickable = 6,
        Snap = 7,
        Ground = 8,
        Finish = 9,
        Rover = 10,
        Battery = 11,
        Floor = 12,
        RoverVisuals = 13,

    }
}
