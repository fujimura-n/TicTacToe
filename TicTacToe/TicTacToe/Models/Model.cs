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
		private readonly Status[,] boardStatuses = new Status[BoardSize, BoardSize] 
		{
			{ Status.None, Status.None, Status.None },
			{ Status.None, Status.None, Status.None },
			{ Status.None, Status.None, Status.None },
		};

		public event EventHandler BoardChanged;
		public event EventHandler<GameEndedEventArgs> GameEnded;

		public String  Label{get; set;}

		public bool IsGameEnded { get; private set; }

		public Status GameReault { get; set; }

		public Player CurrentPlayer { get; private set; }

		public Status[,] BoardStatuses
		{
			get { return this.boardStatuses; }
		}

		public void PutPiece(int row, int column, Status status)
		{
			if (IsGameEnded)
			{
				return;
			}

			if (boardStatuses[row, column] != Status.None)
			{
				return;
			}

			boardStatuses[row, column] = status;
			BoardChanged.Invoke(this, EventArgs.Empty);
			SwitchCurrentPleyer();
			if (CheckIfGameFinished())
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
					boardStatuses[i, j] = Status.None;
				}
			}
			CurrentPlayer = Player.Circle;
			BoardChanged.Invoke(this, EventArgs.Empty);
			IsGameEnded = false;
			GameReault = Status.None;
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

		private bool CheckIfGameFinished()
		{
			//行の判定
			for (int i = 0; i < BoardSize; i++)
			{
				var row = new List<Status>();

				for (int j = 0; j < BoardSize; j++)
				{
					row.Add(this.BoardStatuses[i, j]);
				}

				if(row[0] == row[1] && row[0] == row[2] && row[0] != Status.None)
				{
					GameReault = row[0];
					IsGameEnded = true;
					return true;
				}
			}

			//列の判定
			for (int j = 0; j < BoardSize; j++)
			{
				var column = new List<Status>();
				for (int i = 0; i < BoardSize; i++)
				{
					column.Add(this.BoardStatuses[i, j]);
				}
				if (column[0] == column[1] && column[0] == column[2] && column[0] != Status.None)
				{
					GameReault = column[0];
					IsGameEnded = true;
					return true;
				}
			}

			//斜めの判定
			var diagonally = new List<Status>();
			diagonally.Add(this.boardStatuses[0, 0]);
			diagonally.Add(this.boardStatuses[1, 1]);
			diagonally.Add(this.boardStatuses[2, 2]);
			if (diagonally[0] == diagonally[1] && diagonally[0] == diagonally[2] && diagonally[0] != Status.None)
			{
				GameReault = diagonally[0];
				IsGameEnded = true;
				return true;
			}

			diagonally.Clear();
			diagonally.Add(this.boardStatuses[0, 2]);
			diagonally.Add(this.boardStatuses[1, 1]);
			diagonally.Add(this.boardStatuses[2, 0]);
			if (diagonally[0] == diagonally[1] && diagonally[0] == diagonally[2] && diagonally[0] != Status.None)
			{
				GameReault = diagonally[0];
				IsGameEnded = true;
				return true;
			}

			//引き分け判定
			foreach(var a in this.boardStatuses)
			{
				if(a == Status.None)
				{
					IsGameEnded = false;
					return false;
				}				
			}

			GameReault = Status.None;
			IsGameEnded = true;
			return true;

		}
	}
}
