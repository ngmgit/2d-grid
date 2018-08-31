using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerStats mPlayerStats;
	public Vector2 currentPosInGrid;
	public SetSpawnPositions spawnPosition;

	[SerializeField]
	private Vector3 nextPosition;
	[SerializeField]
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
	}

	private void SetCurrentGridPosition()
	{
		Vector2 currentScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		currentPosInGrid = new Vector2 (currentScreenPos.x / mPlayerStats.grid.sizeInPixel.x,
										currentScreenPos.y / mPlayerStats.grid.sizeInPixel.y);
	}

	private void MovePlayer()
	{
		if (CheckIfPrevPosInGrid() && CheckIfPlayerOutside())
			ScreenWrapPlayer();

		if (CanChangePosition()) {
			currentSpeed = mPlayerStats.speed;
			SetPosition();
		}

		MoveToPosition();
	}

	private bool CanChangePosition () {
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
		prevPostion.z = 0;

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
				prevPostion = Camera.main.ScreenToWorldPoint(swapScreenPos);
				prevPostion.z = 0;

				swapScreenPos = new Vector2 (currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x, 0);
				nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
				nextPosition.z = 0;
				break;

			case mDirection.DOWN:
				swapScreenPos = new Vector2 (
					currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x,
					(mPlayerStats.grid.rows + 0.5f) * mPlayerStats.grid.sizeInPixel.y);
				transform.position = Camera.main.ScreenToWorldPoint(swapScreenPos);

				swapScreenPos = new Vector2 (
					currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x,
					(mPlayerStats.grid.rows + 1) * mPlayerStats.grid.sizeInPixel.y);
				prevPostion = Camera.main.ScreenToWorldPoint(swapScreenPos);
				prevPostion.z = 0;

				swapScreenPos = new Vector2 (currentPosInGrid.x * mPlayerStats.grid.sizeInPixel.x, Screen.height);
				nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
				nextPosition.z = 0;
				break;

			case mDirection.RIGHT:
				swapScreenPos = new Vector2 (
					- mPlayerStats.grid.sizeInPixel.x / 2,
					currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);
				transform.position = Camera.main.ScreenToWorldPoint(swapScreenPos);

				swapScreenPos = new Vector2 (
					- mPlayerStats.grid.sizeInPixel.x,
					currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);
				prevPostion = Camera.main.ScreenToWorldPoint(swapScreenPos);
				prevPostion.z = 0;

				swapScreenPos = new Vector2 (0, currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);
				nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
				nextPosition.z = 0;
				break;

			case mDirection.LEFT:
				swapScreenPos = new Vector2 (
					(mPlayerStats.grid.columns + 0.5f) * mPlayerStats.grid.sizeInPixel.x,
					currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);
				transform.position = Camera.main.ScreenToWorldPoint(swapScreenPos);

				swapScreenPos = new Vector2 (
					(mPlayerStats.grid.columns + 1) * mPlayerStats.grid.sizeInPixel.x,
					currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);
				prevPostion = Camera.main.ScreenToWorldPoint(swapScreenPos);
				prevPostion.z = 0;

				swapScreenPos = new Vector2 (Screen.width, currentPosInGrid.y * mPlayerStats.grid.sizeInPixel.y);
				nextPosition = Camera.main.ScreenToWorldPoint(swapScreenPos);
				nextPosition.z = 0;
				break;
		}
	}

	private bool CheckIfPlayerOutside () {

		PlayerStats.MyBounds newBounds = GameController.GetScreenToWorld(0.5f, mPlayerStats);

		if ((transform.position.x >= newBounds.lower.x
			&& transform.position.x <= newBounds.upper.x)
			&&
			(transform.position.y >= newBounds.lower.y
			&& transform.position.y <= newBounds.upper.y))
		{
			return false;
		}

		return true;
	}

	private bool CheckIfPrevPosInGrid () {
		if (Mathf.Approximately(prevPostion.x, mPlayerStats.mbounds.lower.x)||
			Mathf.Approximately(prevPostion.x, mPlayerStats.mbounds.upper.x)||
			Mathf.Approximately(prevPostion.y, mPlayerStats.mbounds.lower.y)||
			Mathf.Approximately(prevPostion.y, mPlayerStats.mbounds.upper.y))
		{
			return true;
		}

		return false;
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
