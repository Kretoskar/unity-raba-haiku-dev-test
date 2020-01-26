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

        private void Start()
        {
            HighlightedTile = null;
            Activate();
        }

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

        public override void Activate()
        {
            base.Activate();
            SpawnPuzzleTiles();
        }

        private void SpawnPuzzleTiles()
        {
            for (int i = 0; i < 24; i++)
            {
                SpawnSingleTile(i);
            }
        }

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

            _allPuzzleTiles.Add(puzzle);
            _availablePuzzleTiles.RemoveAt(tileIndex);
        }
    }
}
