using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

namespace goshdarngames.Character
{
	[System.Serializable]
	public class PlatformCharacter
	{
		public PlatformMovement movement;
		public goshdarngames.InputManager.CharacterController controller;
		public Transform spawnPoint;
		
		public void Spawn(InputDevice device)
		{
			controller.transform.position = spawnPoint.position;	
			controller.character = movement;
			Enable(device);
		}
		
		public void Enable(InputDevice device)
		{
			controller.inputDevice = device;
			controller.gameObject.SetActive(true);
		}
		
		public void Disable()
		{
			controller.gameObject.SetActive(false);
		}
	}
	
	public class PlatformCharacterSpawner : CharacterSpawner
	{
		public List<PlatformCharacter> characterPool = new List<PlatformCharacter>();
		
		void Start()
		{
			foreach(PlatformCharacter chr in characterPool)
			{
				chr.Disable();
			}
		}
		
		public override float MaxPlayers
		{
			get { return characterPool.Count; }
		}
		
		public override ICharacter GetPlayer(int index)
		{
			return characterPool[index] as ICharacter;
		}
		
		public override void SpawnPlayer(int index, InputDevice device)
		{
			characterPool[index].Spawn(device);
		}
		
		public override void Reset()
		{
			foreach(PlatformCharacter chr in characterPool)
			{
				chr.Disable();
			}
		}
		
	}
	
}