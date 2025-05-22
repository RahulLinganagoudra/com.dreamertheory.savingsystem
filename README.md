# EntitySaver - Unity Save System Component

The `EntitySaver` component is part of a lightweight Unity saving system. It enables any GameObject to serialize and deserialize its state using attached `ISaveable` components. Each entity is uniquely identified with a persistent `EntityID`.
A lightweight save/load system for Unity that allows objects to register as saveable entities with unique persistent IDs.

[![Unity](https://img.shields.io/badge/Unity-2020%2B-white?logo=unity&labelColor=black)](https://unity.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![GitHub Repo stars](https://img.shields.io/github/stars/RahulLinganagoudra/com.dreamertheory.savingsystem)](https://github.com/RahulLinganagoudra/com.dreamertheory.savingsystem))
[![Made With ❤️](https://img.shields.io/badge/made%20with-%E2%9D%A4-red)](https://github.com/RahulLinganagoudra)



---

## ✨ Features

- Assigns a unique GUID to each entity.
- Automatically gathers all `ISaveable` components on the GameObject.
- Supports saving and loading of multiple components' data.
- Editor integration with a "Generate ID" button.

---

## 🔧 Installation

#### Steps
1. Open Unity.
2. Go to Window → Package Manager.
3. Click the + button (top-left).
4. Choose “Add package from Git URL…”.
5. Paste the following URL:
``` bash :
https://github.com/RahulLinganagoudra/com.dreamertheory.savingsystem.git
```
---

### Usage
This saving system uses an adapter pattern to allow switching between local, cloud, or other saving strategies easily.
``` text :
     ┌────────────────────┐
     │    GameObject      │
     │  (with components) │
     └────────┬───────────┘
              │
              ▼
     ┌────────────────────┐
     │   EntitySaver.cs   │ ◄────── Generates and holds unique EntityID
     └────────┬───────────┘
              │
              ▼
     ┌────────────────────┐
     │   ISaveable[]      │ ◄────── All attached ISaveable components
     └────────┬───────────┘
              │
              ▼
     ┌────────────────────────────┐
     │     Save / Load Request    │
     └────────┬──────────┬────────┘
              │          │
      ┌───────▼───┐  ┌───▼────────┐
      │ LocalSave │  │ CloudSave  │   ◄──── Pluggable Adapters (via ISaveSystem)
      │  System   │  │  System    │
      └───────────┘  └────────────┘
              │
              ▼
     ┌────────────────────────────┐
     │  FileSystem / Server API   │
     └────────────────────────────┘

```
1. Implement the ISaveable Interface 
	Each component that needs to be saved must implement:
	
	``` cs :
	public interface ISaveable
	{
	    string Save();
	    void Load(string data);
	}
	```
	Add this to any script whose state you want saved.
2. Add the EntitySaver Component
	Attach the EntitySaver component to any GameObject. It will automatically:
	- Assign a unique EntityID.
	
	- Find and cache all ISaveable components.
	
	- Delegate save/load operations to each of them.

3. Saving & Loading with a Save System Adapter
	Instead of directly saving data, your logic should go through an ISaveSystem:
	``` cs :
	public interface ISaveSystem
	{
	    void SaveData(string key, string value, Action<bool> callback);
	    void LoadData(string key, Action<string> callback);
	}
	```
	You can Use Local saving system included in the package
	``` cs :
 	public class LocalSaveSystem : MonoBehaviour, ISaveSystem
	{
	    // Saves to Application.persistentDataPath/key.json
	    public void SaveData(string key, string value, Action<bool> callback) { /* ... */ }
	
	    // Loads from key.json if exists
	    public void LoadData(string key, Action<string> callback) { /* ... */ }
	}
	```
 	which saves data locally at:
	``` shell :
	%APPDATA%/../LocalLow/<Company>/<Project>/key.json
	```

### Prefab Setup Example
You can use EntitySaver on any prefab that needs to be saved/loaded. Here's a common use-case:

- Player

  - EntitySaver component
  
  - PlayerStats (implements ISaveable)
  
  - Inventory (implements ISaveable)

Each component manages its own data, while EntitySaver coordinates saving/loading.

### Example
``` cs :
public class PlayerStats : MonoBehaviour, ISaveable
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
	}
	
	string ISaveable.Save()
	{
		return new BigData() { num1 = num1, num2 = num2, name = "" }.SetData();
	}
}
```
`SetData()` and `GetData()` are the functions provided in the package that will convert the string to whatever the datatype you want
Attach EntitySaver and PlayerStats to the same GameObject.

### Saving and Loading
Use the Save() and Load() methods of EntitySaver to serialize or deserialize:
``` cs :
SavingSystem.Instance.Save();
SavingSystem.Instance.Load();
```
### Editor Integration
The editor script adds a "Generate ID" button in the Inspector for the EntitySaver component.
- Use this button to assign a unique ID to the GameObject.
  
  ![](https://github.com/RahulLinganagoudra/MediaResources/blob/main/Screenshot%202025-05-15%20174244.png)
  ![](https://github.com/RahulLinganagoudra/MediaResources/blob/main/Screenshot%202025-05-15%20174250.png)
  
- Required if you want to reference this object persistently across sessions.
### Notes
The EntityID is serialized and can be generated via the button or programmatically.

Do not modify EntityID at runtime unless you know what you're doing — it may break save data consistency.
