using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicDb : MonoBehaviour {

	public enum EffectCategoryType
	{
		Tick,
		TickLong,
		TimerFinish
	}


	[System.Serializable]
	public class MusicGroup
	{
		public string name;
		//public List<AudioClip> ClipList;
		public List<string> ClipNameList;
	}


	[System.Serializable]
	public class EffectCategory
	{
		public EffectCategoryType Type;
		public List<AudioClip> ClipList;
	}


	[SerializeField]
	public List<MusicGroup> MusicGroups;

	[SerializeField]
	public List<EffectCategory> EffectClips;


	private void Start()
	{

	}

	public MusicGroup GetMusicGroup(string groupName)
	{
		MusicGroup category = MusicGroups.Find(x=>x.name == groupName);

		return category;
	}


	public AudioClip GetEffect(EffectCategoryType type)
	{
		EffectCategory category = EffectClips.Find(x=>x.Type == type);

		return category.ClipList[0];
	}



}
