using Newtonsoft.Json;

namespace SavingSystem
{
	public interface ISaveable
	{
		string Save();
		void Load(string data);

	}
	public static class JsonExt
	{
		public static T GetData<T>(this string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
		public static string SetData(this object json)
		{
			return JsonConvert.SerializeObject(json);
		}
	}
}