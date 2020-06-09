using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models
{
	public class PersonPlayer : IPlayer
	{
		public void ChangedToMyTurn(PlayerForm player, ITicTacToeModel model)
		{
			return;
		}

		public void PutPiece(int row, int column, PlayerForm player, ITicTacToeModel model)
		{
			model.PutPiece(row, column, player);
		}
	}
}
