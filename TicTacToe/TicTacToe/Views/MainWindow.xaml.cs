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
using TicTacToe.Models.Common;
using TicTacToe.Models.Contexts;

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
		private const int ButtonSize = 110;
		private readonly Button[,] buttons;
		private readonly ITicTacToeModel model = ModelProvider.Instance.TicTacToeModel;
		private readonly int boardSize = ModelProvider.Instance.TicTacToeModel.BoardSize;
		private readonly ImageBrush bgImageBrush = ImageResourceProvider.CreateImageBrush("bg_natural_mori.jpg", Stretch.Fill);
		private readonly ImageBrush crossImageBrush = ImageResourceProvider.CreateImageBrush("animal_quiz_kuma_batsu.png");
		private readonly ImageBrush circleImageBrush = ImageResourceProvider.CreateImageBrush("animal_quiz_usagi_maru.png");
		private readonly ImageBrush crossWinnerImage = ImageResourceProvider.CreateImageBrush("animal_dance_bear.png");
		private readonly ImageBrush circleWinnerImage = ImageResourceProvider.CreateImageBrush("animal_dance_rabbit.png");

		public MainWindow()
		{
			InitializeComponent();
			buttons = new Button[boardSize, boardSize];
			this.model.BoardChanged += new EventHandler(this.UpdateBoard);
			this.model.CurrentPlayerChanged += new EventHandler((s, e) => this.DisplayCurrentPlayer());
			this.model.GameEnded += new EventHandler<GameEndedEventArgs>(this.ExitGame);

			//Gridの設定
			for (int i = 0; i < boardSize; i++)
			{
				ColumnDefinition colDef = new ColumnDefinition();
				RowDefinition rowDef = new RowDefinition();
				BoadGrid.ColumnDefinitions.Add(colDef);
				BoadGrid.RowDefinitions.Add(rowDef);
				Background = bgImageBrush;
			}

			//Buttonの配置
			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					Button button = new Button()
					{
						Width = ButtonSize,
						Height = ButtonSize,
						HorizontalAlignment = HorizontalAlignment.Center,
						VerticalAlignment = VerticalAlignment.Center,
						Opacity = 0.9,
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

			this.model.StartGame();

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
			for (int i = 0; i < boardSize; i++)
			{
				for (int j = 0; j < boardSize; j++)
				{
					var status = model.BoardStatuses[i, j];
					switch (status)
					{
						case PlayerForm.Circle:
							buttons[i, j].Background = circleImageBrush;
							break;
						case PlayerForm.Cross:
							buttons[i, j].Background = crossImageBrush;
							break;
						case PlayerForm.None:
							buttons[i, j].Background = null;
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
			WinnerImage.Background = null;
		}

		//現在のプレーヤーを表示します。
		private void DisplayCurrentPlayer()
		{
			var messasge = String.Empty;
			switch (model.CurrentPlayer)
			{
				case PlayerForm.Circle:
					messasge = "○の番です";
					break;
				case PlayerForm.Cross:
					messasge = "✕の番です";
					break;
				case PlayerForm.None:
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
		private void SetGameResultLabel(PlayerForm winner)
		{
			var message = String.Empty;
			switch (winner)
			{
				case PlayerForm.Circle:
					message = "○の勝ち";
					WinnerImage.Background = circleWinnerImage;
					break;
				case PlayerForm.Cross:
					message = "✕の勝ち";
					WinnerImage.Background = crossWinnerImage;
					break;
				case PlayerForm.None:
					message = "引き分け";
					break;
			}
			GameResultLabel.Content = message;
		}

	}
}
