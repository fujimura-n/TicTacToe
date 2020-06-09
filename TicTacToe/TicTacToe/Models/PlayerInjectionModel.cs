using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace TicTacToe.Models
{
	public class PlayerInjectionModel : NotificationObject, ITicTacToeModel
	{
		private readonly IPlayer circlePyaler;
		private readonly IPlayer crossPlayer;
		private Model model;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="boardSize"></param>
		/// <param name="alignNumber"></param>
		/// <param name="circlePyaler"></param>
		/// <param name="crossPlayer"></param>
		public PlayerInjectionModel(int boardSize, int alignNumber, IPlayer circlePyaler, IPlayer crossPlayer)
		{
			this.BoardSize = boardSize;
			this.AlignNumber = alignNumber;
			this.model = new Model(boardSize, alignNumber);

			this.model.BoardChanged += new EventHandler((s, e) => this.BoardChanged.Invoke(this, EventArgs.Empty));
			this.model.CurrentPlayerChanged += new EventHandler((s, e) => this.CurrentPlayerChangedActionAsync());
			this.model.GameEnded += new EventHandler<GameEndedEventArgs>((s, e) => this.GameEnded.Invoke(this, e));
			this.circlePyaler = circlePyaler;
			this.crossPlayer = crossPlayer;
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
		public PlayerForm Winner { get; set; }

		/// <summary>
		/// 現在駒を配置できるプレーヤーを取得します。
		/// </summary>
		public PlayerForm CurrentPlayer
		{
			get { return model.CurrentPlayer; }
			private set { model.CurrentPlayer = value; }
		}

		/// <summary>
		/// 盤上の駒の配置状態を取得します。
		/// </summary>
		public PlayerForm[,] BoardStatuses
		{
			get { return this.model.BoardStatuses; }
		}

		public event EventHandler BoardChanged;
		public event EventHandler CurrentPlayerChanged;
		public event EventHandler<GameEndedEventArgs> GameEnded;

		public void StartGame(PlayerForm firstMove = PlayerForm.Circle)
		{
			model.StartGame(firstMove);
		}

		public void PutPiece(int row, int column, PlayerForm player)
		{
			switch (player)
			{
				case PlayerForm.Circle:
					this.circlePyaler.PutPiece(row, column, player, this.model);
					break;
				case PlayerForm.Cross:
					this.crossPlayer.PutPiece(row, column, player, this.model);
					break;
				default:
					return;
			}
		}

		public void ResetGame()
		{
			model.ResetGame();
		}

		/// <summary>
		/// CurrentPlayer変更時の処理をします。
		/// </summary>
		private void CurrentPlayerChangedActionAsync()
		{
			this.CurrentPlayerChanged.Invoke(this, EventArgs.Empty);

			switch (model.CurrentPlayer)
			{
				case PlayerForm.Circle:
					this.circlePyaler.ChangedToMyTurn(model.CurrentPlayer, this.model);
					break;
				case PlayerForm.Cross:
					this.crossPlayer.ChangedToMyTurn(model.CurrentPlayer, this.model);
					break;
				default:
					return;
			}

		}

	}
}
