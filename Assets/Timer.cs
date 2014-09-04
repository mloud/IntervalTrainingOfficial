using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

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
	public delegate void IntervalTickDelegate(bool last);

	[System.Serializable]
	public class IntervalDefinition
	{
		public int hours;
		public int minutes;
		public int seconds;
	
		public string Name;
		public float MusicVolume;
		public Col.ColorType ColorType;
		public string InfoText;
		public float Duration() { return hours * 3600 + minutes * 60 + seconds; }
	}



	[System.Serializable]
	public class Config
	{
		public List<IntervalDefinition> Intervals;

		public int RepetitionCount;
		public int NoticeTicks;
		public int RepeatFromIndex;
		public int Order;

		public string Name;
		public string MusicName;
		public bool LenghtFromMusic;
		public bool LoopMusic;
		public bool Enabled;
	
		public void SetFrom(Config config)
		{
			RepetitionCount = config.RepetitionCount;
			NoticeTicks = config.NoticeTicks;
			MusicName = config.MusicName;
			LenghtFromMusic = config.LenghtFromMusic;
			LoopMusic = config.LoopMusic;
		
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


	private float StartTime { get; set; }
	private float EndTime { get; set; }

	public TimerStartedDelegate TimerStarted { get; set; }
	public TimerEndedDelegate TimerEnded { get; set; }
	public TimerPauseDelegate TimerPause { get; set; }
	public TimerUnPauseDelegate TimerUnPause { get; set; }
	public TimerResetDelegate TimerReset { get; set; }
	public IntervalEndedDelegate IntervalEnded { get; set; }
	public IntervalStartedDelegate IntervalStarted { get; set; }
	public IntervalTickDelegate IntervalTick { get; set; }


	private StringBuilder StringBuilder { get; set; }


	public class IntervalState
	{
		public float StartTime { get { return _startTime; } set { _startTime = value; }}
		public float EndTime { get { return _endTime; } set {_endTime = value; ReInit(); }}
	
		public List<float> NoticeTicks { get; set; }

		public int Index { get; set; }
		public int Round { get; set; }

		private float _startTime;
		private float _endTime;

		public IntervalState(int noticeTicksCount)
		{
			NoticeTicks = new List<float>(noticeTicksCount);
			for (int i = 0; i < noticeTicksCount; ++i)
			{
				NoticeTicks.Add(0);
			}
		}

		public void ReInit()
		{
			for ( int i = 0; i < NoticeTicks.Count; ++i)
			{
				NoticeTicks[i] = EndTime - (NoticeTicks.Count - i - 1);
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

	}


	void Update () 
	{
		if (CurrentState == State.Running)
		{
			float timeNow = Time.time;

			// Check for end
			if (timeNow > EndTime)
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
					if (noticeTime > 0 && Time.time > noticeTime)
					{
						CurrentIntervalState.NoticeTicks[i] = -1;
						
						if (IntervalTick != null)
						{
							IntervalTick( i == (CurrentIntervalState.NoticeTicks.Count - 1));
						}
					}
				}
				
				
				// Check for interval end
				if (timeNow > CurrentIntervalState.EndTime)
				{
					if (IntervalEnded != null)
					{
						IntervalEnded();
					}

					SetNextInterval();

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
	public void Run()
	{
		Time.timeScale = 1;

		CurrentState = State.Running;


		StartTime = Time.time;
		EndTime = StartTime + Duration();

	
		CurrentIntervalState.Index = 0;
		CurrentIntervalState.StartTime = StartTime;
		CurrentIntervalState.EndTime = StartTime + Cfg.Intervals[CurrentIntervalState.Index].Duration();
		CurrentIntervalState.Round = 0;

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

		StartTime = -1;
		CurrentIntervalState.Index = 0;

		if (TimerReset != null)
		{
			TimerReset();
		}
	}

	private void SetNextInterval()
	{
		CurrentIntervalState.Index++;
		if (CurrentIntervalState.Index == Cfg.Intervals.Count)
		{
			CurrentIntervalState.Index = Cfg.RepeatFromIndex;
		}

		CurrentIntervalState.StartTime = CurrentIntervalState.EndTime;
		CurrentIntervalState.EndTime = CurrentIntervalState.StartTime + Cfg.Intervals[CurrentIntervalState.Index].Duration();
	
		CurrentIntervalState.Round++;
	}
	
	public float ElapsedTime()
	{
		float result = 0;

		if (CurrentState == State.Stopped)
		{
			result = 0;
		}
		else
		{
			result = Mathf.Min (Duration(), Time.time - StartTime);
		}

		return result;
	}

	public float RestTime()
	{
		return Duration() - ElapsedTime();
	}

	public string RestTimeFormatted(bool useMillis)
	{
		return GetFormattedTime(RestTime(), useMillis);
	}


	public string ElapsedTimeFormatted(bool useMillis)
	{
		return GetFormattedTime(ElapsedTime(), useMillis);
	}

	public string DurationFormatted(bool useMill)
	{
		return GetFormattedTime(Duration(), useMill);
	}


	public float CurrentIntervalEndTime()
	{
		return CurrentIntervalState.EndTime;
	}

	public float CurrentIntervalDuration()
	{
		return CurrentState == State.Stopped ? Cfg.Intervals [CurrentIntervalState.Index].Duration () : CurrentIntervalState.EndTime - CurrentIntervalState.StartTime;
	}

	public string GetFormattedTime(float timeSec, bool useMillis = true, bool showZeroHours = false, bool showZeroMinutes = false)
	{
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
		return  CurrentState == State.Stopped ? 0 : Time.time - CurrentIntervalState.StartTime;
	}

	public float CurrentIntervalRestTime()
	{
		return  CurrentState == State.Stopped ? CurrentIntervalDuration() : CurrentIntervalEndTime() - Time.time;
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
		return CurrentIntervalState.Round / 2 + 1;
	}



	public void Pause()
	{
		CurrentState = State.Paused;
		Time.timeScale = 0;

		if (TimerPause != null)
		{
			TimerPause(Cfg.Intervals[CurrentIntervalState.Index]);
		}

	}

	public void UnPause()
	{
		CurrentState = State.Running;
		Time.timeScale = 1;


		if (TimerUnPause != null)
		{
			TimerUnPause(Cfg.Intervals[CurrentIntervalState.Index]);
		}
	}


	public float Duration()
	{
		float timeSum = 0;
		//TODO
		for (int i = Cfg.RepeatFromIndex; i < Cfg.Intervals.Count; ++i)
		{
			timeSum += Cfg.Intervals[i].Duration();
		}

		timeSum *= Cfg.RepetitionCount;


		for (int i = 0; i < Cfg.RepeatFromIndex; ++i) 
		{
			timeSum += Cfg.Intervals[i].Duration();
		}

		return timeSum;
	}


	public void PrintIntervals()
	{

		for (int i = 0; i < Cfg.Intervals.Count; ++i)
		{
			Debug.Log("Interval: " + i + " " + Cfg.Intervals[i].hours.ToString() + " : " +  Cfg.Intervals[i].minutes.ToString() + " : " + Cfg.Intervals[i].seconds.ToString());
		}

	}


}
