using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public PlayerStats mPlayerStats;
	public GridLayoutGroup gridGrpBg;
	public SetSpawnPositions playerSpawn;

	[Range(6, 16)]
	public int rows = 8;

	[Range(4, 10)]
	public int columns = 6;

	private void Awake()
	{
		float Hz = Screen.width;
		float Vt = Screen.height;

		float gridSizeHz = Hz / columns;
		float gridSizeVt = Vt / rows;

		// Get Row and Column length w.r.t to World Units
		mPlayerStats.distance.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0,0)),
									Camera.main.ScreenToWorldPoint(new Vector2(0, gridSizeHz)));
		mPlayerStats.distance.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0,0)),
									Camera.main.ScreenToWorldPoint(new Vector2(gridSizeVt,0)));

		// single row and column size stored in pixel
		mPlayerStats.grid.sizeInPixel = new Vector2(gridSizeHz, gridSizeVt);
		mPlayerStats.grid.rows = rows;
		mPlayerStats.grid.columns = columns;

		// storing screen space bounds in World units
		mPlayerStats.mbounds.lower = Camera.main.ScreenToWorldPoint (new Vector2(0,0));
		mPlayerStats.mbounds.upper = Camera.main.ScreenToWorldPoint (new Vector2(Screen.width, Screen.height));

		gridGrpBg.cellSize = mPlayerStats.grid.sizeInPixel;

		if (columns % 2 != 0) {
			columns += 1;
		}

		if (rows %2 != 0) {
			rows += 1;
		}

		playerSpawn.spawnPosInGrid = new Vector2 (mPlayerStats.grid.sizeInPixel.x * (columns * 0.5f),
										mPlayerStats.grid.sizeInPixel.y * (rows * 0.5f));
	}

	// Gives new bounds based on the grid Offset which can increment or decrement the current bounds
	public static PlayerStats.MyBounds GetScreenToWorld(float gridOffset, PlayerStats playerStats)
	{
		Vector2 incrementinScreenUnits = new Vector2(playerStats.grid.sizeInPixel.x * gridOffset,
											playerStats.grid.sizeInPixel.y * gridOffset);

		Vector2 uppperBoundsInScreenUnits = new Vector2(Screen.width, Screen.height);

		Vector2 newLowerBounds = Camera.main.ScreenToWorldPoint (new Vector2(- incrementinScreenUnits.x, -incrementinScreenUnits.y));
		Vector2 newUpperBounds = Camera.main.ScreenToWorldPoint (new Vector2(uppperBoundsInScreenUnits.x + incrementinScreenUnits.x,
									uppperBoundsInScreenUnits.y + incrementinScreenUnits.y));

		PlayerStats.MyBounds newBounds;
		newBounds.lower = newLowerBounds;
		newBounds.upper = newUpperBounds;

		return newBounds;
	}

	// Given a gridOffset in grid Units can check if given position lies inside the incremented or decremented new bounds
	public static bool CheckIfPlayerOutside (Vector2 currentPos, PlayerStats mPlayerStats, float gridOffset)
	{
		PlayerStats.MyBounds newBounds = GetScreenToWorld(gridOffset, mPlayerStats);

		if ((currentPos.x >= newBounds.lower.x
			&& currentPos.x <= newBounds.upper.x)
			&&
			(currentPos.y >= newBounds.lower.y
			&& currentPos.y <= newBounds.upper.y))
		{
			return false;
		}

		return true;
	}

	public static bool CheckIfPrevPosInGrid (Vector2 prevPostion, PlayerStats mPlayerStats)
	{
		if (Mathf.Approximately(prevPostion.x, mPlayerStats.mbounds.lower.x)||
			Mathf.Approximately(prevPostion.x, mPlayerStats.mbounds.upper.x)||
			Mathf.Approximately(prevPostion.y, mPlayerStats.mbounds.lower.y)||
			Mathf.Approximately(prevPostion.y, mPlayerStats.mbounds.upper.y))
		{
			return true;
		}

		return false;
	}

}
