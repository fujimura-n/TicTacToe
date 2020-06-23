using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
	public class CleverCPUPlayer : IPlayer
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
			(row, column) = VacantPosition(model, model.BoardSize, player);
			model.PutPiece(row, column, player);
		}

		public void PutPiece(int row, int column, PlayerForm player, ITicTacToeModel model)
		{
			return;
		}

		/// <summary>
		/// 駒が置かれていない座標を取得します。
		/// </summary>
		/// <returns></returns>
		private (int row, int column) VacantPosition(ITicTacToeModel model, int boardSize, PlayerForm player)
		{
			//駒が置かれていない座標をランダムに取得
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

			var list = new List<PlayerForm>();
			var count = 0;

			//縦横斜めの列のうち、自分の駒が一つもないかつ、相手の駒が一つ以上あるかつ、相手の駒の数が多い列に駒を置く

			//行の判定
			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					list.Add(model.BoardStatuses[i, j]);
				}

				if (!list.Contains(player) && list.Contains(player.GetOpponentPlayerForm()) && count < list.Count(element => element == player.GetOpponentPlayerForm()))
				{
					count = list.Count(element => element == player.GetOpponentPlayerForm());
					row = i;
					var index = random.Next(0, list.Count - 1);
					while (list[index] != PlayerForm.None)
					{
						index = random.Next(0, list.Count - 1);
					}
					column = index;
				}
				list.Clear();
			}

			//列の判定
			for (int j = 0; j < boardSize; j++)
			{
				for (int i = 0; i < boardSize; i++)
				{
					list.Add(model.BoardStatuses[i, j]);
				}

				if (!list.Contains(player) && list.Contains(player.GetOpponentPlayerForm()) && count < list.Count(element => element == player.GetOpponentPlayerForm()))
				{
					count = list.Count(element => element == player.GetOpponentPlayerForm());
					var index = random.Next(0, list.Count - 1);
					while (list[index] != PlayerForm.None)
					{
						index = random.Next(0, list.Count - 1);
					}
					row = index;
					column = j;
				}
				list.Clear();
			}

			//右斜めの判定
			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					var x = i;
					var y = j;
					while (0 <= x && y < boardSize)
					{
						list.Add(model.BoardStatuses[x, y]);
						x--;
						y++;
					}
					if (list.Count < model.AlignNumber)
					{
						list.Clear();
						break;
					}
					if (!list.Contains(player) && list.Contains(player.GetOpponentPlayerForm()) && count < list.Count(element => element == player.GetOpponentPlayerForm()))
					{
						count = list.Count(element => element == player.GetOpponentPlayerForm());
						var index = random.Next(0, list.Count - 1);
						while (list[index] != PlayerForm.None)
						{
							index = random.Next(0, list.Count - 1);
						}
						row = i - index;
						column = j + index;
					}
					list.Clear();
				}
			}

			//左斜めの判定
			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					var x = i;
					var y = j;
					while (x < boardSize && y < boardSize)
					{
						list.Add(model.BoardStatuses[x, y]);
						x++;
						y++;
					}
					if (list.Count < model.AlignNumber)
					{
						list.Clear();
						break;
					}
					if (!list.Contains(player) && list.Contains(player.GetOpponentPlayerForm()) && count < list.Count(element => element == player.GetOpponentPlayerForm()))
					{
						count = list.Count(element => element == player.GetOpponentPlayerForm());
						var index = random.Next(0, list.Count - 1);
						while (list[index] != PlayerForm.None)
						{
							index = random.Next(0, list.Count - 1);
						}
						row = i + index;
						column = j + index;
					}
					list.Clear();
				}
			}

			return (row, column);
		}
	}
}
