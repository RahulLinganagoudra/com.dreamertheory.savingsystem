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

## 🚀 Usage

1. Add the `EntitySaver` component to any GameObject.
2. Ensure your component implements the `ISaveable` interface:
``` cs : 
   public interface ISaveable
   {
       string Save();
       void Load(string data);
   }
```
3. Each ISaveable component on the GameObject will be included in save/load operations.

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
  ![Inspector Screenshot](https://your-image-url.com/editor-button.png)
- Required if you want to reference this object persistently across sessions.
### Notes
The EntityID is serialized and can be generated via the button or programmatically.

Do not modify EntityID at runtime unless you know what you're doing — it may break save data consistency.
