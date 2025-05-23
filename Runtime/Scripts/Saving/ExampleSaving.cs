﻿using UnityEngine;
namespace SavingSystem
{
	public class ExampleSaving : MonoBehaviour, ISaveable
	{
		[System.Serializable]
		struct BigData
		{
			public int num1;
			public float num2;
			public string name;
		}
		[SerializeField]
		int num1 = 0;
		[SerializeField]
		float num2 = 2;

		void ISaveable.Load(string data)
		{
			BigData save = data.GetData<BigData>();
			num1 = save.num1;
			num2 = save.num2;
#if UNITY_EDITOR
			Debug.Log($"Got from save!\nnum 1 = {save.num1}\nnum 2 = {save.num2}");
#endif
		}

		string ISaveable.Save()
		{
			return new BigData() { num1 = num1, num2 = num2, name = "" }.SetData();
		}
  		[ContextMenu("Change Number")]
		private void ChangeNumbers()
		{
			num1 = Random.Range(0, 100);
			num2 = Random.Range(0, 100f);
		}
	}

}
