using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Player/State And Info")]
public class PlayerStats : ScriptableObject {
	public mDirection dir;
	public Vector2 dirVector;
	public float speed;
	public float dashSpeed;
	public float rotateSpeed;
	public int score;

	[System.Serializable]
	public struct MyGrid {
		public Vector2 sizeInPixel;
		public int rows;
		public int columns;
	}

	[System.Serializable]
	public struct MyBounds {
		public Vector2 lower;
		public Vector2 upper;
	}

	public MyBounds mbounds;
	public MyGrid grid;
	public Vector2 distance;
}
