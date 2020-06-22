using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.Models.Contexts
{
	//Modelを提供するクラス
	public class ModelProvider
	{
		/// <summary>
		/// インスタンスを取得します。
		/// </summary>
		public static ModelProvider Instance { get; } = new ModelProvider();

		private ModelProvider(){ }

		/// <summary>
		/// Modelを取得します。
		/// </summary>
		public PlayerInjectionModel TicTacToeModel { get; set; }

	}
}
