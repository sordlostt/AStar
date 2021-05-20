using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    [System.Serializable]
    public struct TerrainType
    {
        [SerializeField]
        Texture2D texture;

        [SerializeField]
        int cost;

        public Texture2D Texture { get => texture; }
        public int Cost { get => cost; }
    }

    public class TileMap : MonoBehaviour
    {
        public static TileMap instance = null;

        class TileGrid
        {
            Dictionary<Vector2Int, Tile> grid = new Dictionary<Vector2Int, Tile>();

            public void AddTile(Tile tile)
            {
                grid.Add(tile.Coordinates, tile);
            }

            public Tile GetTile(int x, int y)
            {
                Tile result;
                grid.TryGetValue(new Vector2Int(x, y), out result);
                return result;
            }

            public IEnumerable<Tile> GetNeighbours(Tile tile)
            {
                List<Tile> neighbours = new List<Tile>();
                Vector2Int tileCoords = tile.Coordinates;

                neighbours.Add(GetTile(tileCoords.x + 1, tileCoords.y));
                neighbours.Add(GetTile(tileCoords.x - 1, tileCoords.y));

                if (tileCoords.y % 2 == 0)
                {
                    neighbours.Add(GetTile(tileCoords.x - 1, tileCoords.y + 1));
                    neighbours.Add(GetTile(tileCoords.x, tileCoords.y + 1));
                    neighbours.Add(GetTile(tileCoords.x - 1, tileCoords.y - 1));
                    neighbours.Add(GetTile(tileCoords.x, tileCoords.y - 1));
                }
                else
                {
                    neighbours.Add(GetTile(tileCoords.x, tileCoords.y + 1));
                    neighbours.Add(GetTile(tileCoords.x + 1, tileCoords.y + 1));
                    neighbours.Add(GetTile(tileCoords.x, tileCoords.y - 1));
                    neighbours.Add(GetTile(tileCoords.x + 1, tileCoords.y - 1));
                }
                return neighbours;
            }
        }

        [SerializeField]
        TileMapConfig mapConfig;

        TileGrid tileGrid = new TileGrid();


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }

            GenerateMap();
        }

        private void GenerateMap()
        {
            var terrainTypes = mapConfig.TerrainTypes as List<TerrainType>;

            for (int y = 0; y < mapConfig.MapSize.y; y++)
            {
                for (int x = 0; x < mapConfig.MapSize.x; x++)
                {
                    TerrainType terrainType = terrainTypes[Random.Range(0, terrainTypes.Count)];
                    GameObject tileObj = InsertHex(x, y);
                    Tile tile = tileObj.GetComponent<Tile>();
                    tileObj.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", terrainType.Texture);
                    tile.Coordinates = new Vector2Int(x, y);
                    tile.Cost = terrainType.Cost;
                    tileGrid.AddTile(tile);
                }
            }
        }

        private GameObject InsertHex(int x, int z)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (mapConfig.TileInnerRadius * 2f);
            position.y = 0f;
            position.z = z * (mapConfig.TileOuterRadius * 1.5f);

            return Instantiate<GameObject>(mapConfig.TilePrefab, position, Quaternion.identity);
        }

        public IEnumerable<Tile> GetNeighboursForTile(Tile tile)
        {
            List<Tile> neighbours = tileGrid.GetNeighbours(tile) as List<Tile>;
            List<Tile> neighboursNew = new List<Tile>(neighbours);

            foreach (Tile neighbour in neighbours)
            {
                if (neighbour == null || neighbour.IsWater())
                {
                    neighboursNew.Remove(neighbour);
                } 
            }

            return neighboursNew;
        }
    }
}

