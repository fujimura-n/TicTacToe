using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace TicTacToe.Models
{
	public class Model : NotificationObject
	{
		private const int BoardSize = 3;
		private readonly Player[,] boardStatuses = new Player[BoardSize, BoardSize]
		{
			{ Player.None, Player.None, Player.None },
			{ Player.None, Player.None, Player.None },
			{ Player.None, Player.None, Player.None },
		};

		public event EventHandler BoardChanged;
		public event EventHandler<GameEndedEventArgs> GameEnded;

		public String  Label{get; set;}

		public bool IsGameEnded { get; private set; }

		public Player GameReault { get; set; }

		public Player CurrentPlayer { get; private set; }

		public Player[,] BoardStatuses
		{
			get { return this.boardStatuses; }
		}

		public void PutPiece(int row, int column, Player status)
		{
			if (IsGameEnded)
			{
				return;
			}

			if (boardStatuses[row, column] != Player.None)
			{
				return;
			}

			boardStatuses[row, column] = status;
			BoardChanged.Invoke(this, EventArgs.Empty);
			SwitchCurrentPleyer();
			(bool isGameEnded, Player winner) = CheckIfGameEnded();
			IsGameEnded = isGameEnded;
			GameReault = winner;
			if (isGameEnded)
			{
				GameEnded.Invoke(this, new GameEndedEventArgs(GameReault));
			}

		}

		public void ResetBoard()
		{
			for (int i = 0; i < BoardSize; i++)
			{
				for (int j = 0; j < BoardSize; j++)
				{
					boardStatuses[i, j] = Player.None;
				}
			}
			CurrentPlayer = Player.Circle;
			BoardChanged.Invoke(this, EventArgs.Empty);
			IsGameEnded = false;
			GameReault = Player.None;
		}
		private void SwitchCurrentPleyer()
		{
			if (CurrentPlayer == Player.Circle)
			{
				CurrentPlayer = Player.Cross;
			}
			else if (CurrentPlayer == Player.Cross)
			{
				CurrentPlayer = Player.Circle;
			}
		}

		private (bool isGameEnded, Player winner) CheckIfGameEnded()
		{
			//行の判定
			for (int i = 0; i < BoardSize; i++)
			{
				var row = new List<Player>();

				for (int j = 0; j < BoardSize; j++)
				{
					row.Add(this.BoardStatuses[i, j]);
				}

				if(row[0] == row[1] && row[0] == row[2] && row[0] != Player.None)
				{
					return (true, row[0]);
				}
			}

			//列の判定
			for (int j = 0; j < BoardSize; j++)
			{
				var column = new List<Player>();
				for (int i = 0; i < BoardSize; i++)
				{
					column.Add(this.BoardStatuses[i, j]);
				}
				if (column[0] == column[1] && column[0] == column[2] && column[0] != Player.None)
				{
					return (true, column[0]);
				}
			}

			//斜めの判定
			var diagonally = new List<Player>();
			diagonally.Add(this.boardStatuses[0, 0]);
			diagonally.Add(this.boardStatuses[1, 1]);
			diagonally.Add(this.boardStatuses[2, 2]);
			if (diagonally[0] == diagonally[1] && diagonally[0] == diagonally[2] && diagonally[0] != Player.None)
			{
				return (true, diagonally[0]);
			}

			diagonally.Clear();
			diagonally.Add(this.boardStatuses[0, 2]);
			diagonally.Add(this.boardStatuses[1, 1]);
			diagonally.Add(this.boardStatuses[2, 0]);
			if (diagonally[0] == diagonally[1] && diagonally[0] == diagonally[2] && diagonally[0] != Player.None)
			{
				return (true, diagonally[0]);
			}

			//引き分け判定
			foreach(var a in this.boardStatuses)
			{
				if(a == Player.None)
				{
					return (false, Player.None);
				}				
			}

			return (true, Player.None);

		}
	}
}
