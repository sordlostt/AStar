using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class TileMap : MonoBehaviour
    {
        public static TileMap instance = null;

        class TileGrid
        {
            Dictionary<Vector2, Tile> grid = new Dictionary<Vector2, Tile>();

            public void AddTile(Tile tile)
            {
                grid.Add(tile.GetCoordinates(), tile);
            }

            public Tile GetTile(float x, float y)
            {
                Tile result;
                grid.TryGetValue(new Vector2(x, y), out result);
                return result;
            }

            public IEnumerable<Tile> GetNeighbours(Tile tile)
            {
                List<Tile> neighbours = new List<Tile>();
                Vector2 tileCoords = tile.GetCoordinates();

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

        [System.Serializable]
        public struct TerrainType
        {
            public Texture2D texture;
            public float cost;
        }

        [SerializeField]
        int mapSizeX;
        [SerializeField]
        int mapSizeY;
        [SerializeField]
        List<TerrainType> terrainTypes = new List<TerrainType>();
        [SerializeField]
        GameObject tilePrefab;
        [SerializeField]
        float tileOuterRadius;
        [SerializeField]
        float tileInnerRadius;


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
            for (int y = 0; y < mapSizeY; y++)
            {
                for (int x = 0; x < mapSizeX; x++)
                {
                    TerrainType terrainType = terrainTypes[Random.Range(0, terrainTypes.Count)];
                    GameObject tileObj = InsertHex(x, y);
                    Tile tile = tileObj.GetComponent<Tile>();
                    tileObj.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", terrainType.texture);
                    tile.SetCoordinates(x, y);
                    tile.SetCost(terrainType.cost);
                    tileGrid.AddTile(tile);
                }
            }
        }

        private GameObject InsertHex(int x, int z)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (tileInnerRadius * 2f);
            position.y = 0f;
            position.z = z * (tileOuterRadius * 1.5f);

            return Instantiate<GameObject>(tilePrefab, position, Quaternion.identity);
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

