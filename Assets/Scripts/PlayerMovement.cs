using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerStats mPlayerStats;
	public Vector2 currentPosInGrid;

	private Vector3 nextPosition;
	private Vector3 prevPostion;
	private Vector2 targetDir;
	private Vector2 prevTargetDir;
	private float currentSpeed;

	void Start()
	{
		currentSpeed = mPlayerStats.speed;
		targetDir = Vector2.up;
		Vector2 SpawnPos = Camera.main.ScreenToWorldPoint(
							new Vector2(mPlayerStats.grid.sizeInPixel.x * 3,
										mPlayerStats.grid.sizeInPixel.y * 4));

		transform.position = new Vector3(SpawnPos.x, SpawnPos.y, 0);
		nextPosition = SpawnPos;

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
		ChangeSpeed();

		prevTargetDir = targetDir;
		prevPostion = nextPosition;
	}

	private void SetCurrentGridPosition()
	{
		Vector2 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		currentPosInGrid = new Vector2 (currentScreenPos.x / mPlayerStats.grid.sizeInPixel.x,
										currentScreenPos.y / mPlayerStats.grid.sizeInPixel.y);
	}

	private void MovePlayer()
	{
		if (CanChangePosition()) {
			currentSpeed = mPlayerStats.speed;
			SetPosition();
		}

		MoveToPosition();
	}

	private bool CanChangePosition () {
		return Vector2.Distance(transform.position, nextPosition) <= 0;
	}

	private void MoveToPosition()
	{
		transform.position = Vector3.MoveTowards(transform.position, nextPosition, currentSpeed * Time.deltaTime);
	}

	private void SetPosition()
	{
		Vector2 screenPos = Camera.main.WorldToScreenPoint(nextPosition);

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
				SetDirectionAndPosition(Vector2.up);
				break;

			case mDirection.DOWN:
				SetDirectionAndPosition(Vector2.down);
				break;

			case mDirection.RIGHT:
				SetDirectionAndPosition(Vector2.right);
				break;

			case mDirection.LEFT:
				SetDirectionAndPosition(Vector2.left);
				break;
		}
	}

	private void SetDirectionAndPosition(Vector2 dir)
	{
		targetDir = dir;

		if (Vector2.Dot(targetDir, prevTargetDir) == -1)
			SetPosition();
	}

	private void ChangeSpeed()
	{
		if (Vector2.Dot(targetDir, prevTargetDir) == 0) {
			currentSpeed = mPlayerStats.dashSpeed;
		}
	}
}
