using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MusicWord.Models
{
    class TimeModel : PropertyChangedBase
	{
		private int _secondes;
		private Timer timer;
		public TimeModel(int secondes)
		{
			this._secondes = secondes;
		}
		

		public int Secondes
		{
			get { return _secondes; }
		}

		private void OnTimedEvent(Object source, ElapsedEventArgs e)
		{
			_secondes -= 1;
			if (_secondes == 0)
			{
				timer.Stop();
				timer.Dispose();
				
			}
			NotifyOfPropertyChange(() => Secondes);
		}
		public void start()
		{
			// timer for one seconed intervals
			timer = new Timer(1000);
			timer.Elapsed += OnTimedEvent;
			timer.AutoReset = true;
			timer.Enabled = true;
		}
	}
}
