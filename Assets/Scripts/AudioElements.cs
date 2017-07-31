using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioElements : MonoBehaviour
{

	static AudioElements instance;

	public static AudioElements Instance { get { return instance; } }

	Dictionary<int, AudioSource> idTouchToAudioDic = new Dictionary<int, AudioSource> ();


	public GameObject FingerAudioPrefab;

	void Awake ()
	{
		instance = this;
	}

	public bool CheckExists (int id)
	{
		return idTouchToAudioDic.ContainsKey (id);
	}

	public void SafeRemoveAudioSource (int id)
	{
		if (CheckExists (id))
		{
			Destroy (idTouchToAudioDic [id].gameObject);
			idTouchToAudioDic.Remove (id);
		}
	}

	public void CreateAudioSource (int id, float posX, float posY)
	{
		if (!CheckExists (id))
		{
			GameObject obj = GameObject.Instantiate (FingerAudioPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			idTouchToAudioDic.Add (id, obj.GetComponent<AudioSource> ());
		}

		ModifyAudioSource (id, posX, posY);
	}

	public void ModifyAudioSource (int id, float posX, float posY)
	{
		float formalizedX = 3 * (posX / (float)Screen.width);  //Mathf.Clamp(  100 * (posX - 0.53f * Screen.width) / (float)Screen.width, -30,70);
		float posXToPitch = formalizedX;//Mathf.Pow(1.05946f,formalizedX);
		float posYToVolume = Mathf.Clamp(posY / (float)Screen.height,0,1);

		idTouchToAudioDic [id].pitch = posXToPitch;
		idTouchToAudioDic [id].volume = posYToVolume;
	}
}
