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
using TicTacToe.Models.Contexts;
using TicTacToe.ViewModels;

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
		private readonly ImageBrush bgImageBrush = new ImageBrush() { ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/Users/p000526866/git/TicTacToe/TicTacToe/TicTacToe/Resources/bg_natural_mori.jpg", UriKind.Relative)) };
		private readonly ImageBrush crossImageBrush = new ImageBrush()
		{
			ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/Users/p000526866/git/TicTacToe/TicTacToe/TicTacToe/Resources/animal_quiz_kuma_batsu.png", UriKind.Relative)),
			Stretch = Stretch.Uniform
		};
		private ImageBrush circleImageBrush = new ImageBrush()
		{
			ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/Users/p000526866/git/TicTacToe/TicTacToe/TicTacToe/Resources/animal_quiz_usagi_maru.png", UriKind.Relative)),
			Stretch = Stretch.Uniform
		};
		private ImageBrush crossWinnerImage = new ImageBrush()
		{
			ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/Users/p000526866/git/TicTacToe/TicTacToe/TicTacToe/Resources/animal_dance_bear.png", UriKind.Relative)),
			Stretch = Stretch.Uniform
		};
		private ImageBrush circleWinnerImage = new ImageBrush()
		{
			ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/Users/p000526866/git/TicTacToe/TicTacToe/TicTacToe/Resources/animal_dance_rabbit.png", UriKind.Relative)),
			Stretch = Stretch.Uniform
		};

		public MainWindow()
		{
			InitializeComponent();
			this.model.BoardChanged += new EventHandler(this.UpdateBoard);
			this.model.CurrentPlayerChanged += new EventHandler((s, e) => this.DisplayCurrentPlayer());
			this.model.GameEnded += new EventHandler<GameEndedEventArgs>(this.ExitGame);
			buttons = new Button[boardSize, boardSize];


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
						case Player.Circle:
							buttons[i, j].Background = circleImageBrush;
							break;
						case Player.Cross:
							buttons[i, j].Background = crossImageBrush;
							break;
						case Player.None:
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
					WinnerImage.Background = circleWinnerImage;
					break;
				case Player.Cross:
					message = "✕の勝ち";
					WinnerImage.Background = crossWinnerImage;
					break;
				case Player.None:
					message = "引き分け";
					break;
			}
			GameResultLabel.Content = message;
		}



	}
}
