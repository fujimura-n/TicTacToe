using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models
{
	public interface IPlayer
	{
		/// <summary>
		/// 自分の番になったときに呼ばれる関数
		/// </summary>
		/// <param name="player"></param>
		/// <param name="model"></param>
		void ChangedToMyTurn(PlayerForm player, ITicTacToeModel model);

		/// <summary>
		/// ユーザーからボタン入力があった場合に呼ばれる関数
		/// </summary>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <param name="player"></param>
		/// <param name="model"></param>
		void PutPiece(int row, int column, PlayerForm player, ITicTacToeModel model);
	}
}
