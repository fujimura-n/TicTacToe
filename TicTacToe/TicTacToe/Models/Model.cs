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
		private readonly Board<PlayerForm> board;

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
			this.board = new Board<PlayerForm>(PlayerForm.None, boardSize);
			this.CurrentPlayer = PlayerForm.Circle;
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
		public PlayerForm Winner { get; set; }

		/// <summary>
		/// 現在駒を配置できるプレーヤーを取得します。
		/// </summary>
		public PlayerForm CurrentPlayer { get; set; }

		/// <summary>
		/// 盤上の駒の配置状態を取得します。
		/// </summary>
		public PlayerForm[,] BoardStatuses
		{
			get { return this.board.BoardStatuses; }
		}

		public void StartGame(PlayerForm firstMove = PlayerForm.Circle)
		{
			CurrentPlayer = firstMove;
			CurrentPlayerChanged.Invoke(this, EventArgs.Empty);
			var str = CurrentPlayer.ToString();
		}

		/// <summary>
		/// 盤上に駒を配置します。
		/// ゲームがすでに終了している場合、指定した位置にすでに駒が置かれている場合は何もしません。
		/// </summary>
		/// <param name="row">配置する行</param>
		/// <param name="column">配置する列</param>
		/// <param name="player">配置する駒</param>
		public void PutPiece(int row, int column, PlayerForm player)
		{
			if (IsGameEnded)
			{
				return;
			}

			if (this.board.BoardStatuses[row, column] != PlayerForm.None)
			{
				return;
			}

			this.board.PutPiece(row, column, player);
			BoardChanged.Invoke(this, EventArgs.Empty);
			SwitchCurrentPleyer();
			(bool isGameEnded, PlayerForm winner) = CheckIfGameEnded(BoardSize, AlignNumber);
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
			this.board.ResetBoard(PlayerForm.None);
			CurrentPlayer = PlayerForm.Circle;
			CurrentPlayerChanged.Invoke(this, EventArgs.Empty);
			BoardChanged.Invoke(this, EventArgs.Empty);
			IsGameEnded = false;
			Winner = PlayerForm.None;
		}

		/// <summary>
		/// <see cref="CurrentPlayer"/>を切り替えます。
		/// </summary>
		private void SwitchCurrentPleyer()
		{
			if (CurrentPlayer == PlayerForm.Circle)
			{
				CurrentPlayer = PlayerForm.Cross;
				CurrentPlayerChanged.Invoke(this, EventArgs.Empty);
			}
			else if (CurrentPlayer == PlayerForm.Cross)
			{
				CurrentPlayer = PlayerForm.Circle;
				CurrentPlayerChanged.Invoke(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// すでにゲームが終了しているか判定します。
		/// </summary>
		/// <returns name="IsGameEnded">ゲームが終了しているかどうか</returns>
		/// <returns name="winner">ゲームの勝者</returns>
		private (bool isGameEnded, PlayerForm winner) CheckIfGameEnded(int boardSize, int alignNumber)
		{
			//行の判定
			for (int i = 0; i < boardSize; i++)
			{
				var row = new List<PlayerForm>();

				for (int j = 0; j < boardSize; j++)
				{
					row.Add(this.BoardStatuses[i, j]);
				}
				
				(bool isGameEnded, PlayerForm winner) = JudgeAlign(row, alignNumber);
				if (isGameEnded)
				{
					return (isGameEnded, winner);
				}
				row.Clear();
			}

			//列の判定
			for (int j = 0; j < boardSize; j++)
			{
				var column = new List<PlayerForm>();
				for (int i = 0; i < boardSize; i++)
				{
					column.Add(this.BoardStatuses[i, j]);
				}

				(bool isGameEnded, PlayerForm winner) = JudgeAlign(column, alignNumber);
				if (isGameEnded)
				{
					return (isGameEnded, winner);
				}
				column.Clear();
			}

			//斜めの判定
			var diagonally_right = new List<PlayerForm>();
			var diagonally_left = new List<PlayerForm>();
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
					PlayerForm winner;
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
				if(a == PlayerForm.None)
				{
					return (false, PlayerForm.None);
				}				
			}
			return (true, PlayerForm.None);

		}

		/// <summary>
		/// <see cref="list"/>の中で同じ駒が<see cref="alignNumber"/>個連続して並んでいるか判定します。
		/// </summary>
		/// <param name="list">配置された駒の列</param>
		/// <param name="alignNumber">列の中でいくつ揃ったら勝ちとするか</param>
		/// <returns name="IsGameEnded">ゲームが終了しているかどうか</returns>
		/// <returns name="winner">ゲームの勝者</returns>
		private (bool isGameEnded, PlayerForm winner) JudgeAlign(List<PlayerForm> list, int alignNumber)
		{
			if (list.Count < alignNumber)
			{
				return (false, PlayerForm.None);
			}
			var count = 0;
			for (int index = 0; index < list.Count - 1; index++)
			{
				if (!list[index].Equals(PlayerForm.None) && list[index].Equals(list[index + 1]))
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
			return (false, PlayerForm.None);
		}
	}
}
