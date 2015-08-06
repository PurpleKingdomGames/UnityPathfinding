using UnityEngine;
using PurpleKingdomGames.Core.Pathfinding;
using PurpleKingdomGames.Unity.Core;

namespace PurpleKingdomGames.Unity.Components
{
    public class GridBuilder2DComponent : MonoBehaviour
    {
		public LayerMask LayerMask = -1;
		public float NodeSize = 0.5f;
		public float Width = 10;
		public float Height = 10;

		[HideInInspector]
		public GridNode2D[,] Grid;

        // Use this for initialization
        private void Start()
        {
			Recalculate ();
        }

        // Update is called once per frame
        public void Update()
        {
            // TODO: Show a grid in the editor window (only when game isn't running)
            // TODO: Allow the grid to be saved to a file
            // TODO: Allow the grid to be loaded from a file
			// TODO: Allow the Seeker to seek Asynchronously
			Recalculate ();
		}
		
		public void Recalculate()
		{
			Vector2 position = transform.position;
			position.x -= Width / 2;
			position.y -= Height / 2;

			Rect bounds = new Rect (position, new Vector2 (Width, Height));
			Grid = GridBuilder2D.BuildGrid (bounds, NodeSize, LayerMask);
		}
    }
}
