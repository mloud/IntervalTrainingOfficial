using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


public class Timer : MonoBehaviour
{
	public enum State
	{
		Stopped = 0,
		Running,
		Paused
	}

	public State CurrentState { get; private set; }

	public delegate void TimerStartedDelegate(Config cfg);
	public delegate void TimerEndedDelegate();
	public delegate void TimerPauseDelegate(IntervalDefinition intervalDef);
	public delegate void TimerUnPauseDelegate(IntervalDefinition intervalDef);
	public delegate void TimerResetDelegate();
	public delegate void IntervalStartedDelegate(IntervalDefinition intervalDef);
	public delegate void IntervalEndedDelegate();
	public delegate void IntervalTickDelegate(int index, int count);

	[System.Serializable]
	public class IntervalDefinition
	{
		public IntervalDefinition ShallowCopy()
		{
			return (IntervalDefinition)this.MemberwiseClone ();
		}

		public int hours;
		public int minutes;
		public int seconds;
	
		public string Name;
		public float MusicVolume;
		public Col.ColorType ColorType;
		public string InfoText;
		public float Duration() { return hours * 3600 + minutes * 60 + seconds; }
	}

	[SerializeField]
	private Config newConfig;

	[System.Serializable]
	public class Config
	{
		public List<IntervalDefinition> Intervals;

	
		public int RepetitionCount;
		public int NoticeTicks;
		public int RepeatFromIndex;
		public int RepeatToIndex;
		public int Order;

		public string Name;
		public string MusicName;
		public bool LenghtFromMusic;
		public bool LoopMusic;
		public bool Enabled;
		public bool Removable = true;
		public bool Sound = true;
		public string Id;

		public Config Clone()
		{
			Config config = new Config ();

			config.RepetitionCount = RepetitionCount;
			config.NoticeTicks = NoticeTicks;
			config.RepeatFromIndex = RepeatFromIndex;
			config.RepeatToIndex = RepeatToIndex;
			config.Order = Order;
			config.Name = Name;
			config.MusicName = MusicName;
			config.LenghtFromMusic = LenghtFromMusic;
			config.LoopMusic = LoopMusic;
			config.Enabled = Enabled;
			config.Sound = Sound;
			config.Id = Id;

			config.Intervals = new List<IntervalDefinition> ();

			for (int i = 0; i < Intervals.Count; ++i)
			{
				config.Intervals.Add(Intervals[i].ShallowCopy());
			}


			return config;

		}

		public void SetFrom(Config config)
		{
			RepetitionCount = config.RepetitionCount;
			NoticeTicks = config.NoticeTicks;
			MusicName = config.MusicName;
			LenghtFromMusic = config.LenghtFromMusic;
			LoopMusic = config.LoopMusic;
			RepeatToIndex = config.RepeatToIndex;
			RepeatFromIndex = config.RepeatFromIndex;
			Sound = config.Sound;
			Id = config.Id;
			Name = config.Name;


			Intervals = new List<IntervalDefinition>(config.Intervals.Count);

			for (int i = 0; i < config.Intervals.Count; ++i)
			{
				IntervalDefinition intervalDefinition = new IntervalDefinition();
				intervalDefinition.ColorType = config.Intervals[i].ColorType;
				intervalDefinition.Name = config.Intervals[i].Name;
				intervalDefinition.hours = config.Intervals[i].hours;
				intervalDefinition.minutes = config.Intervals[i].minutes;
				intervalDefinition.MusicVolume = config.Intervals[i].MusicVolume;
				intervalDefinition.seconds = config.Intervals[i].seconds;
				intervalDefinition.InfoText = config.Intervals[i].InfoText; 

				Intervals.Add(intervalDefinition);
			}

		}

	}

	[SerializeField]
	public Config Cfg;

	[SerializeField]
	public List<Config> Presets;


	public TimerStartedDelegate TimerStarted { get; set; }
	public TimerEndedDelegate TimerEnded { get; set; }
	public TimerPauseDelegate TimerPause { get; set; }
	public TimerUnPauseDelegate TimerUnPause { get; set; }
	public TimerResetDelegate TimerReset { get; set; }
	public IntervalEndedDelegate IntervalEnded { get; set; }
	public IntervalStartedDelegate IntervalStarted { get; set; }
	public IntervalTickDelegate IntervalTick { get; set; }


	private StringBuilder StringBuilder { get; set; }

	private Stopwatch MainStopwatch { get; set; } 
	//private Stopwatch IntervalStopwatch { get; set; }


	private float NextIntervalElapsed { get; set; }
	private float StartntervalElapsed { get; set; }
	private float MainTimerElapsed { get; set; }

	private bool DirtyCurrentIntervalElapsedTime { get; set; }

	public class IntervalState
	{
		public List<float> NoticeTicks { get; set; }

		public int Index { get; set; }
		public int Round { get; set; }

		public IntervalState(int noticeTicksCount)
		{
			NoticeTicks = new List<float>(noticeTicksCount);
			for (int i = 0; i < noticeTicksCount; ++i)
			{
				NoticeTicks.Add(0);
			}
		}
	}

	public IntervalState CurrentIntervalState { get; private set; }

	void Awake()
	{
		StringBuilder = new StringBuilder(15,15); // max capacity for hh:mm:ss:ms
		CurrentState = State.Stopped;
		CurrentIntervalState = new IntervalState(Cfg.NoticeTicks);

		Presets.Sort (delegate(Config x, Config y) {
						if (x.Order < y.Order)
								return -1;
						else if (x.Order > y.Order)
								return 1;

						return 0;
				});


		MainStopwatch = new Stopwatch ();
		//IntervalStopwatch = new Stopwatch ();
	}


	void Update () 
	{
		MainTimerElapsed = (float)MainStopwatch.Elapsed.TotalSeconds;

		if (DirtyCurrentIntervalElapsedTime)
		{
			NextIntervalElapsed = StartntervalElapsed + CurrentIntervalDuration ();

			SetNoticeTicksForCurrentInterval();

			DirtyCurrentIntervalElapsedTime = false;
		}

		if (CurrentState == State.Running)
		{
				// Check for end
			if (MainTimerElapsed >=  Duration())
			{
				// call end delegates
				if (TimerEnded != null)
				{
					TimerEnded();
				}

				CurrentState = State.Stopped;
			}
			else
			{
				for (int i = 0; i < CurrentIntervalState.NoticeTicks.Count; ++i)
				{
					float noticeTime = CurrentIntervalState.NoticeTicks[i];
					if (noticeTime > 0 && MainTimerElapsed > noticeTime)
					{
						CurrentIntervalState.NoticeTicks[i] = -1;
						
						if (IntervalTick != null)
						{
							IntervalTick( i, CurrentIntervalState.NoticeTicks.Count);
						}
					}
				}
				
				
				// Check for interval end
				//if (timeNow > CurrentIntervalState.EndTime)
				//if (IntervalStopwatch.Elapsed.TotalSeconds >= Cfg.Intervals[CurrentIntervalState.Index].Duration())
				if (MainTimerElapsed >= NextIntervalElapsed)
				{
					if (IntervalEnded != null)
					{
						IntervalEnded();
					}

					SetNextInterval();

					//IntervalStopwatch.Reset ();
					//IntervalStopwatch.Start();


					
					if (IntervalStarted != null)
					{
						IntervalStarted(Cfg.Intervals[CurrentIntervalState.Index]);
					}
				}
			}
		}
	}

	public void SetPresetConfig(string name)
	{
		Cfg.SetFrom(Presets.Find(x=>x.Name == name));
	}


	public void SetDirtyCurrentIntervalElapsedTime()
	{
		DirtyCurrentIntervalElapsedTime = true;
	}

	public void Run()
	{
		CurrentState = State.Running;

		MainStopwatch.Reset ();
		MainStopwatch.Start ();

		//IntervalStopwatch.Reset ();
		//IntervalStopwatch.Start ();
	
		CurrentIntervalState.Index = 0;
		CurrentIntervalState.Round = 0;
		NextIntervalElapsed = MainTimerElapsed + CurrentIntervalDuration ();
		StartntervalElapsed = 0;

		SetNoticeTicksForCurrentInterval ();

		if (IntervalStarted != null)
		{
			IntervalStarted(Cfg.Intervals[CurrentIntervalState.Index]);
		}

		if (TimerStarted != null)
		{
			TimerStarted(Cfg);
		}
		
	}
	
	public void Reset()
	{
		CurrentState = State.Stopped;

		CurrentIntervalState.Index = 0;


		MainStopwatch.Reset ();
		//IntervalStopwatch.Reset ();

		if (TimerReset != null)
		{
			TimerReset();
		}
	}

	private void SetNextInterval()
	{
		CurrentIntervalState.Index++;
		if (CurrentIntervalState.Index == Cfg.RepeatToIndex + 1)
		{
			if (CurrentIntervalState.Round == Cfg.RepetitionCount)
			{

			}
			else
			{
				CurrentIntervalState.Index = Cfg.RepeatFromIndex;
				CurrentIntervalState.Round++;
			}


		}
		else if (CurrentIntervalState.Index == Cfg.RepeatFromIndex)
		{
			CurrentIntervalState.Round++;
		}
		


	
		StartntervalElapsed = MainTimerElapsed;
		NextIntervalElapsed = MainTimerElapsed + CurrentIntervalDuration ();

		SetNoticeTicksForCurrentInterval ();
	}

	private void SetNoticeTicksForCurrentInterval()
	{
		float duration = CurrentIntervalDuration (); 
		int noticeCount = CurrentIntervalState.NoticeTicks.Count;

		for ( int i = 0; i < noticeCount; ++i)
		{
			CurrentIntervalState.NoticeTicks[i] = NextIntervalElapsed - (noticeCount - i - 1);
		}
	}

	public float TotalElapsedTime()
	{
		float result = 0;

		if (CurrentState == State.Stopped)
		{
			result = 0;
		}
		else
		{
			result = MainTimerElapsed;
		}

		return result;
	}

	public float TotalRestTime()
	{
		return  Duration () - TotalElapsedTime ();
	}

	public string TotalRestTimeFormatted(bool useMillis)
	{
		return GetFormattedTime(TotalRestTime(), useMillis);
	}


	public string TotalElapsedTimeFormatted(bool useMillis)
	{
		return GetFormattedTime(TotalElapsedTime(), useMillis);
	}

	public string TotalDurationFormatted(bool useMill)
	{
		return GetFormattedTime(Duration(), useMill);
	}


	public float CurrentIntervalDuration()
	{
		return Cfg.Intervals [CurrentIntervalState.Index].Duration ();
	}

	public string GetFormattedTime(float timeSec, bool useMillis = true, bool showZeroHours = false, bool showZeroMinutes = false)
	{

		if (timeSec < 0)
			timeSec = 0;

		TimeSpan ts = System.TimeSpan.FromSeconds(timeSec);
		
		int hours  = (int)System.Math.Truncate(ts.TotalHours);
		int minutes = ts.Minutes;
		int seconds = ts.Seconds;
		int millis = ts.Milliseconds / 100;

		//clear
		StringBuilder.Length = 0;

		if (showZeroHours || hours > 0)
		{
			StringBuilder.Append( hours < 10 ? "0" + hours : hours.ToString());
			StringBuilder.Append(":");
		}
		if (showZeroMinutes || (hours != 0 || minutes != 0))
		{
			StringBuilder.Append( minutes < 10 ? "0" + minutes : minutes.ToString());
			StringBuilder.Append(":");
		}
		StringBuilder.Append( seconds < 10 ? "0" + seconds : seconds.ToString());

		if (useMillis)
		{	
			StringBuilder.Append(":");
			StringBuilder.Append(millis.ToString());
		}

		String str = StringBuilder.ToString();

	
		if (str.StartsWith("0"))
		{
			str = str.Remove(0,1);	
		}

		return str;

//		if (hours == 0)
//		{
//			return string.Format("{0}:{1:ss}:{2}", minutes.ToString(), seconds.ToString(), millis.ToString());
//		}
//		else
//			return string.Format("{0}:{1:ss}:{2}:{3}", hours.ToString(), minutes.ToString(), seconds.ToString(), millis.ToString());
	}

	public string CurrentIntervalDurationFormatted(bool useMillis)
	{
		return GetFormattedTime(CurrentIntervalDuration(), useMillis);
	}

	public string CurrentIntervalElapsedTimeFormatted(bool useMillis)
	{
		return GetFormattedTime(CurrentIntervalElapsedTime(), useMillis);
	}

	public float CurrentIntervalElapsedTime()
	{
		return  CurrentState == State.Stopped ? 0 : MainTimerElapsed - StartntervalElapsed;
	}

	public float CurrentIntervalRestTime()
	{
		//return  CurrentState == State.Stopped ? CurrentIntervalDuration () : CurrentIntervalDuration () - (float)IntervalStopwatch.Elapsed.TotalSeconds;
		return  CurrentState == State.Stopped ? CurrentIntervalDuration () : NextIntervalElapsed - MainTimerElapsed;
	}

	public string CurrentIntervalRestTimeFormatted(bool useMillis)
	{
		return GetFormattedTime(CurrentIntervalRestTime(), useMillis);
	}

	public int RoundCount()
	{
		return Cfg.RepetitionCount;
	}

	public int CurrentRound()
	{
		return CurrentIntervalState.Round;
	}



	public void Pause()
	{
		CurrentState = State.Paused;
		//Time.timeScale = 0;

		MainStopwatch.Stop ();
		//IntervalStopwatch.Stop ();

		if (TimerPause != null)
		{
			TimerPause(Cfg.Intervals[CurrentIntervalState.Index]);
		}

	}

	public void UnPause()
	{
	
		CurrentState = State.Running;
		//Time.timeScale = 1;
		MainStopwatch.Start ();
		//IntervalStopwatch.Start ();
		
		if (TimerUnPause != null)
		{
			TimerUnPause(Cfg.Intervals[CurrentIntervalState.Index]);
		}
	}


	public float Duration()
	{
		float timeSum = 0;
		//TODO
		for (int i = Cfg.RepeatFromIndex; i <= Cfg.RepeatToIndex; ++i)
		{
			timeSum += Cfg.Intervals[i].Duration();
		}

		timeSum *= Cfg.RepetitionCount;


		for (int i = 0; i < Cfg.RepeatFromIndex; ++i) 
		{
			timeSum += Cfg.Intervals[i].Duration();
		}


		for (int i = Cfg.RepeatToIndex + 1; i < Cfg.Intervals.Count; ++i) 
		{
			timeSum += Cfg.Intervals[i].Duration();
		}



		return timeSum;
	}


	public void PrintIntervals()
	{

		for (int i = 0; i < Cfg.Intervals.Count; ++i)
		{
			core.dbg.Dbg.Log("Interval: " + i + " " + Cfg.Intervals[i].hours.ToString() + " : " +  Cfg.Intervals[i].minutes.ToString() + " : " + Cfg.Intervals[i].seconds.ToString());
		}

	}


	public Config CreateNewConfig()
	{
		Config config = newConfig.Clone ();

		config.Order = Presets.Count;

		return config;
	}


	public string GetActualTime(bool mode24)
	{
		DateTime dateTime = System.DateTime.Now;

		if (mode24)
			return (dateTime.Hour < 10 ? "0" : "") + dateTime.Hour+":"+(dateTime.Minute < 10 ? "0" : "") + dateTime.Minute;
		else
		{
			return dateTime.ToString("hh:mm tt");
		}
	}


}
