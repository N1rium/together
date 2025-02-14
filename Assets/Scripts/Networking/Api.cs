using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public static class Api
    {
        private static string BaseProdUrl = "https://game-backend-xi.vercel.app";
        private static string BaseDevUrl = "http://localhost:3000";

        private static string BaseUrl = BaseProdUrl;
    
        public static async Task<Response<T>> Get<T>(string url)
        {
            using var webRequest = UnityWebRequest.Get($"{BaseUrl}{url}");
    
            var operation = webRequest.SendWebRequest();
            var tcs = new TaskCompletionSource<bool>();

            operation.completed += _ => tcs.SetResult(true);

            // Await the completion of the operation
            await tcs.Task;

            // Check if there were any errors
            if (webRequest.result != UnityWebRequest.Result.ConnectionError &&
                webRequest.result != UnityWebRequest.Result.ProtocolError)
            {
                var json = JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
                return new()
                {
                    data = json,
                    statusCode = webRequest.responseCode, 
                };
            }

            Debug.LogError("Error: " + webRequest.error);
            return new()
            {
                statusCode = webRequest.responseCode,
            };
        }
    
        public static async Task<T> Post<T>(string url, string jsonData)
        {
            using var webRequest = new UnityWebRequest($"{BaseUrl}{url}", "POST");
            if (jsonData != null)
            {
                var bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }
        
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            if (Token() != null)
            {
                webRequest.SetRequestHeader("Authorization", $"Bearer {Token()}");
            }

            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (webRequest.result != UnityWebRequest.Result.ConnectionError &&
                webRequest.result != UnityWebRequest.Result.ProtocolError)
                return JsonUtility.FromJson<T>(webRequest.downloadHandler.text);
        
            Debug.LogError("Error: " + webRequest.error);
            return default;
        }

        [CanBeNull]
        private static string Token() => PlayerPrefs.GetString("token", null);
    }

    [Serializable]
    public sealed class Response<T>
    {
        public T data;
        public long statusCode;
        [CanBeNull] public string error;
    }
}