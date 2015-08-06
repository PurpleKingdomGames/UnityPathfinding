using UnityEngine;
using System.Collections.Generic;
using PurpleKingdomGames.Core;
using PurpleKingdomGames.Core.Pathfinding;

namespace PurpleKingdomGames.Unity.Core {
    public static class GridBuilder2D
    {
        public static GridNode2D[,] BuildGrid(Rect bounds, float nodeSize)
        {
            return BuildGrid(bounds, nodeSize, Physics2D.AllLayers, new Dictionary<string, int>());
        }

        public static GridNode2D[,] BuildGrid(Rect bounds, float nodeSize, Dictionary<string, int> tagPenalties)
        {
            return BuildGrid(bounds, nodeSize, Physics2D.AllLayers, tagPenalties);
        }

        public static GridNode2D[,] BuildGrid(Rect bounds, float nodeSize, LayerMask layerMask)
        {
            return BuildGrid(bounds, nodeSize, layerMask, new Dictionary<string, int>());
        }

        public static GridNode2D[,] BuildGrid(Rect bounds, float nodeSize, LayerMask layerMask, Dictionary<string, int> tagPenalties)
        {
            float xDistance = bounds.xMax - bounds.xMin;
            float yDistance = bounds.yMax - bounds.yMin;
            int gridX = (int) Mathf.Ceil(xDistance / nodeSize);
            int gridY = (int) Mathf.Ceil(yDistance / nodeSize);

            GridNode2D[,] returnVal = new GridNode2D[gridX, gridY];
            int nodeY = 0;

            for (float y = bounds.yMax; y > bounds.yMin; y -= nodeSize) {
                // trace ray from left to right
                float distanceSet = 0;
                int nodeX = 0;
                while (distanceSet <= xDistance) {
                    float distanceLeft = xDistance - distanceSet;
                    Vector2 origin = new Vector2(bounds.xMin + distanceSet, y);
                    RaycastHit2D rayCast = Physics2D.Raycast(origin, Vector2.right, distanceLeft, layerMask);

                    // If it hits something then mark each entry up to that point as passable
                    float hitPoint = Mathf.Floor(rayCast.distance / nodeSize) * nodeSize;
                    if (rayCast.collider == null) {
                        hitPoint = distanceLeft;
                    }

                    Debug.DrawRay(origin, Vector2.right * hitPoint);

                    for (float i = 0; i < hitPoint; i += nodeSize) {
                        returnVal[nodeX, nodeY] = new GridNode2D(new Point2D(xDistance + i, y)) {
                            Passable = true,
                        };
                        nodeX++;
                    }

                    // Mark the point it hit as impassable
                    distanceSet += (hitPoint + nodeSize);
                    if (distanceSet <= xDistance && rayCast.collider == null) {
                        nodeX++;
                        bool passable = false;
                        int penalty = 0;

                        if (tagPenalties.ContainsKey(rayCast.collider.tag)) {
                            penalty = tagPenalties[rayCast.collider.tag];
                            passable = true;
                        }

                        returnVal[nodeX, nodeY] = new GridNode2D(new Point2D(0, y)) {
                            Passable = passable,
                            Penalty = penalty
                        };
                    }
                    // Repeat until there is no distance to go
                }

                nodeY++;
            }

            return returnVal;
        }
    }
}
