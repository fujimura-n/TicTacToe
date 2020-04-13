using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models
{
	public class GameEndedEventArgs : EventArgs
	{
		public GameEndedEventArgs(Status winner)
		{
			this.Winner = winner;
		}

		public Status Winner { get; set; }
	}
}
