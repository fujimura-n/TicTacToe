using Livet;
using Livet.Commands;
using Livet.Messaging;
using System.Collections.ObjectModel;
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

		private ViewModelCommand _GameStartCommand;

		public ViewModelCommand GameStartCommand
		{
			get
			{
				if(_GameStartCommand == null)
				{
					_GameStartCommand = new ViewModelCommand(GameStart);
				}
				return _GameStartCommand;
			}
		}
		public ObservableCollection<string> ChoiceList { get; } = new ObservableCollection<string>() { "人間", "CPU", "CPU (つよい)" };

		public string CirclePlayer { get; set; }

		public string CrossPlayer { get; set; }

		public void GameStart()
		{
			if(CirclePlayer == null || CrossPlayer == null)
			{
				return;
			}
			ModelProvider.Instance.TicTacToeModel = new PlayerInjectionModel(BoardSize, AlignNumber, GetPlayerInstance(CirclePlayer), GetPlayerInstance(CrossPlayer));
			var message = new TransitionMessage(new MainWindowViewModel(), "SelectModeMessageKey");
			Messenger.Raise(message);
		}

		private IPlayer GetPlayerInstance(string player)
		{
			if (player == "人間")
			{
				return new PersonPlayer();
			}
			else if (player == "CPU")
			{
				return new CPUPlayer();
			}
			else if(player == "CPU (つよい)")
			{
				return new CleverCPUPlayer();
			}
			else
			{
				return new PersonPlayer();
			}
		}

	}
}
