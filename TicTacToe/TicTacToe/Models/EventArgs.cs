using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models
{
	public class GameEndedEventArgs : EventArgs
	{
		public GameEndedEventArgs(Player winner)
		{
			this.Winner = winner;
		}

		public Player Winner { get; set; }
	}
}
