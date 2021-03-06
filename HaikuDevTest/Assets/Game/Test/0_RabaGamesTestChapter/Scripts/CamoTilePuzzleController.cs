﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MudRoom.Puzzle
{
    public class CamoTilePuzzleController : PuzzleController
    {
        //Changing sprite's 'pixels per unit' would make this variable an int,
        //but as a programmer I didn't want to interrupt designer's work.
        //In a real world example I would contact the designer, but I wanted
        //to show that I can handle poor designer work. ;)
        private const float _tileEdgeLength = 2.75f;
        //How many tiles can be located in puzzle width
        private const int _widthInTiles = 6;

        [SerializeField]
        private List<PuzzleTile> _availablePuzzleTiles = new List<PuzzleTile>();
        [SerializeField]
        private Transform _tilesParent = null;

        private int _puzzleCount = 24;
        //Again, x an y would be ints if it wasn't for incorrect 'pixels per unit'
        private Vector2 _upperLeftCornerPuzzlePosition = new Vector3(-6.375f, 4.2f);
        private List<PuzzleTile> _allPuzzleTiles = new List<PuzzleTile>();

        /// <summary>
        /// Tile highlighted by the user
        /// </summary>
        public PuzzleTile HighlightedTile { get; set; }

        /// <summary>
        /// Returns true if there is a highlihted tile already
        /// </summary>
        public bool ShouldSwap 
        {
            get
            {
                return HighlightedTile != null;
            }
        }

        /// <summary>
        /// Activate the puzzle UI
        /// </summary>
        private void Start()
        {
            HighlightedTile = null;
            Activate();
        }

        /// <summary>
        /// Call base class's win method if all the 
        /// puzzle tiles are in correct position.
        /// </summary>
        public void CheckForWin()
        {
            bool win = true;
            foreach(var tile in _allPuzzleTiles)
            {
                if (!tile.IsTileInCorrectPosition)
                    win = false;
            }
            if(win)
            {
                Win();
            }
        }

        /// <summary>
        /// Activate the UI and set up puzzle tiles
        /// </summary>
        public override void Activate()
        {
            base.Activate();
            SpawnPuzzleTiles();
        }

        /// <summary>
        /// Randomize puzzle positions
        /// </summary>
        public override void ResetPuzzle()
        {
            base.ResetPuzzle();
            MixPuzzleTiles();
        }

        /// <summary>
        /// Set puzzles to correct positions 
        /// and send won event to won sfm.
        /// </summary>
        public override void Skip()
        {
            SolveThePuzzle();
            base.Skip();
        }

        /// <summary>
        /// Set all tiles to correct position
        /// </summary>
        private void SolveThePuzzle()
        {
            for(int i = 0; i < _puzzleCount; i++)
            {
                foreach(var tile in _allPuzzleTiles)
                {
                    if(tile.CorrectID == i)
                    {
                        tile.transform.localPosition = new Vector2(_upperLeftCornerPuzzlePosition.x + (i % _widthInTiles) * _tileEdgeLength,
                            _upperLeftCornerPuzzlePosition.y - (Mathf.Floor(i / _widthInTiles) * _tileEdgeLength));
                        tile.ID = i;
                    }
                }
            }
        }

        /// <summary>
        /// Randomize puzzle tiles positions
        /// </summary>
        private void MixPuzzleTiles() //Note: MixTiles() and SpawnPuzzleTiles() look similar, but I made it two seperate methods for readability
        {
            _availablePuzzleTiles = new List<PuzzleTile>(_allPuzzleTiles);
            for(int i = 0; i < _puzzleCount; i++)
            {
                //Select a random tile
                int tileIndex = UnityEngine.Random.Range(0, _availablePuzzleTiles.Count);
                PuzzleTile puzzle = _availablePuzzleTiles[tileIndex];

                //Move it to position corresponding to 'i'
                puzzle.transform.localPosition = new Vector2(_upperLeftCornerPuzzlePosition.x + (i % _widthInTiles) * _tileEdgeLength,
                    _upperLeftCornerPuzzlePosition.y - (Mathf.Floor(i / _widthInTiles) * _tileEdgeLength));
                puzzle.ID = i;

                //Remove this tile from available tiles
                _availablePuzzleTiles.RemoveAt(tileIndex);
            }
        }

        /// <summary>
        /// Spawn puzzle tiles in random order
        /// </summary>
        private void SpawnPuzzleTiles()
        {
            for (int i = 0; i < _puzzleCount; i++)
            {
                SpawnSingleTile(i);
            }
        }

        /// <summary>
        /// Instantiate a puzzle tile and set it up.
        /// Position is set depending on the index.
        /// </summary>
        /// <param name="index">current tile index</param>
        private void SpawnSingleTile(int index)
        {
            //Select a random tile so that the puzzle is never the same each game
            int tileIndex = UnityEngine.Random.Range(0, _availablePuzzleTiles.Count);
            PuzzleTile puzzle = Instantiate(_availablePuzzleTiles[tileIndex], _tilesParent);

            //ID corresponds to the puzzle position
            puzzle.ID = index;
            puzzle.Controller = this;
            puzzle.transform.localPosition = new Vector2(_upperLeftCornerPuzzlePosition.x + (index % _widthInTiles) * _tileEdgeLength,
                _upperLeftCornerPuzzlePosition.y - (Mathf.Floor(index / _widthInTiles) * _tileEdgeLength));

            //Add this tile to all tiles list
            _allPuzzleTiles.Add(puzzle);
            //Remove this tile from available tiles
            _availablePuzzleTiles.RemoveAt(tileIndex);
        }
    }
}
