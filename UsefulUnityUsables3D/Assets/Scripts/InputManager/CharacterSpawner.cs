﻿using UnityEngine;
using System.Collections;
using goshdarngames.Character;
using InControl;

public class CharacterSpawner : MonoBehaviour
{	
	public virtual float MaxPlayers
	{
		get {return 0;}
	}
	
	public virtual ICharacter GetPlayer(int index)
	{
		return null;
	}
	
	public virtual void SpawnPlayer(int index, InputDevice device)
	{
	}
	
	public virtual void Reset()
	{
	}
}
