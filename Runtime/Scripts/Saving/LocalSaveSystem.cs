using System;
using System.IO;
using UnityEngine;

namespace SavingSystem.Core
{
	public class LocalSaveSystem : MonoBehaviour, ISaveSystem
	{
		private string GetFilePath(string key) => Path.Combine(Application.persistentDataPath, key + ".json");

		public void SaveData(string key, string value, Action<bool> callback)
		{
			try
			{
				File.WriteAllText(GetFilePath(key), value);
				callback?.Invoke(true);
			}
			catch (Exception e)
			{
				Debug.LogError("Local Save Error: " + e.Message);
				callback?.Invoke(false);
			}
		}

		public void LoadData(string key, Action<string> callback)
		{
			try
			{
				string filePath = GetFilePath(key);
				if (File.Exists(filePath))
				{
					string data = File.ReadAllText(filePath);
					callback?.Invoke(data);
				}
				else
				{
					callback?.Invoke(null);
				}
			}
			catch (Exception e)
			{
				Debug.LogError("Local Load Error: " + e.Message);
				callback?.Invoke(null);
			}
		}
	}

}


