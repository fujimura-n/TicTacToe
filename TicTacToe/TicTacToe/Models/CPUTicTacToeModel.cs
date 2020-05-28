using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace TicTacToe.Models
{
	public class CPUTicTacToeModel : NotificationObject, ITicTacToeModel
	{
		private Model model;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="boardSize">ボードの大きさ</param>
		/// <param name="alignNumber">いくつ揃ったら勝ちとするか</param>
		public CPUTicTacToeModel(int boardSize, int alignNumber)
		{
			this.BoardSize = boardSize;
			this.AlignNumber = alignNumber;
			this.model = new Model(boardSize, alignNumber);

			this.model.BoardChanged += new EventHandler((s, e) => this.BoardChanged.Invoke(this, EventArgs.Empty));
			this.model.CurrentPlayerChanged += new EventHandler(async (s, e) => await this.CurrentPlayerChangedActionAsync());
			this.model.GameEnded += new EventHandler<GameEndedEventArgs>((s, e) => this.GameEnded.Invoke(this, e));
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
		public bool IsGameEnded
		{
			get { return model.IsGameEnded; }
			private set { }
		}

		/// <summary>
		/// ゲームの勝者を取得します。
		/// </summary>
		public Player Winner { get; set; }

		/// <summary>
		/// 現在駒を配置できるプレーヤーを取得します。
		/// </summary>
		public Player CurrentPlayer
		{
			get { return model.CurrentPlayer; }
			private set { model.CurrentPlayer = value; }
		}

		/// <summary>
		/// 盤上の駒の配置状態を取得します。
		/// </summary>
		public Player[,] BoardStatuses
		{
			get { return this.model.BoardStatuses; }
		}

		public event EventHandler BoardChanged;
		public event EventHandler CurrentPlayerChanged;
		public event EventHandler<GameEndedEventArgs> GameEnded;

		public void PutPiece(int row, int column, Player player)
		{
			if (player.Equals(Player.Cross))
			{
				///CPUプレーヤーの場合の番にユーザーが駒を配置しようとした場合は何もしない
				return;
			}
			model.PutPiece(row, column, player);
		}

		/// <summary>
		/// ランダムに✕の駒を配置します。
		/// </summary>
		private void PutPieceRandom()
		{
			int row;
			int column;
			(row, column) = VacantPosition();
			model.PutPiece(row, column, Player.Cross);
		}

		/// <summary>
		/// 駒が置かれていない座標をランダムに取得します。
		/// </summary>
		/// <returns></returns>
		private (int row, int column) VacantPosition()
		{
			int row;
			int column;
			Random random = new Random();
			row = random.Next(0, BoardSize);
			column = random.Next(0, BoardSize);

			while (!this.BoardStatuses[row, column].Equals(Player.None))
			{
				row = random.Next(0, BoardSize);
				column = random.Next(0, BoardSize);
			}

			return (row, column);
		}

		/// <summary>
		/// CurrentPlayer変更時の処理をします。
		/// </summary>
		private async Task CurrentPlayerChangedActionAsync()
		{
			this.CurrentPlayerChanged.Invoke(this, EventArgs.Empty);
			if (model.CurrentPlayer.Equals(Player.Cross))
			{
				await Task.Delay(1000);
				PutPieceRandom();
			}
		}

		public void ResetGame()
		{
			model.ResetGame();
		}

	}
}
