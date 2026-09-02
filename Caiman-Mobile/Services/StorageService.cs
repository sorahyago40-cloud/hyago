namespace Caiman_Panel;

public interface IStorageService
{
	void Set(string key, string value);
	string? Get(string key);
	void Remove(string key);
	void Clear();
	bool Exists(string key);
}

public class StorageService : IStorageService
{
	public void Set(string key, string value)
	{
		try
		{
			SecureStorage.SetAsync(key, value).GetAwaiter().GetResult();
		}
		catch
		{
			Preferences.Set(key, value);
		}
	}

	public string? Get(string key)
	{
		try
		{
			return SecureStorage.GetAsync(key).GetAwaiter().GetResult();
		}
		catch
		{
			return Preferences.Get(key, null as string);
		}
	}

	public void Remove(string key)
	{
		try
		{
			SecureStorage.Remove(key);
		}
		catch { }

		Preferences.Remove(key);
	}

	public void Clear()
	{
		Preferences.Clear();
	}

	public bool Exists(string key)
	{
		return Preferences.ContainsKey(key);
	}
}
