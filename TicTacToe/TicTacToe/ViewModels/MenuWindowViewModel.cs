using Livet;
using Livet.Commands;
using Livet.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.Models;
using TicTacToe.Models.Contexts;

namespace TicTacToe.ViewModels
{
	public class MenuWindowViewModel : ViewModel
	{
		private const int BoardSize = 3;
		private const int AlignNumber = 3;
		public void Initialize()
		{
			
		}


		private ViewModelCommand _SelectPvPModeCommand;

		private ViewModelCommand _SelectPvCModeCommand;

		public ViewModelCommand SelectPvPModeCommand
		{
			get
			{
				if (_SelectPvPModeCommand == null)
				{
					_SelectPvPModeCommand = new ViewModelCommand(SelectPvPMode);
				}
				return _SelectPvPModeCommand;
			}
		}
		public ViewModelCommand SelectPvCModeCommand
		{
			get
			{
				if (_SelectPvCModeCommand == null)
				{
					_SelectPvCModeCommand = new ViewModelCommand(SelectPvCMode);
				}
				return _SelectPvCModeCommand;
			}
		}



		public void SelectPvPMode()
		{
			ModelProvider.Instance.TicTacToeModel = new Model(BoardSize, AlignNumber);
			var message = new TransitionMessage(new MainWindowViewModel(), "SelectModeMessageKey");
			Messenger.Raise(message);
		}

		public void SelectPvCMode()
		{
			ModelProvider.Instance.TicTacToeModel = new CPUTicTacToeModel(BoardSize, AlignNumber);
			var message = new TransitionMessage(new MainWindowViewModel(), "SelectModeMessageKey");
			Messenger.Raise(message);
		}
	}
}
