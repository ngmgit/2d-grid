﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
	public Vector2 deltaPos;
	public int count;


	public enum mDirection
	{
		UP, DOWN, LEFT, RIGHT
	}
	public mDirection currentDirection;

	private enum mQuadrant
	{
		TOPRIGHT,
		TOPLEFT,
		BOTTOMLEFT,
		BOTTOMRIGHT
	}
	private mQuadrant  currentQuadrant;

	private void Start()
	{
		currentDirection = mDirection.UP;
	}

	// Update is called once per frame
	void Update ()
	{
		count = Input.touchCount;
		if (Input.touchCount > 0) {
			if (count > 1) {
				PauseOnMutiTouch();
				return;
			}

			Touch touch = Input.GetTouch(0);
			HandleTouchPhase (touch);
		}
	}

	private void HandleTouchPhase(Touch touch)
	{
		if (touch.deltaPosition.magnitude < 1) {
			return;
		}
		deltaPos = touch.deltaPosition.normalized;

		switch (touch.phase)
		{
			case TouchPhase.Moved:
				SetQuadrant();
				break;
		}
	}

	private void PauseOnMutiTouch() {

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
				if (Vector2.Dot(Vector2.up, deltaPos) > 0.5f)
					currentDirection = mDirection.UP;
				else
					currentDirection = mDirection.RIGHT;
				break;

			case mQuadrant.TOPLEFT:
				if (Vector2.Dot(Vector2.up, deltaPos) > 0.5f)
					currentDirection = mDirection.UP;
				else
					currentDirection = mDirection.LEFT;
				break;

			case mQuadrant.BOTTOMLEFT:
				if (Vector2.Dot(Vector2.down, deltaPos) > 0.5f)
					currentDirection = mDirection.DOWN;
				else
					currentDirection = mDirection.LEFT;
				break;

			case mQuadrant.BOTTOMRIGHT:
				if (Vector2.Dot(Vector2.down, deltaPos) > 0.5f)
					currentDirection = mDirection.DOWN;
				else
					currentDirection = mDirection.RIGHT;
				break;
		}
	}
}