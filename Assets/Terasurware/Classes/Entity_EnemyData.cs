using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_EnemyData : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public int index;
		public string name;
		public int hp;
		public int speed;
		public int attack;
		public int skillnum;
	}
}

