using UnityEngine;
using System;
using PurpleKingdomGames.Unity.Components;

public class Platform2DPlayerComponent : MonoBehaviour {

	private AStarSeeker2DComponent seeker;
	private Vector2[] path;

	// Use this for initialization
	private void Update () {
		if (seeker == null) {
			seeker = GetComponent<AStarSeeker2DComponent> ();
		}

		if (path == null && seeker.IsReady) {
			path = seeker.Seek (new Vector2 (7.9f, -5.7f));
		}
	}
}
