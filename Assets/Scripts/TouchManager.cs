using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputInfo
{
	public int Id;
	public float PosX;
	public float PosY;
	public TouchPhase Phase;

	public InputInfo (int id, float posX, float posY, TouchPhase phase)
	{
		Id = id;
		PosX = posX;
		PosY = posY;
		Phase = phase;
	}
}


public class TouchManager : MonoBehaviour
{



	List<InputInfo> GetInput ()
	{
		List<InputInfo> result = new List<InputInfo> ();

		#if UNITY_EDITOR
		if (Input.GetMouseButton (0))
		{
			//Check is inside collider

			if (AudioElements.Instance.CheckExists (0))
			{
				result.Add (new InputInfo (0, Input.mousePosition.x, Input.mousePosition.y, TouchPhase.Moved));
			}
			else
			{
				result.Add (new InputInfo (0, Input.mousePosition.x, Input.mousePosition.y, TouchPhase.Began));
			}
		}
		else if(AudioElements.Instance.CheckExists (0))
		{
			result.Add (new InputInfo (0, Input.mousePosition.x, Input.mousePosition.y, TouchPhase.Ended));

		}
		#endif

		for (int i = 0; i < Input.touches.Length; i++)
		{
			Touch input = Input.touches [i];
			result.Add (new InputInfo (input.fingerId, input.position.x, input.position.y, input.phase));
		}	

		return result;
	}

	// Update is called once per frame
	void Update ()
	{
		//Get all input
		List<InputInfo> inputs = GetInput ();

		//Manage them
		ManageInputs (inputs);
	}

	void ManageInputs (List <InputInfo> inputs)
	{
		for (int i = 0; i < inputs.Count; i++)
		{
			InputInfo iInfo = inputs [i];
			if (iInfo.Phase == TouchPhase.Ended)
			{
				AudioElements.Instance.SafeRemoveAudioSource (iInfo.Id);
			}
			else if (iInfo.Phase == TouchPhase.Began)
			{
				AudioElements.Instance.CreateAudioSource (iInfo.Id, iInfo.PosX, iInfo.PosY);
			}
			else if (iInfo.Phase == TouchPhase.Moved)
			{
				AudioElements.Instance.ModifyAudioSource (iInfo.Id, iInfo.PosX, iInfo.PosY);
			}
		}
	}
}
