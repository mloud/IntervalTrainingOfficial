using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MusicController : MonoBehaviour {

	[SerializeField]
	Text txtCurrentMusicTime;

	[SerializeField]
	Text txtCurrentMusicLength;

	[SerializeField]
	Text txtMusicTimeLengthDelimiter;



	[SerializeField]
	ProgressBar barMusicProgress;

	[SerializeField]
	Button btnPlay;

	[SerializeField]
	Button btnRewindForw;

	[SerializeField]
	Button btnRewindBack;

	[SerializeField]
	List<Button> musicButtonsList;



	void Init()
	{

	}


	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		IAudioManager audioManager = Application.Instance.AudioManager;
		if (audioManager.IsMusicSet())
		{
			float songTime = audioManager.GetSongTime();
			float songLength = audioManager.GetSongLength();

			barMusicProgress.Set(songTime / songLength);


			txtCurrentMusicTime.gameObject.SetActive(true);
			txtCurrentMusicLength.gameObject.SetActive(true);
			txtMusicTimeLengthDelimiter.gameObject.SetActive(true);

			txtCurrentMusicTime.text = Application.Instance.Timer.GetFormattedTime(songTime,false, true, true);
			txtCurrentMusicLength.text =  Application.Instance.Timer.GetFormattedTime(songLength, false, true, true);
		}
		else
		{
			barMusicProgress.Set(0);
			txtCurrentMusicTime.gameObject.SetActive(false);
			txtCurrentMusicLength.gameObject.SetActive(false);
			txtMusicTimeLengthDelimiter.gameObject.SetActive(false);

		}
	}


	public void OnButtonClick(Button button)
	{
		IAudioManager audioManager = Application.Instance.AudioManager;

		if (button == btnPlay)
		{
			if (audioManager.IsPlaying())
				audioManager.Pause();
			else 
				audioManager.Play();
		}
		else if (button == btnRewindForw)
		{
			audioManager.RewindSong(+10);
		}
		else if (button == btnRewindBack)
		{
			audioManager.RewindSong(-10);
		}

	}

	public void OnMusicButtonClick(Button button)
	{
		if (button.name == "btnAlbum1")
		{
			Application.Instance.AudioManager.SetMusic(Application.Instance.MusicDb.GetMusicGroup("group0"));
		}
		else if (button.name == "btnAlbum2")
		{
			Application.Instance.AudioManager.SetMusic(Application.Instance.MusicDb.GetMusicGroup("group1"));
		}
	
	}



	public void OnMusicTimeChange(Slider slider)
	{
//		//AudioManager audioManager = Application.Instance.AudioManager;
//
//		if (audioManager.IsSongSelected())
//		{
//			audioManager.SetSongTime(slider.value);
//		}
	}
}
