using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mDirection
{
	UP, DOWN, LEFT, RIGHT
}

public class TouchManager : MonoBehaviour
{
	public  PlayerStats mPlayerStats;
	public mDirection currentDirection;

	private enum mQuadrant
	{
		TOPRIGHT,
		TOPLEFT,
		BOTTOMLEFT,
		BOTTOMRIGHT
	}
	private mQuadrant  currentQuadrant;
	private Vector2 deltaPos;
	private Vector2 currentDirectionVector;
	private readonly float dotProductConst = 0.5f;

	private void Start()
	{
		currentDirection = mDirection.UP;
		currentDirectionVector = Vector2.up;
	}

	// Update is called once per frame
	private void Update ()
	{
		int count = Input.touchCount;

		if (count > 0) {
			if (count > 1) {
				PauseOnMutiTouch();
				return;
			}

			Touch touch = Input.GetTouch(0);
			HandleTouchPhase (touch);
		}

		mPlayerStats.dir = currentDirection;
		mPlayerStats.dirVector = currentDirectionVector;
	}

	private void HandleTouchPhase(Touch touch)
	{
		deltaPos = touch.deltaPosition;

		if ((deltaPos * touch.deltaTime).magnitude < 0.25f) {
			return;
		}

		deltaPos = deltaPos.normalized;

		switch (touch.phase)
		{
			case TouchPhase.Moved:
				SetQuadrant();
				break;
		}
	}

	private void SetQuadrant()
	{
		if (deltaPos.x > 0 && deltaPos.y > 0)
			currentQuadrant = mQuadrant.TOPRIGHT;

		if (deltaPos.x < 0 && deltaPos.y > 0)
			currentQuadrant = mQuadrant.TOPLEFT;

		if (deltaPos.x < 0 && deltaPos.y < 0)
			currentQuadrant = mQuadrant.BOTTOMLEFT;

		if (deltaPos.x > 0 && deltaPos.y < 0)
			currentQuadrant = mQuadrant.BOTTOMRIGHT;


		switch (currentQuadrant)
		{
			case mQuadrant.TOPRIGHT:
				if (Vector2.Dot(Vector2.up, deltaPos) > dotProductConst)
				{
					currentDirection = mDirection.UP;
					currentDirectionVector = Vector2.up;
				} else {
					currentDirection = mDirection.RIGHT;
					currentDirectionVector = Vector2.right;
				}
				break;

			case mQuadrant.TOPLEFT:
				if (Vector2.Dot(Vector2.up, deltaPos) > dotProductConst)
				{
					currentDirection = mDirection.UP;
					currentDirectionVector = Vector2.up;
				} else {
					currentDirection = mDirection.LEFT;
					currentDirectionVector = Vector2.left;
				}
				break;

			case mQuadrant.BOTTOMLEFT:
				if (Vector2.Dot(Vector2.down, deltaPos) > dotProductConst)
				{
					currentDirection = mDirection.DOWN;
					currentDirectionVector = Vector2.down;
				} else {
					currentDirection = mDirection.LEFT;
					currentDirectionVector = Vector2.left;
				}
				break;

			case mQuadrant.BOTTOMRIGHT:
				if (Vector2.Dot(Vector2.down, deltaPos) > dotProductConst)
				{
					currentDirection = mDirection.DOWN;
					currentDirectionVector = Vector2.down;
				} else {
					currentDirection = mDirection.RIGHT;
					currentDirectionVector = Vector2.right;
				}
				break;
		}
	}

	private void PauseOnMutiTouch() {
		// show pause screen
	}
}
