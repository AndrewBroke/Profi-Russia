#pragma warning disable 649

using UnityEngine;

namespace Watermelon
{
    [System.Serializable]
    public class Prop
    {
        [SerializeField]
        private Item type;
        public Item Type
        {
            get { return type; }
        }

        [SerializeField]
        private GameObject prefab;
        public GameObject Prefab
        {
            get { return prefab; }
        }

        public Prop(Item type, GameObject prefab)
        {
            this.type = type;
            this.prefab = prefab;
        }

        public Prop(Item type)
        {
            this.type = type;
        }
    }
}