using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MusicWord.Models
{
    class LevelOneModel : PropertyChangedBase
	{
		
		private double _percentage;
		private HashSet<char> _guesses;
		private HashSet<char> _wordLetters;
		private string _word;
		private bool _timeOver;
		private TimeModel _timer;
		private string _hiddenWord;
		public LevelOneModel(string word, TimeModel timer, double percentage = 0.25)
		{
			
			this._percentage = percentage;
			this._guesses = creatDeafultGuesses(word);
			this._word = word;
			this._wordLetters = getWordLetters(word.ToLower());
			// to show the letters to the player
			this._timeOver = false;
			this._timer = timer;
			this._timer.start();
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
		private void timeIsOver(object sender, EventArgs e)
		{
			_timeOver = true;
		}
		private string shuffle(string str)
		{
			char[] array = str.ToCharArray();
			Random rng = new Random();
			int n = array.Length;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				var value = array[k];
				array[k] = array[n];
				array[n] = value;
			}
			return new String(array);
		}
		private HashSet<char> getWordLetters(string word)
		{
			var letters = new HashSet<char>();
			foreach (var letter in word)
				letters.Add(letter);
			return letters;
		}

		private HashSet<char> creatDeafultGuesses(string word)
		{
			var letters = new HashSet<char>();
			var showenLetters = new HashSet<char>();
			int numOfShowenLetters;
			// get shuffle word array
			string shuffleWord = shuffle(word.ToLower());

			// create set of word letters
			letters = getWordLetters(shuffleWord);
			numOfShowenLetters = (int)Math.Ceiling(this._percentage * letters.Count);

			// create set of only showen letters
			int i = 0;
			foreach (var letter in letters)
			{
				if (i < numOfShowenLetters)
					showenLetters.Add(letter);
				else
					break;
				i++;
			}
			return showenLetters;

		}
		private void updateHiddenWord()
		{
			string wordString = "";
			foreach (var letter in this._word)
			{
				if (this._guesses.Contains(Char.ToLower(letter)))
					wordString += letter + " ";

				else
					wordString += "__ ";
			}
			
			HiddenWord = wordString;
		}


		private bool isSolved()
		{
			foreach (var letter in this._word)
				if (!this._guesses.Contains(Char.ToLower(letter)))
					return false;
			return true;
		}

		public void EnterGuess(string guess)
		{
			try
			{
				// check if input is valid
				char letter = char.Parse(guess);
				if (Char.IsLetter(letter))
				{
					// add to guess the new letter and print the word
					this._guesses.Add(Char.ToLower(letter));
					updateHiddenWord();
				}
			}
			// if there is an exception the input is not valid we ignore it
			catch (ArgumentNullException ) { }
			catch (FormatException )  { }
		}

		public void start()
		{
			updateHiddenWord();
			//string guess;
			//char letter;
			////_timer.eventHandler += timeIsOver;
			//_timer.start();
			//updateHiddenWord();
			//while (!isSolved() && !_timeOver)
			//{
			//	Console.WriteLine("Enter ypur guess:");
			//	guess = Console.ReadLine();
			//	try
			//	{
			//		// check if input is valid
			//		letter = char.Parse(guess);
			//		if (Char.IsLetter(letter))
			//		{
			//			// add to guess the new letter and print the word
			//			this._guesses.Add(Char.ToLower(letter));
			//			updateHiddenWord();
			//		}
			//	}
			//	catch
			//	{
			//		continue;
			//	}

			//}
		}
	}
}
