﻿using System;
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
				string path = GetFilePath(key);
				string directory = Path.GetDirectoryName(path);
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
		
				File.WriteAllText(path, value);
				callback?.Invoke(true);
			}
			catch (Exception e)
			{
#if UNITY_EDITOR
				Debug.LogError("Local Save Error: " + e.Message);
#endif		
		  		callback?.Invoke(false);
			}
		}


		public void LoadData(string key, Action<string> callback)
		{
			try
			{
   
				string filePath = GetFilePath(key);
    
				if (!Directory.Exists(Path.GetDirectoryName(filePath)))
				{
					callback?.Invoke(null);
					return;
				}

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
#if UNITY_EDITOR
				Debug.LogError("Local Load Error: " + e.Message);
#endif		
				callback?.Invoke(null);
			}
		}
	}

}


