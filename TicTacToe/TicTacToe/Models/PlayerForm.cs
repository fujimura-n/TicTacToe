using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models
{
	public enum PlayerForm
	{
		Circle,
		Cross,
		None,
	}
	public static partial class EnumExtend
	{
		public static PlayerForm GetOpponentPlayerForm(this PlayerForm player)
		{
			var value = PlayerForm.None;
			switch (player)
			{
				case PlayerForm.Circle:
					value = PlayerForm.Cross;
					break;
				case PlayerForm.Cross:
					value = PlayerForm.Circle;
					break;
			}
			return value;
		}
	}
}
