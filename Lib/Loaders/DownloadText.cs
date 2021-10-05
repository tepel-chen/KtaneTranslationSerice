#nullable enable

using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/* Mostly copied from Twitch Plays KTANE code by samfundev */

namespace TranslationService.Loaders
{

	public class DownloadText : CustomYieldInstruction
	{
		UnityWebRequest request;
		UnityWebRequestAsyncOperation asyncOperation;
		string? settingsPath;
		int retryCount;
		private string? backupText;

		private static readonly object settingsFileLock = new object();

		public DownloadText(string url, string? backup = null)
		{
			request = UnityWebRequest.Get(url);
			Debug.Log($"Fetching from {request.url}");
			asyncOperation = request.SendWebRequest();

			if(backup != null)
            {
				var settingsFolder = Path.Combine(Application.persistentDataPath, "TranslationSettings");
				if (!Directory.Exists(settingsFolder))
					Directory.CreateDirectory(settingsFolder);

				settingsPath = Path.Combine(settingsFolder, backup + ".txt");
			}
		}

		bool downloadSuccess => !request.isNetworkError && !request.isHttpError;
		bool success => downloadSuccess || backupText != null;


		public override bool keepWaiting
		{
			get
			{
				if (!asyncOperation.isDone)
					return true;

				if (!downloadSuccess && retryCount < 5)
				{
					retryCount++;
					Debug.Log($"Fetching from {request.url} (Count {retryCount})");
					request = UnityWebRequest.Get(request.url);
					asyncOperation = request.SendWebRequest();
					return true;
				}
				if (!downloadSuccess && settingsPath != null)
				{
					try
					{
						lock (settingsFileLock)
						{
							Debug.Log($"Failed to fetch from {request.url}, reading backup from {settingsPath}.");
							if (!File.Exists(settingsPath))
							{
								Debug.Log($"Failed to read {settingsPath}");
								return false;
							}
							backupText = File.ReadAllText(settingsPath);
							return false;
						}
					}
					catch (Exception e)
					{
						Debug.Log($"Failed to read {settingsPath}");
						Debug.LogException(e);
					}


				} else if (settingsPath != null)
                {
					lock(settingsFileLock)
                    {
                        try
                        {
							File.WriteAllText(settingsPath, request.downloadHandler.text);
                        } catch (Exception e)
                        {
							Debug.Log($"Failed to write to {settingsPath}");
							Debug.LogException(e);
                        }
                    }
                }

				return false;
			}
		}

		public string? Text => downloadSuccess ? request.downloadHandler.text : success ? backupText : null;
	}
}
