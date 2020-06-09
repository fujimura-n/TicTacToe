using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models
{
	public class GameEndedEventArgs : EventArgs
	{
		public GameEndedEventArgs(PlayerForm winner)
		{
			this.Winner = winner;
		}

		public PlayerForm Winner { get; set; }
	}
}
