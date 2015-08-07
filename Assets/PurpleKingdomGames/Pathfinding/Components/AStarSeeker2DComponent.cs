using UnityEngine;
using System.Threading;
using PurpleKingdomGames.Core;
using PurpleKingdomGames.Core.Pathfinding;
using PurpleKingdomGames.Core.Pathfinding.Seekers;

namespace PurpleKingdomGames.Unity.Components
{
	public class AStarSeeker2DComponent : MonoBehaviour {
		public enum StartPositionType
		{
			Top, Middle, Bottom
		}

		public bool IsReady {
			get { return GridBuilder.Grid != null; }
		}

		public StartPositionType StartPathFrom = StartPositionType.Middle;
		public GridBuilder2DComponent GridBuilder;

		public Vector2[] Seek(Vector2 target)
		{
			return Seek (GetStartPosition(), target);
		}

		public Vector2[] Seek(Vector2 start, Vector2 target)
		{
			if (!IsReady) {
				throw new UnassignedReferenceException("Grid builder component has not initialised");
			}

			// Get the offset for the grid
			float offsetX = GridBuilder.transform.position.x - (GridBuilder.Width / 2);
			float offsetY = GridBuilder.transform.position.y + (GridBuilder.Height / 2);
			
			Point2D startPoint = new Point2D (
				Mathf.Ceil (Mathf.Abs (offsetX - start.x) / GridBuilder.NodeSize),
				Mathf.Ceil (Mathf.Abs(offsetY - start.y) / GridBuilder.NodeSize)
			);

			Point2D endPoint = new Point2D (
				Mathf.Ceil (Mathf.Abs(offsetX - target.x) / GridBuilder.NodeSize),
				Mathf.Ceil (Mathf.Abs(offsetY - target.y) / GridBuilder.NodeSize)
			);
			
			Point2D[] path = AStar.Seek (GridBuilder.Grid, startPoint, endPoint);
			if (path == null) {
				return new Vector2[0];
			}

			Vector2[] returnPath = new Vector2 [path.Length];
			for (int i = 0; i < path.Length; i++) {
				returnPath[i] = new Vector2(
					(path[i].X * GridBuilder.NodeSize) + offsetX,
					(path[i].Y * GridBuilder.NodeSize) + offsetY
				);
			}

			return returnPath;
		}

		public Vector2[] SeekAsync(Vector2 target)
		{
			return SeekAsync (GetStartPosition(), target);
		}

		public Vector2[] SeekAsync(Vector2 start, Vector2 target)
		{
			return Seek (start, target);
		}

		public Vector2 GetStartPosition()
		{
			Vector2 position = transform.position;
			Renderer renderer = gameObject.GetComponent<Renderer> ();

			if (StartPathFrom == StartPositionType.Top && renderer != null ) {
				position.y = renderer.bounds.max.y;
			}

			if (StartPathFrom == StartPositionType.Bottom) {
				position.y = renderer.bounds.min.y;
			}

			return position;
		}
	}
}
