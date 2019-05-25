using System;
using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

namespace Assets.Scripts
{
	public class DataService
	{

		private SQLiteConnection connection;
		private string databaseName;

		public DataService(string databaseName)
		{
			this.databaseName = databaseName;

#if UNITY_EDITOR
			var dbPath = string.Format(@"Assets/StreamingAssets/{0}", databaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, databaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb =
 new WWW("jar:file://" + Application.dataPath + "!/assets/" + databaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb =
 Application.dataPath + "/Raw/" + databaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb =
 Application.dataPath + "/StreamingAssets/" + databaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb =
 Application.dataPath + "/StreamingAssets/" + databaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb =
 Application.dataPath + "/Resources/Data/StreamingAssets/" + databaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb =
 Application.dataPath + "/StreamingAssets/" + databaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
			connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
			Debug.Log("Final PATH: " + dbPath);

			
			//TODO: clear table before deploying for the first time (with code or with hands?)
			try
			{
				foreach (var value in GetRecords())
				{}
			}
			catch (Exception e)
			{
				connection.CreateTable<Record>();
			}
		}

		public IEnumerable<Record> GetRecords()
		{
			return connection.Table<Record>();
		}

		public IEnumerable<Record> GetUserRecords(String login)
		{
			return connection.Table<Record>().Where(x => x.Login == login);
		}

		public void AddRecord(Record record)
		{
			connection.Insert(record);
		}

		public void ClearTable()
		{
			connection.DeleteAll<Record>();
		}
	}
}