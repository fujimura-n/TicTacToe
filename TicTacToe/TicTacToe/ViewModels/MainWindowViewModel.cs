using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using TicTacToe.Models;
using System.Threading.Tasks;

namespace TicTacToe.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		private Model model = new Model();
		private const int BoardSize = 3;
		private const string Circle = "○";
		private const string Cross = "✕";
		private string[,] boardStatuses = new string[BoardSize, BoardSize]
		{
			{ string.Empty, string.Empty, string.Empty },
			{ string.Empty, string.Empty, string.Empty },
			{ string.Empty, string.Empty, string.Empty },
		};
		private string label;
		private string label_00;
		private string label_01;
		private string label_02;
		private string label_10;
		private string label_11;
		private string label_12;
		private string label_20;
		private string label_21;
		private string label_22;

		// Some useful code snippets for ViewModel are defined as l*(llcom, llcomn, lvcomm, lsprop, etc...).
		public void Initialize()
		{
			this.model.BoardChanged += new EventHandler(this.UpdateBoard);
			this.model.GameEnded += new EventHandler<GameEndedEventArgs>(this.SetGameResultLabel);
		}		

		public string Label
		{
			get => this.label;
			set { this.label = value; this.RaisePropertyChanged(); }
		}

		public string Button_00Label
		{
			get => this.label_00;
			set { this.label_00 = value; this.RaisePropertyChanged(); }
		}

		public string Button_01Label
		{
			get => this.label_01;
			set { this.label_01 = value; this.RaisePropertyChanged(); }
		}

		public string Button_02Label
		{
			get => this.label_02;
			set { this.label_02 = value; this.RaisePropertyChanged(); }
		}

		public string Button_10Label
		{
			get => this.label_10;
			set { this.label_10 = value; this.RaisePropertyChanged(); }
		}

		public string Button_11Label
		{
			get => this.label_11;
			set { this.label_11 = value; this.RaisePropertyChanged(); }
		}

		public string Button_12Label
		{
			get => this.label_12;
			set { this.label_12 = value; this.RaisePropertyChanged(); }
		}

		public string Button_20Label
		{
			get => this.label_20;
			set { this.label_20 = value; this.RaisePropertyChanged(); }
		}

		public string Button_21Label
		{
			get => this.label_21;
			set { this.label_21 = value; this.RaisePropertyChanged(); }
		}

		public string Button_22Label
		{
			get => this.label_22;
			set { this.label_22 = value; this.RaisePropertyChanged(); }
		}

		/// <summary>
		/// 盤上のセルが選択されたときの処理を行います。
		/// </summary>
		/// <param name="cellNum">選択されたセルの番号(行番号+ 列番号)</param>
		public void Clicked(string cellNum)
		{
			//行番号と列番号を分離して取り出す
			string[] arr = cellNum.Split(',');
			int row = int.Parse(arr[0]);
			int column = int.Parse(arr[1]);

			this.model.PutPiece(row, column, model.CurrentPlayer);
		}

		/// <summary>
		/// ゲームをリセットします。
		/// </summary>
		public void Reset()
		{
			this.model.ResetGame();
			this.Label = string.Empty;
		}

		/// <summary>
		/// 盤の表示を更新します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdateBoard(object sender, EventArgs e)
		{
			for (int i = 0; i < BoardSize; i++)
			{
				for (int j = 0; j < BoardSize; j++)
				{
					switch(this.model.BoardStatuses[i, j])
					{
						case Player.Circle:
							this.boardStatuses[i, j] = Circle;
							break;
						case Player.Cross:
							this.boardStatuses[i, j] = Cross;
							break;
						case Player.None:
							this.boardStatuses[i, j] = string.Empty;
							break;
					}	
				}
			}
			SetLabels();
		}

		/// <summary>
		/// ゲームの結果を表示します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SetGameResultLabel(object sender, GameEndedEventArgs e)
		{
			string message = string.Empty;
			switch (e.Winner)
			{
				case Player.Circle:
					message = "○の勝ち";
					break;
				case Player.Cross:
					message = "✕の勝ち";
					break;
				case Player.None:
					message = "引き分け";
					break;
			}
			Label = $"ゲーム結果: {message}";
		}

		/// <summary>
		/// <see cref="boardStatuses"/>の値をViewのLabelに反映します。
		/// </summary>
		private void SetLabels()
		{
			Button_00Label = this.boardStatuses[0, 0];
			Button_01Label = this.boardStatuses[0, 1];
			Button_02Label = this.boardStatuses[0, 2];
			Button_10Label = this.boardStatuses[1, 0];
			Button_11Label = this.boardStatuses[1, 1];
			Button_12Label = this.boardStatuses[1, 2];
			Button_20Label = this.boardStatuses[2, 0];
			Button_21Label = this.boardStatuses[2, 1];
			Button_22Label = this.boardStatuses[2, 2];
		}


	}
}
