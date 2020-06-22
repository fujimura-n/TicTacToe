using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models
{
	public interface ITicTacToeModel
	{
		event EventHandler BoardChanged;
		event EventHandler CurrentPlayerChanged;
		event EventHandler<GameEndedEventArgs> GameEnded;

		/// <summary>
		/// ボードの大きさ（マスの数）
		/// </summary>
		int BoardSize { get; }

		/// <summary>
		/// いくつ揃ったら勝ちとするか
		/// </summary>
		int AlignNumber { get; }


		/// <summary>
		/// ゲームの状態を取得します。
		/// </summary>
		GameStatus GameStatus { get; }

		/// <summary>
		/// ゲームの勝者を取得します。
		/// </summary>
		PlayerForm Winner { get; set; }

		/// <summary>
		/// 現在駒を配置できるプレーヤーを取得します。
		/// </summary>
		PlayerForm CurrentPlayer { get; }

		/// <summary>
		/// 盤上の駒の配置状態を取得します。
		/// </summary>
		PlayerForm[,] BoardStatuses{ get; }

		/// <summary>
		/// ゲームを開始します。
		/// </summary>
		void StartGame();

		/// <summary>
		/// 盤上に駒を配置します。
		/// ゲームがすでに終了している場合、指定した位置にすでに駒が置かれている場合は何もしません。
		/// </summary>
		/// <param name="row">配置する行</param>
		/// <param name="column">配置する列</param>
		/// <param name="player">配置する駒</param>
		void PutPiece(int row, int column, PlayerForm player);

		/// <summary>
		/// ゲームをリセットします。
		/// </summary>
		public void ResetGame();
	}
}
