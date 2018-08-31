using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapManager : MonoBehaviour {
	public PlayerStats mPlayerStats;
	public PlayerMovement playerCopy1;
	public PlayerMovement playerCopy2;

	public PlayerMovement currentCopyActive;
	private Vector2 newPosinGrid;

	private void Awake() {

		playerCopy1.spawnPosition.spawnPosInGrid = new Vector2 (
			mPlayerStats.grid.sizeInPixel.x * 3,
			mPlayerStats.grid.sizeInPixel.y * 4);

		playerCopy2.spawnPosition.spawnPosInGrid = new Vector2 (
			mPlayerStats.grid.sizeInPixel.x * 3,
			mPlayerStats.grid.sizeInPixel.y * (4 +  mPlayerStats.grid.rows));

		currentCopyActive = playerCopy1;
	}

	// Update is called once per frame
	void Update ()
	{
		CheckAndSpawn();
	}

	private void CheckAndSpawn() {
		if (CheckBoundsForCurrectActive ()) {
			SetPlayerClone();
		}

		if (currentCopyActive.currentPosInGrid.x < -1 ||
			currentCopyActive.currentPosInGrid.x > mPlayerStats.grid.columns + 1 ||
			currentCopyActive.currentPosInGrid.y < -1 ||
			currentCopyActive.currentPosInGrid.y > mPlayerStats.grid.rows + 1)
		{
			PlayerMovement tempActive;

			if (currentCopyActive == playerCopy1) {
				tempActive = playerCopy2;
			} else {
				tempActive = playerCopy1;
			}

			currentCopyActive = tempActive;
		}
	}

	private bool CheckBoundsForCurrectActive()
	{
		return
		currentCopyActive.currentPosInGrid.x < 1 ||
		currentCopyActive.currentPosInGrid.x > mPlayerStats.grid.columns - 1||
		currentCopyActive.currentPosInGrid.y < 1 ||
		currentCopyActive.currentPosInGrid.y > mPlayerStats.grid.rows - 1;
	}

	private void SetPlayerClone()
	{
		if (currentCopyActive == playerCopy1)
		{
			SetNewGridPosForClone();
			newPosinGrid = new Vector2(mPlayerStats.grid.sizeInPixel.x * newPosinGrid.x,
										mPlayerStats.grid.sizeInPixel.y * newPosinGrid.y);
			playerCopy2.spawnPosition.spawnPosInGrid = newPosinGrid;
		} else {
			SetNewGridPosForClone();
			newPosinGrid = new Vector2(mPlayerStats.grid.sizeInPixel.x * newPosinGrid.x,
										mPlayerStats.grid.sizeInPixel.y * newPosinGrid.y);
			playerCopy1.spawnPosition.spawnPosInGrid = newPosinGrid;
		}
	}

	void SetNewGridPosForClone ()
	{
		if (Mathf.Abs(mPlayerStats.dirVector.x) == 1) {

			if (currentCopyActive.currentPosInGrid.x == 1 + Mathf.Epsilon) {
				newPosinGrid.x = -1;
			} else {
				newPosinGrid.x = mPlayerStats.grid.columns + 1;
			}
			newPosinGrid.y = currentCopyActive.currentPosInGrid.y;

		} else {

			if (currentCopyActive.currentPosInGrid.y == 1 + Mathf.Epsilon) {
				newPosinGrid.y = -1;
			} else {
				newPosinGrid.y = mPlayerStats.grid.rows + 1;
			}

			newPosinGrid.x = currentCopyActive.currentPosInGrid.x;
		}
	}
}
