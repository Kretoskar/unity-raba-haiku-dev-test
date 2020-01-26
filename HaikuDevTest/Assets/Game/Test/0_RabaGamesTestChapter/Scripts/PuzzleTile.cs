using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MudRoom.Puzzle
{
	public class PuzzleTile : MonoBehaviour
	{
		[SerializeField]
		private int _correctID;

		//Serialized for easy debuging
		[SerializeField]
		private int _id;

		public int ID { 
			get { return _id; }
			set { _id = value; }
		}
	}
}