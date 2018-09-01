using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerStats mPlayerStats;
	public SetSpawnPositions spawnPosition;

	private Vector2 currentPosInGrid;
	private Vector3 nextPosition;
	private Vector3 prevPostion;
	private Vector2 targetDir;
	private Vector2 prevTargetDir;
	private float currentSpeed;
	private readonly int playerZ = 0;

	private void Start()
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
	private void Update ()
	{
		SetCurrentGridPosition();
		MovePlayer ();
		SetPlayerDirection ();

		prevTargetDir = targetDir;
	}

	private void SetCurrentGridPosition()
	{
		Vector2 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		currentPosInGrid = new Vector2 (currentScreenPos.x / mPlayerStats.grid.sizeInPixel.x,
										currentScreenPos.y / mPlayerStats.grid.sizeInPixel.y);
	}

	private void MovePlayer()
	{
		bool isOutside = GameController.CheckIfPlayerOutside(transform.position, mPlayerStats, 0.5f);
		bool isPrevPosOutside = GameController.CheckIfPrevPosInGrid(prevPostion, mPlayerStats);

		if (isPrevPosOutside && isOutside)
			ScreenWrapPlayer();

		if (CanChangePosition()) {
			currentSpeed = mPlayerStats.speed;
			SetPosition();
		}

		MoveToPosition();
	}

	private bool CanChangePosition ()
	{
		float dist  = Vector2.Distance(transform.position, nextPosition);
		return dist <= 0 ;
	}

	private void MoveToPosition()
	{
		transform.position = Vector3.MoveTowards(transform.position, nextPosition, currentSpeed * Time.deltaTime);
	}

	private void SetPosition()
	{
		prevPostion = nextPosition;
		prevPostion.z = playerZ;

		Vector2 screenPos = Camera.main.WorldToScreenPoint(prevPostion);

		Vector3 nextScreenPos;
		nextScreenPos.x = screenPos.x + (mPlayerStats.grid.sizeInPixel.x * targetDir.x);
		nextScreenPos.y = screenPos.y + (mPlayerStats.grid.sizeInPixel.y * targetDir.y);
		nextScreenPos.z = playerZ;

		Vector3 nextWorldPos = Camera.main.ScreenToWorldPoint(nextScreenPos);
		nextPosition = nextWorldPos;
		nextPosition.z = playerZ;
	}

	private void ScreenWrapPlayer()
	{
		Vector2 swapScreenPos;

		// In each section new current, prev, next position for the player is set if he is outside the bounds.
		switch (mPlayerStats.dir) {
			case mDirection.UP:
				swapScreenPos = new Vector2 (currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x,
									- mPlayerStats.grid.sizeInPixel.y / 2);
				SetPlayerWrapPosition(swapScreenPos);
				SetNextAndPrevPositions(new Vector2 (currentPosInGrid.x, -1));
				break;

			case mDirection.DOWN:
				swapScreenPos = new Vector2 (currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x,
									(mPlayerStats.grid.rows + 0.5f) * mPlayerStats.grid.sizeInPixel.y);
				SetPlayerWrapPosition(swapScreenPos);
				SetNextAndPrevPositions(new Vector2 (currentPosInGrid.x, mPlayerStats.grid.rows + 1));
				break;

			case mDirection.RIGHT:
				swapScreenPos = new Vector2 (- mPlayerStats.grid.sizeInPixel.x / 2,
									currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);
				SetPlayerWrapPosition(swapScreenPos);
				SetNextAndPrevPositions(new Vector2 (-1, currentPosInGrid.y));
				break;

			case mDirection.LEFT:
				swapScreenPos = new Vector2 ((mPlayerStats.grid.columns + 0.5f) * mPlayerStats.grid.sizeInPixel.x,
									currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);
				SetPlayerWrapPosition(swapScreenPos);
				SetNextAndPrevPositions(new Vector2 (mPlayerStats.grid.columns + 1, currentPosInGrid.y));
				break;
		}
	}

	private void SetPlayerWrapPosition(Vector2 swapScreenPos)
	{
		Vector2 swapWorldPos = Camera.main.ScreenToWorldPoint(swapScreenPos);
		transform.position = new Vector3 (swapWorldPos.x, swapWorldPos.y, playerZ);
	}

	private void SetNextAndPrevPositions(Vector2 gridPosiForPrev)
	{
		Vector2 swapScreenPos;
		Vector2 nextGridPos;

		int rows = mPlayerStats.grid.rows;
		int columns = mPlayerStats.grid.columns;

		swapScreenPos = new Vector2 (gridPosiForPrev.x * mPlayerStats.grid.sizeInPixel.x,
							gridPosiForPrev.y * mPlayerStats.grid.sizeInPixel.y);
		prevPostion = Camera.main.ScreenToWorldPoint(swapScreenPos);
		prevPostion.z = playerZ;

		nextGridPos = new Vector2(Mathf.Clamp(gridPosiForPrev.x, 0, columns), Mathf.Clamp(gridPosiForPrev.y, 0, rows));
		swapScreenPos = new Vector2 (nextGridPos.x * mPlayerStats.grid.sizeInPixel.x,
							nextGridPos.y * mPlayerStats.grid.sizeInPixel.y);
		nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
		nextPosition.z = playerZ;
	}

	private void SetPlayerDirection()
	{

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

		if (Vector2.Dot(targetDir, prevTargetDir) == 0 && currentSpeed != mPlayerStats.dashSpeed) {
			currentSpeed = mPlayerStats.dashSpeed;
		}

		if (Vector2.Dot(targetDir, prevTargetDir) == -1)
			SetPosition();
	}
}
