using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TicTacToe.Models.Common
{
	public class ImageResourceProvider
	{
		public static ImageBrush CreateImageBrush(string fileName, Stretch stretch = Stretch.Uniform)
		{
			var filePath = "C:/Users/p000526866/git/TicTacToe/TicTacToe/TicTacToe/Resources/";
			return new ImageBrush()
			{
				ImageSource = new BitmapImage(new Uri(filePath + fileName, UriKind.Relative)),
				Stretch = stretch
			};
		}
	}
}
