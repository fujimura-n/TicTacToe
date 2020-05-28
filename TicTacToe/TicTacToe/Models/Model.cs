using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace TicTacToe.Models
{
	//TicTacToeModel
	public class Model : NotificationObject, ITicTacToeModel
	{
		private readonly Board<Player> board;

		public event EventHandler BoardChanged;
		public event EventHandler CurrentPlayerChanged;
		public event EventHandler<GameEndedEventArgs> GameEnded;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="boardSize">ボードの大きさ</param>
		/// <param name="alignNumber">いくつ揃ったら勝ちとするか</param>
		public Model(int boardSize, int alignNumber)
		{
			this.BoardSize = boardSize;
			this.AlignNumber = alignNumber;
			this.board = new Board<Player>(Player.None, boardSize);
		}

		/// <summary>
		/// ボードの大きさ（マスの数）
		/// </summary>
		public int BoardSize { get; }

		/// <summary>
		/// いくつ揃ったら勝ちとするか
		/// </summary>
		public int AlignNumber { get; }

		/// <summary>
		/// ゲームが終了しているかどうかを取得します。
		/// </summary>
		public bool IsGameEnded { get; private set; }

		/// <summary>
		/// ゲームの勝者を取得します。
		/// </summary>
		public Player Winner { get; set; }

		/// <summary>
		/// 現在駒を配置できるプレーヤーを取得します。
		/// </summary>
		public Player CurrentPlayer { get; set; }

		/// <summary>
		/// 盤上の駒の配置状態を取得します。
		/// </summary>
		public Player[,] BoardStatuses
		{
			get { return this.board.BoardStatuses; }
		}

		/// <summary>
		/// 盤上に駒を配置します。
		/// ゲームがすでに終了している場合、指定した位置にすでに駒が置かれている場合は何もしません。
		/// </summary>
		/// <param name="row">配置する行</param>
		/// <param name="column">配置する列</param>
		/// <param name="player">配置する駒</param>
		public void PutPiece(int row, int column, Player player)
		{
			if (IsGameEnded)
			{
				return;
			}

			if (this.board.BoardStatuses[row, column] != Player.None)
			{
				return;
			}

			this.board.PutPiece(row, column, player);
			BoardChanged.Invoke(this, EventArgs.Empty);
			SwitchCurrentPleyer();
			(bool isGameEnded, Player winner) = CheckIfGameEnded(BoardSize, AlignNumber);
			this.IsGameEnded = isGameEnded;
			this.Winner = winner;
			if (isGameEnded)
			{
				GameEnded.Invoke(this, new GameEndedEventArgs(this.Winner));
			}

		}

		/// <summary>
		/// ゲームをリセットします。
		/// </summary>
		public void ResetGame()
		{
			this.board.ResetBoard(Player.None);
			CurrentPlayer = Player.Circle;
			CurrentPlayerChanged.Invoke(this, EventArgs.Empty);
			BoardChanged.Invoke(this, EventArgs.Empty);
			IsGameEnded = false;
			Winner = Player.None;
		}

		/// <summary>
		/// <see cref="CurrentPlayer"/>を切り替えます。
		/// </summary>
		private void SwitchCurrentPleyer()
		{
			if (CurrentPlayer == Player.Circle)
			{
				CurrentPlayer = Player.Cross;
				CurrentPlayerChanged.Invoke(this, EventArgs.Empty);
			}
			else if (CurrentPlayer == Player.Cross)
			{
				CurrentPlayer = Player.Circle;
				CurrentPlayerChanged.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// すでにゲームが終了しているか判定します。
		/// </summary>
		/// <returns name="IsGameEnded">ゲームが終了しているかどうか</returns>
		/// <returns name="winner">ゲームの勝者</returns>
		private (bool isGameEnded, Player winner) CheckIfGameEnded(int boardSize, int alignNumber)
		{
			//行の判定
			for (int i = 0; i < boardSize; i++)
			{
				var row = new List<Player>();

				for (int j = 0; j < boardSize; j++)
				{
					row.Add(this.BoardStatuses[i, j]);
				}
				
				(bool isGameEnded, Player winner) = JudgeAlign(row, alignNumber);
				if (isGameEnded)
				{
					return (isGameEnded, winner);
				}
				row.Clear();
			}

			//列の判定
			for (int j = 0; j < boardSize; j++)
			{
				var column = new List<Player>();
				for (int i = 0; i < boardSize; i++)
				{
					column.Add(this.BoardStatuses[i, j]);
				}

				(bool isGameEnded, Player winner) = JudgeAlign(column, alignNumber);
				if (isGameEnded)
				{
					return (isGameEnded, winner);
				}
				column.Clear();
			}

			//斜めの判定
			var diagonally_right = new List<Player>();
			var diagonally_left = new List<Player>();
			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					//右斜めの要素をListに格納
					var x = i;
					var y = j;
					while (0 <= x && y < boardSize)
					{
						diagonally_right.Add(this.BoardStatuses[x, y]);
						x-- ;
						y++;
					}

					//左斜めの要素をListに格納
					x = i;
					y = j;
					while (x < boardSize && y < boardSize)
					{
						diagonally_left.Add(this.BoardStatuses[x, y]);
						x++;
						y++;
					}

					//判定
					bool isGameEnded;
					Player winner;
					(isGameEnded, winner) = JudgeAlign(diagonally_right, alignNumber);
					if (isGameEnded)
					{
						return (isGameEnded, winner);
					}
					diagonally_right.Clear();

					(isGameEnded, winner) = JudgeAlign(diagonally_left, alignNumber);
					if (isGameEnded)
					{
						return (isGameEnded, winner);
					}
					diagonally_left.Clear();
				}				
			}

			//引き分け判定
			foreach (var a in this.BoardStatuses)
			{
				if(a == Player.None)
				{
					return (false, Player.None);
				}				
			}
			return (true, Player.None);

		}

		/// <summary>
		/// <see cref="list"/>の中で同じ駒が<see cref="alignNumber"/>個連続して並んでいるか判定します。
		/// </summary>
		/// <param name="list">配置された駒の列</param>
		/// <param name="alignNumber">列の中でいくつ揃ったら勝ちとするか</param>
		/// <returns name="IsGameEnded">ゲームが終了しているかどうか</returns>
		/// <returns name="winner">ゲームの勝者</returns>
		private (bool isGameEnded, Player winner) JudgeAlign(List<Player> list, int alignNumber)
		{
			if (list.Count < alignNumber)
			{
				return (false, Player.None);
			}
			var count = 0;
			for (int index = 0; index < list.Count - 1; index++)
			{
				if (!list[index].Equals(Player.None) && list[index].Equals(list[index + 1]))
				{
					count++;
				}
				else
				{
					count = 0;
				}

				if (count >= alignNumber - 1)
				{
					return (true, list[index]);
				}
			}
			return (false, Player.None);
		}
	}
}
