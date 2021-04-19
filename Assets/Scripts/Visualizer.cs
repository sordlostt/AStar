using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathing
{
    public class Visualizer : MonoBehaviour
    {
        [SerializeField]
        GameObject pathIndicator;

        Tile nodeA;
        Tile nodeB;

        Billboard nodeABillboard;
        Billboard nodeBBillboard;

        List<GameObject> indicators = new List<GameObject>();

        private void Update()
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(cameraRay, out hit, float.PositiveInfinity, LayerMask.GetMask("Tile")) && Input.GetMouseButtonDown(0))
            {
                Tile tile = hit.transform.gameObject.GetComponentInParent<Tile>();

                if (tile != null && !tile.IsWater())
                {
                    if (nodeA == null)
                    {
                        nodeA = tile;
                        nodeABillboard = tile.GetComponentInChildren<Billboard>();
                        nodeABillboard.SetText("A");
                    }
                    else if (nodeB == null)
                    {
                        if (nodeA != tile)
                        {
                            nodeB = tile;
                            var path = AStar.GetPath(nodeA, nodeB);
                            if (path != null)
                            {
                                nodeBBillboard = nodeB.GetComponentInChildren<Billboard>();
                                nodeBBillboard.SetText("B");
                                foreach (Tile node in path)
                                {
                                    indicators.Add(Instantiate(pathIndicator, node.transform.position, Quaternion.identity));
                                }
                            }
                            else
                            {
                                nodeB = null;
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject indicator in indicators)
                        {
                            Destroy(indicator);
                        }
                        indicators.Clear();

                        nodeABillboard.DisableText();
                        nodeBBillboard.DisableText();

                        nodeA = tile;
                        nodeB = null;
                        nodeBBillboard = null;

                        nodeABillboard = tile.GetComponentInChildren<Billboard>();
                        nodeABillboard.SetText("A");
                    }
                }
            }
        }
    }
}
