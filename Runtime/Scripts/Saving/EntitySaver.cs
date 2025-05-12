using UnityEngine;
using System;
namespace SavingSystem
{

	public class EntitySaver : MonoBehaviour
	{
		[Serializable]
		public class DataFile
		{
			public string id = "";
			public string[] data;
		}

		[SerializeField] public string EntityID { get; private set; } = "";

		ISaveable[] saveables;

		private void Awake()
		{
			saveables ??= GetComponents<ISaveable>();
		}
		[ContextMenu("Generate ID")]
		private void GenerateID() => EntityID = Guid.NewGuid().ToString();
		internal DataFile Save()
		{
			DataFile data = new();
			data.id = EntityID;
			int i = 0;
			saveables ??= GetComponents<ISaveable>();
			data.data = new string[saveables.Length];

			foreach (ISaveable saveable in saveables)
			{
				data.data[i] = saveable.Save();
				i++;
			}
			return data;
		}
		internal void Load(DataFile data)
		{
			//EntityID = data.id;
			int i = 0;
			saveables ??= GetComponents<ISaveable>();
			foreach (var compenent in saveables)
			{
				compenent.Load(data.data[i++]);
			}
		}
	}

}
