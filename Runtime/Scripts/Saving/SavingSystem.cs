using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace SavingSystem.Core
{
	public class SavingSystem : MonoBehaviour
	{
		public static SavingSystem instance;

		class Data
		{
			public EntitySaver.DataFile[] entityData;
		}
		[SerializeField, Header("Saving System Adapter"), NotNull] ISaveSystem saveSystem;
		[BeginGroup("AutoSave"), SerializeField, LeftToggle, Space]
		private bool UseAutoSave = false;

		[SerializeField, Tooltip("Saving interval in seconds"), EndGroup]
		[Space]
		[EditorButton(nameof(Load))]
		[EditorButton(nameof(Save))]
		private int savingInterval = 120;
		private float timeDelta;
		private readonly string encryptionKey = "@#123!";


		public UnityEvent OnSave = new();


		string JsonPathEditor => Path.Combine(Application.dataPath, "GameSaves", "Save.json");
		string JsonPathRuntime => Path.Combine(Application.persistentDataPath, "Save.data");

		private void Awake()
		{
			instance = this;
			saveSystem = GetComponent<ISaveSystem>();
#if UNITY_EDITOR
			LoadFromEditor();
#else
            LoadFromRuntime();
#endif
		}

		private void Update()
		{
			if (!UseAutoSave) return;
			timeDelta += Time.unscaledDeltaTime;
			if (timeDelta > savingInterval)
			{
				timeDelta = 0;
				Save();
				OnSave?.Invoke();
			}
		}

		/// <summary>
		/// Save the current state to the appropriate location (editor/runtime).
		/// </summary>
		public void Save()
		{
			var state = CaptureState();

#if UNITY_EDITOR
			SaveToEditor(state);
#else
            SaveToRuntime(state);
#endif
			OnSave?.Invoke();
		}

		/// <summary>
		/// Load the saved state from the appropriate location (editor/runtime).
		/// </summary>
		public void Load()
		{
#if UNITY_EDITOR
			LoadFromEditor();
#else
            LoadFromRuntime();
#endif
		}

		// PRIVATE

		private void SaveToEditor(Data data)
		{
			string jsonData = JsonUtility.ToJson(data, true);
			File.WriteAllText(JsonPathEditor, jsonData);
		}

		private void LoadFromEditor()
		{
			if (!File.Exists(JsonPathEditor))
			{
				Debug.LogWarning("Save file not found in editor path. Creating a new one.");
				SaveToEditor(CaptureState());
			}

			string jsonData = File.ReadAllText(JsonPathEditor);
			var data = JsonUtility.FromJson<Data>(jsonData);
			RestoreState(data);
		}

		private void SaveToRuntime(Data data)
		{
			string jsonData = JsonUtility.ToJson(data);
			string cipherData = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonData));
			//this is suppose to be the local save
			//File.WriteAllText(JsonPathRuntime, cipherData);
			saveSystem.SaveData("savedata", cipherData, null);
			PlayerPrefs.Save();
		}

		private void LoadFromRuntime()
		{
			if (!File.Exists(JsonPathRuntime))
			{
				Debug.LogWarning("No runtime save data found. Creating a new one.");
				SaveToRuntime(CaptureState());
			}
			saveSystem.LoadData(JsonPathRuntime, (obfuscatedData) =>
			{
				string jsonData = Encoding.UTF8.GetString(Convert.FromBase64String(obfuscatedData));
				var data = JsonUtility.FromJson<Data>(jsonData);
				RestoreState(data);
			});
		}

		private Data CaptureState()
		{
			var saveables = FindObjectsByType<EntitySaver>(FindObjectsSortMode.None);
			Data data = new Data
			{
				entityData = new EntitySaver.DataFile[saveables.Length]
			};

			for (int i = 0; i < saveables.Length; i++)
			{
				data.entityData[i] = saveables[i].Save();
			}

			return data;
		}

		private void RestoreState(Data data)
		{
			foreach (EntitySaver saveable in FindObjectsByType<EntitySaver>(FindObjectsSortMode.None))
			{
				string id = saveable.EntityID;
				foreach (var entityData in data.entityData)
				{
					if (id == entityData.id)
					{
						saveable.Load(entityData);
						break;
					}
				}
			}
		}
	}
	public interface ISaveSystem
	{
		void SaveData(string key, string value, System.Action<bool> callback);
		void LoadData(string key, System.Action<string> callback);
	}

}


