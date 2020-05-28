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

namespace TicTacToe.Views
{
	/* 
     * If some events were receive from ViewModel, then please use PropertyChangedWeakEventListener and CollectionChangedWeakEventListener.
     * If you want to subscribe custome events, then you can use LivetWeakEventListener.
     * When window closing and any timing, Dispose method of LivetCompositeDisposable is useful to release subscribing events.
     *
     * Those events are managed using WeakEventListener, so it is not occurred memory leak, but you should release explicitly.
     */
	public partial class MenuWindow : Window
	{
		private ImageBrush bgImageBrush = new ImageBrush()
		{
			ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/Users/p000526866/git/TicTacToe/TicTacToe/TicTacToe/Resources/bg_natural_sougen.jpg", UriKind.Relative)),
			Stretch = Stretch.Uniform
		};
		private ImageBrush titleImageBrush = new ImageBrush()
		{
			ImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/Users/p000526866/git/TicTacToe/TicTacToe/TicTacToe/Resources/tictactoe.png", UriKind.Relative)),
			Stretch = Stretch.Uniform
		};

		public MenuWindow()
		{
			InitializeComponent();
			MainGrid.Background = bgImageBrush;
			TitleGrid.Background = titleImageBrush;

		}
	}
}