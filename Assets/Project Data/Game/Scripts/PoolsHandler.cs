using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    public class PoolsHandler
    {
        private Dictionary<RoverPart, PoolGeneric<RoverPartBehavior>> roverPartPools = new Dictionary<RoverPart, PoolGeneric<RoverPartBehavior>>();
        private Dictionary<RoverPart, Pool> partVisualsPools = new Dictionary<RoverPart, Pool>();
        private Dictionary<Item, Pool> propPools = new Dictionary<Item, Pool>();

        public Pool torchPool;
        public Pool cardboardPool;
        public Pool coinPool;
        public Pool tapePool;


        public void Init()
        {
            roverPartPools.Clear();

            for (int i = 0; i < GameController.LevelDatabase.PartsAmount; i++)
            {
                var part = GameController.LevelDatabase.GetPart(i);

                var partPoolName = $"Rover Part {i}";
                var visualsPoolName = $"Rover Part {i} visuals";

                var partPoolSettings = new PoolSettings(partPoolName, part.PartObject, 3, true);
                var visualsPoolSettings = new PoolSettings(visualsPoolName, part.BuildVisuals, 3, true);

                var partPool = new PoolGeneric<RoverPartBehavior>(partPoolSettings);
                var visualsVool = new Pool(visualsPoolSettings);

                roverPartPools.Add(part, partPool);
                partVisualsPools.Add(part, visualsVool);
            }

            propPools.Clear();

            for (int i = 0; i < GameController.LevelDatabase.PropAmount; i++)
            {
                var prop = GameController.LevelDatabase.GetProp(i);

                propPools.Add(prop.Type, PoolManager.AddPool(new PoolSettings("Prop_" + prop.Type.ToString(), prop.Prefab, 5, true)));
            }

            torchPool = PoolManager.GetPoolByName("Torch");
            cardboardPool = PoolManager.GetPoolByName("Cardboard");
            coinPool = PoolManager.GetPoolByName("Coin");
            tapePool = PoolManager.GetPoolByName("Tape");
        }


        public RoverPartBehavior GetRoverPartBehavior(RoverPart part)
        {
            var pool = roverPartPools[part];

            var roverPartBehavior = pool.GetPooledComponent();

            return roverPartBehavior;
        }


        public GameObject GetRoverpartVisuals(RoverPart part)
        {
            var pool = partVisualsPools[part];

            var visuals = pool.GetPooledObject();

            return visuals;
        }


        public void ReturnEverythingToPool()
        {
            foreach (var pool in roverPartPools.Values)
            {
                pool.ReturnToPoolEverything();
            }

            foreach (var pool in partVisualsPools.Values)
            {
                pool.ReturnToPoolEverything();
            }

            foreach (var pool in propPools.Values)
            {
                pool.ReturnToPoolEverything();
            }

            torchPool.ReturnToPoolEverything();
            cardboardPool.ReturnToPoolEverything();
            coinPool.ReturnToPoolEverything();
            tapePool.ReturnToPoolEverything();
        }


        public GroundRenderer GetCardboard()
        {
            return cardboardPool.GetPooledObject().GetComponent<GroundRenderer>();
        }


        public CoinBehavior GetCoin()
        {
            return coinPool.GetPooledObject().GetComponent<CoinBehavior>();
        }


        public Transform GetProp(Item item)
        {
            return propPools[item].GetPooledObject().transform;
        }


        public Transform GetTape()
        {
            return tapePool.GetPooledObject().transform;
        }
    }
}