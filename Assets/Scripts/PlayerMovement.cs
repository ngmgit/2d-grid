using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerStats mPlayerStats;
	public Vector2 currentPosInGrid;
	public SetSpawnPositions spawnPosition;

	private Vector3 nextPosition;
	private Vector3 prevPostion;
	private Vector2 targetDir;
	private Vector2 prevTargetDir;
	private float currentSpeed;

	void Start()
	{
		currentSpeed = mPlayerStats.speed;
		targetDir = Vector2.up;
		MovePlayerToSpawn(spawnPosition.spawnPosInGrid);
		SetCurrentGridPosition();
		SetPosition();
		MoveToPosition();
	}

	private void MovePlayerToSpawn(Vector2 mPosition)
	{
		Vector2 SpawnPos = Camera.main.ScreenToWorldPoint(mPosition);
		transform.position = new Vector3(SpawnPos.x, SpawnPos.y, 0);
		prevPostion = SpawnPos;
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
		ScreenWrapPlayer();

		Vector2 screenPos = Camera.main.WorldToScreenPoint(prevPostion);

		Vector3 nextScreenPos;
		nextScreenPos.x = screenPos.x + (mPlayerStats.grid.sizeInPixel.x * targetDir.x);
		nextScreenPos.y = screenPos.y + (mPlayerStats.grid.sizeInPixel.y * targetDir.y);
		nextScreenPos.z = 0;

		Vector3 nextWorldPos = Camera.main.ScreenToWorldPoint(nextScreenPos);
		nextPosition = nextWorldPos;
		nextPosition.z = 0;
	}

	private void ScreenWrapPlayer() {
		if (CheckIfPlayerOutside())
		{
			Vector2 swapScreenPos;

			switch (mPlayerStats.dir) {
				case mDirection.UP:
					swapScreenPos = new Vector2 (
						currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x,
						- mPlayerStats.grid.sizeInPixel.y / 2);

					transform.position = Camera.main.ScreenToWorldPoint(swapScreenPos);

					swapScreenPos = new Vector2 (
						currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x,
						- mPlayerStats.grid.sizeInPixel.y);

					nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
					break;

				case mDirection.DOWN:
					swapScreenPos = new Vector2 (
						currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x,
						(mPlayerStats.grid.rows + 0.5f) * mPlayerStats.grid.sizeInPixel.y);

					transform.position = Camera.main.ScreenToWorldPoint(swapScreenPos);

					swapScreenPos = new Vector2 (
						currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x,
						(mPlayerStats.grid.rows + 1) * mPlayerStats.grid.sizeInPixel.y);

					nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
					break;

				case mDirection.RIGHT:
					swapScreenPos = new Vector2 (
						- mPlayerStats.grid.sizeInPixel.x / 2,
						currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);

					transform.position = Camera.main.ScreenToWorldPoint(swapScreenPos);

					swapScreenPos = new Vector2 (
						- mPlayerStats.grid.sizeInPixel.x,
						currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);

					nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
					break;

				case mDirection.LEFT:
					swapScreenPos = new Vector2 (
						(mPlayerStats.grid.columns + 0.5f) * mPlayerStats.grid.sizeInPixel.x,
						currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);

					transform.position = Camera.main.ScreenToWorldPoint(swapScreenPos);

					swapScreenPos = new Vector2 (
						(mPlayerStats.grid.columns + 1) * mPlayerStats.grid.sizeInPixel.x,
						currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);

					nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
					break;
			}
		}
	}

	private bool CheckIfPlayerOutside () {
		if ((transform.position.x > mPlayerStats.mbounds.lower.x
			&& transform.position.x < mPlayerStats.mbounds.upper.x)
			&&
			(transform.position.y > mPlayerStats.mbounds.lower.y
			&& transform.position.y < mPlayerStats.mbounds.upper.y))
		{
			return false;
		}

		return true;
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
