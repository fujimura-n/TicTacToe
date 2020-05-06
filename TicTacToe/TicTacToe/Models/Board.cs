using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models
{
	public class Board<Type>
	{
		private readonly Type[,] boardStatuses;
		private int boardSize;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="initValue">ボードの初期値</param>
		/// <param name="boardSize">ボードの大きさ</param>
		public Board(Type initValue, int boardSize)
		{
			this.boardSize = boardSize;
			boardStatuses = new Type[boardSize, boardSize];
			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					boardStatuses[i, j] = initValue;
				}
			}
		}

		/// <summary>
		/// 盤上の駒の配置状態を取得します。
		/// </summary>
		public Type[,] BoardStatuses
		{
			get { return this.boardStatuses; }
		}

		/// <summary>
		/// 盤上に駒を配置します。
		/// ゲームがすでに終了している場合、指定した位置にすでに駒が置かれている場合は何もしません。
		/// </summary>
		/// <param name="row">配置する行</param>
		/// <param name="column">配置する列</param>
		/// <param name="player">配置する駒</param>
		public void PutPiece(int row, int column, Type piece)
		{
			boardStatuses[row, column] = piece;
		}

		/// <summary>
		/// 盤上をリセットします。
		/// </summary>
		/// <param name="initValue">初期値</param>
		public void ResetBoard(Type initValue)
		{
			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					boardStatuses[i, j] = initValue;
				}
			}
		}
	}
}
