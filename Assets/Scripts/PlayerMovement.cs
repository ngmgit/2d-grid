using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerStats mPlayerStats;
	public Vector2 currentPosInGrid;
	public Vector3 nextPosition;

	private Vector2 targetDir;

	void Start()
	{
		targetDir = Vector2.up;
		Vector2 SpawnPos = Camera.main.ScreenToWorldPoint(
							new Vector2(mPlayerStats.grid.sizeInPixel.x * 3,
										mPlayerStats.grid.sizeInPixel.y * 4));

		transform.position = new Vector3(SpawnPos.x, SpawnPos.y, 0);

		SetCurrentGridPosition();
		SetPosition();
		MoveToPosition();
	}

	// Update is called once per frame
	void Update ()
	{
		SetCurrentGridPosition();
		MovePlayer ();
		SetPlayerDirection ();
	}

	private void MovePlayer()
	{
		if (Vector2.Distance(transform.position, nextPosition) <= 0) {
			SetPosition();
		}

		MoveToPosition();
	}

	private void MoveToPosition()
	{
		transform.position = Vector3.MoveTowards(transform.position, nextPosition, mPlayerStats.speed * Time.deltaTime);
	}

	private void SetPosition()
	{
		Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);

		Vector3 nextScreenPos;
		nextScreenPos.x = screenPos.x + (mPlayerStats.grid.sizeInPixel.x * targetDir.x);
		nextScreenPos.y = screenPos.y + (mPlayerStats.grid.sizeInPixel.y * targetDir.y);
		nextScreenPos.z = 0;

		Vector3 nextWorldPos = Camera.main.ScreenToWorldPoint(nextScreenPos);

		nextPosition = nextWorldPos;
		nextPosition.z = 0;
	}

	private void SetPlayerDirection()
	{
		//float step = mPlayerStats.rotateSpeed * Time.deltaTime;
		switch (mPlayerStats.dir)
		{
			case mDirection.UP:
				targetDir = Vector2.up;
				break;

			case mDirection.DOWN:
				targetDir = Vector2.down;
				break;

			case mDirection.RIGHT:
				targetDir = Vector2.right;
				break;

			case mDirection.LEFT:
				targetDir = Vector2.left;
				break;
		}
	}

	private void SetCurrentGridPosition()
	{
		Vector2 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		currentPosInGrid = new Vector2 (currentScreenPos.x / mPlayerStats.grid.sizeInPixel.x,
										currentScreenPos.y / mPlayerStats.grid.sizeInPixel.y);
	}
}
