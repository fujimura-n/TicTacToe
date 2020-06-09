using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
	public class CPUPlayer : IPlayer
	{

		/// <summary>
		/// ランダムに<see cref="playerForm"/>の駒を配置します
		/// </summary>
		/// <param name="player"></param>
		/// <param name="model"></param>
		public async void ChangedToMyTurn(PlayerForm player, ITicTacToeModel model)
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
			int row;
			int column;
			(row, column) = VacantPosition(model, model.BoardSize);
			model.PutPiece(row, column, player);
		}

		public void PutPiece(int row, int column, PlayerForm player, ITicTacToeModel model)
		{
			return;
		}

		/// <summary>
		/// 駒が置かれていない座標をランダムに取得します。
		/// </summary>
		/// <returns></returns>
		private (int row, int column) VacantPosition(ITicTacToeModel model, int boardSize)
		{
			int row;
			int column;
			Random random = new Random();
			row = random.Next(0, boardSize);
			column = random.Next(0, boardSize);

			while (!model.BoardStatuses[row, column].Equals(PlayerForm.None))
			{
				row = random.Next(0, boardSize);
				column = random.Next(0, boardSize);
			}

			return (row, column);
		}
	}
}
