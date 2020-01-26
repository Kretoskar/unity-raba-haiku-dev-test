using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MudRoom.Puzzle
{
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(BoxCollider2D))]
	public class PuzzleTile : MonoBehaviour
	{
		[SerializeField]
		private int _correctID;
		[SerializeField]
		private Sprite _highlightSprite = null;
		//Serialized for easy debuging
		[SerializeField]
		private int _id;

		private bool _isHighlighted;
		private GameObject _highliteObject;

		/// <summary>
		/// The controller for all the tiles.
		/// Injected when tiles are instantiated by the controller itself.
		/// </summary>
		public CamoTilePuzzleController Controller { private get; set; }

		/// <summary>
		/// Is true when the tile is at the position
		/// corresponding to winning puzzle combination
		/// </summary>
		public bool IsTileInCorrectPosition
		{
			get { return ID == _correctID; }
		}

		/// <summary>
		/// Current position at puzzle table
		/// </summary>
		public int ID 
		{ 
			get { return _id; }
			set { _id = value; }
		}

		/// <summary>
		/// Correct position of the puzzle at puzzle table. 
		/// Setting all the tiles at their correct position 
		/// is the condition to winning the puzzle.
		/// </summary>
		public int CorrectID
		{
			get { return _correctID; }
			set { _correctID = value; }
		}

		private void Start()
		{
			_isHighlighted = false;
			SetupHighlite();
		}

		private void SetupHighlite()
		{
			_highliteObject = new GameObject("Highlight");
			_highliteObject.transform.parent = transform;
			_highliteObject.AddComponent<SpriteRenderer>().sprite = _highlightSprite;
			//z is set to -1 to make sure the highlight is visible
			_highliteObject.transform.localPosition = new Vector3(0, 0, -1);
			_highliteObject.SetActive(false);
		}

		private void OnMouseDown()
		{
			if (!_isHighlighted)
			{
				HighlightOrSwapTile();
			}
			else
			{
				UnHighlightTile();
			}
		}

		private void HighlightOrSwapTile()
		{
			if(Controller.ShouldSwap)
			{
				Swap();
			}
			else
			{
				HighlightTile();
			}
		}

		private void Swap()
		{
			PuzzleTile tileToSwap = Controller.HighlightedTile;
			Vector3 tileToSwapPosition = tileToSwap.transform.position;
			int tileToSwapID = tileToSwap.ID;

			tileToSwap.UnHighlightTile();
			tileToSwap.ID = ID;
			tileToSwap.transform.position = transform.position;

			transform.position = tileToSwapPosition;
			ID = tileToSwapID;

			Controller.CheckForWin();
		}

		private void HighlightTile()
		{
			_highliteObject.SetActive(true);
			Controller.HighlightedTile = this;
			_isHighlighted = true;
		}

		private void UnHighlightTile()
		{
			_highliteObject.SetActive(false);
			_isHighlighted = false;
			Controller.HighlightedTile = null;
		}
	}
}