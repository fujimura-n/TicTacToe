using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TicTacToe.Models;

namespace TicTacToe.Views
{
	/* 
     * If some events were receive from ViewModel, then please use PropertyChangedWeakEventListener and CollectionChangedWeakEventListener.
     * If you want to subscribe custome events, then you can use LivetWeakEventListener.
     * When window closing and any timing, Dispose method of LivetCompositeDisposable is useful to release subscribing events.
     *
     * Those events are managed using WeakEventListener, so it is not occurred memory leak, but you should release explicitly.
     */
	public partial class MainWindow : Window
	{
		private const int BoardSize = 5;
		private const int ButtonSize = 100;
		private Model model = new Model(BoardSize);
		private const string Circle = "○";
		private const string Cross = "✕";
		private readonly Button[,] buttons = new Button[BoardSize, BoardSize];


		public MainWindow()
		{
			InitializeComponent();
			this.model.BoardChanged += new EventHandler(this.UpdateBoard);
			this.model.CurrentPlayerChanged += new EventHandler((s, e) => this.DisplayCurrentPlayer());
			this.model.GameEnded += new EventHandler<GameEndedEventArgs>(this.ExitGame);

			//Gridの設定
			for (int i = 0; i < BoardSize; i++)
			{
				ColumnDefinition colDef = new ColumnDefinition();
				RowDefinition rowDef = new RowDefinition();
				BoadGrid.ColumnDefinitions.Add(colDef);
				BoadGrid.RowDefinitions.Add(rowDef);
			}

			//Buttonの配置
			for (int i = 0; i < BoardSize; i++)
			{
				for (int j = 0; j < BoardSize; j++)
				{
					Button button = new Button()
					{
						Width = ButtonSize,
						Height = ButtonSize,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
					};

					var row = i;
					var column = j;
					button.Click += new RoutedEventHandler((s, e) => 
					{
						model.PutPiece(row, column, model.CurrentPlayer);
					});

					buttons[i, j] = button;
					BoadGrid.Children.Add(buttons[i, j]);
					Grid.SetRow(buttons[i, j], i);
					Grid.SetColumn(buttons[i, j], j);
				}
			}

			//Labelの設定
			DisplayCurrentPlayer();

			//ResetButtonの設定
			ResetButton.Click += new RoutedEventHandler((s, e) =>
			{
				this.Reset();
			});

			//GameResultLabelの設定
			GameResultLabel.Content = String.Empty;



		}
		/// <summary>
		/// 盤の表示を更新します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpdateBoard(object sender, EventArgs e)
		{
			//boardStatusesの更新
			for (int i = 0; i < BoardSize; i++)
			{
				for (int j = 0; j < BoardSize; j++)
				{
					var status = model.BoardStatuses[i, j];
					switch (status)
					{
						case Player.Circle:
							buttons[i, j].Content = Circle;
							break;
						case Player.Cross:
							buttons[i, j].Content = Cross;
							break;
						case Player.None:
							buttons[i, j].Content = String.Empty;
							break;
					}
				}
			}
		}

		/// <summary>
		/// ゲームをリセットします。
		/// </summary>
		public void Reset()
		{
			this.model.ResetGame();
			DisplayCurrentPlayer();
			GameResultLabel.Content = String.Empty;
		}

		//現在のプレーヤーを表示します。
		private void DisplayCurrentPlayer()
		{
			var messasge = String.Empty;
			switch (model.CurrentPlayer)
			{
				case Player.Circle:
					messasge = "○の番です";
					break;
				case Player.Cross:
					messasge = "✕の番です";
					break;
				case Player.None:
					messasge = String.Empty;
					break;

			}
			CurrentPlayerLabel.Content = messasge;
		}

		//ゲームの終了処理をします。
		private void ExitGame(object sender, GameEndedEventArgs e)
		{
			CurrentPlayerLabel.Content = String.Empty;
			SetGameResultLabel(e.Winner);
		}

		/// <summary>
		/// ゲームの結果を表示します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SetGameResultLabel(Player winner)
		{
			var message = String.Empty;
			switch (winner)
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
			GameResultLabel.Content = message;
		}



	}
}
