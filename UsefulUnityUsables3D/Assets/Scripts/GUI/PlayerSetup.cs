using UnityEngine;
using System.Collections;

public class PlayerSetup : MonoBehaviour
{
	public CharacterSpawner spawner;
		
	// Update is called once per frame
	void Update ()
	{
		if(InControl.InputManager.ActiveDevice.MenuWasPressed)
		{
			spawner.SpawnPlayer(0, InControl.InputManager.ActiveDevice);
		}
	}
}
