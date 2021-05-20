using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    [CreateAssetMenu(fileName = "TileMapData", menuName = "Configs/TileMapConfig", order = 1)]
    public class TileMapConfig : ScriptableObject
    {
        [SerializeField]
        List<TerrainType> terrainTypes = new List<TerrainType>();

        [SerializeField]
        Vector2Int mapSize;

        [SerializeField]
        GameObject tilePrefab;

        [SerializeField]
        float tileOuterRadius;

        [SerializeField]
        float tileInnerRadius;

        public IEnumerable<TerrainType> TerrainTypes { get => terrainTypes; }

        public Vector2Int MapSize { get => mapSize; }

        public GameObject TilePrefab { get => tilePrefab; }

        public float TileOuterRadius { get => tileOuterRadius; }

        public float TileInnerRadius { get => tileInnerRadius; }
    }
}
