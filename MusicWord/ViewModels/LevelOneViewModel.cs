using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
using System.Reflection;
using System.Windows;

namespace MusicWord.ViewModels
{
    class LevelOneViewModel:AppScreen
    {
		private string _time;
		private string _letterGuess;
		private string _hiddenWord;
		private TimeModel _timer;
		private LevelOneModel _game;
        private CluesModel _cluesGenrator;
        private string _clue;
        public LevelOneViewModel()
		{

            ICategory category = SQLServerModel.getWord(PlayerModel.Instance.Category);
            _cluesGenrator = new CluesModel(category);
            string word = category.Name;
            _timer = new TimeModel(80);
			_timer.PropertyChanged += _timer_PropertyChanged;
			_game = new LevelOneModel(word, _timer, Globals.hidddenPercentage);
			_game.PropertyChanged += _game_PropertyChanged;
			_game.start();
			//_timer.start();
		}

		private void _game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			HiddenWord = _game.HiddenWord;
		}

		private void _timer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			PropertyInfo property = sender.GetType().GetProperty(e.PropertyName);
			int secondes =(int) property.GetValue(sender, null); 
			TimeSpan t = TimeSpan.FromSeconds(secondes);
			Time = t.ToString();
		}
		public string Time
		{
			get { return _time; }
			set { 
				_time = value;
				NotifyOfPropertyChange(() => Time);
			}
		}



		public string HiddenWord
		{
			get { return _hiddenWord; }
			set
			{
				_hiddenWord = value;
				NotifyOfPropertyChange(() => HiddenWord);
			}
		}

		public string LetterGuess
		{
			get { return _letterGuess; }
			set 
			{ 
				_letterGuess = value;
				NotifyOfPropertyChange(() => LetterGuess);
			}
		}
		public void CheckLetter()
		{
			if (!String.IsNullOrEmpty(LetterGuess))
			{

				_game.EnterGuess(LetterGuess);
				LetterGuess = "";

			}
		}
        

        public string Clue
        {
            get { return _clue ; }
            set { _clue = value; }
        }

        public void GetClue()
        {
            string clue = _cluesGenrator.getClue();
            Clue = clue;
            NotifyOfPropertyChange(() => Clue);
        }


	}
}
