using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;



public class JsonManager
{
    public class FileName
    {
        public const string TestSaveFile = "TestSaveFile";
    }

    private string _path = "";
    public string PATH => _path;

    private string _pathFileName = "/SAVE_DATA_FILE";

    public void Init()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            _path = Application.persistentDataPath + _pathFileName;
        }
        else
        {
            _path = Application.dataPath + _pathFileName;
        }
        Debug.Log(_path);

        if (!Directory.Exists(_path))
            Directory.CreateDirectory(_path);
    }

    public void SaveJson<T>(string createPath, string fileName, T value)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        string json = JsonUtility.ToJson(value, true);
        byte[] data = Encoding.UTF8.GetBytes(json);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public void SaveJson<T>(string fileName, T value)
    {
        SaveJson<T>(_path, fileName, value);
    }

    public T LoadJsonFile<T>(string loadPath, string fileName) where T : new()
    {
        if (File.Exists(string.Format("{0}/{1}.json", loadPath, fileName)))
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }
        SaveJson<T>(loadPath, fileName, new T());
        return LoadJsonFile<T>(loadPath, fileName);
    }

    public T LoadJsonFile<T>(string fileName) where T : new()
    {
        return LoadJsonFile<T>(_path, fileName);
    }

    public bool DeleteFile(string path, string fileName)
    {
        if (File.Exists(string.Format("{0}/{1}.json", path, fileName)))
        {
            File.Delete(string.Format("{0}/{1}.json", path, fileName));
            return true;
        }

        return false;
    }

    public bool DeleteFile(string fileName)
    {
        return DeleteFile(_path, fileName);
    }
}
