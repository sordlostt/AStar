using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class Tile : MonoBehaviour, IAStarNode
    {
        // offset coordinates
        Vector2 coordinates;

        float cost;

        public IEnumerable<IAStarNode> Neighbours
        {
            get { return TileMap.instance.GetNeighboursForTile(this); }
        }

        public float CostTo(IAStarNode neighbour)
        {
            Tile tile = (Tile)neighbour;
            return tile.GetCost();
        }

        // estimated cost based on manhattan heuristics modified to accomodate 6-directional movement
        public float EstimatedCostTo(IAStarNode goal)
        {
            Tile destination = (Tile)goal;
            Vector2 destinationCoords = destination.GetCoordinates();
            Vector3 thisCoordsCube = OffsetToCube(coordinates);
            Vector3 destinationCoordsCube = OffsetToCube(destinationCoords);
            var dist = (Mathf.Abs(destinationCoordsCube.x - thisCoordsCube.x) + Mathf.Abs(destinationCoordsCube.y - thisCoordsCube.y) + Mathf.Abs(destinationCoordsCube.z - thisCoordsCube.z)) / 2;
            return dist;
        }

        // converts offset coordinates to cube coordinates
        private Vector3 OffsetToCube(Vector2 coords)
        {
            float cubeX = coords.x - (coords.y - ((int)coords.x & 1)) / 2;
            float cubeZ = coords.y;
            float cubeY = -cubeX - cubeZ;
            return new Vector3(cubeX, cubeY, cubeZ);
        }

        public void SetCost(float cost)
        {
            this.cost = cost;
        }

        public float GetCost()
        {
            return cost;
        }

        // water has a cost smaller than zero
        public bool IsWater()
        {
            return cost < 0.0f;
        }

        public Vector2 GetCoordinates()
        {
            return coordinates;
        }

        public void SetCoordinates(float x, float y)
        {
            this.coordinates = new Vector2(x, y);
        }
    }
}
