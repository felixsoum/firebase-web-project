using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using Newtonsoft.Json;

public class FirebaseRestTest : MonoBehaviour
{
    [SerializeField] DragCircle dragCircle;
    private string DatabaseURL => string.Format("https://{0}.firebaseio.com/circle.json", Secrets.FirebaseURL);

    public void Save()
    {
        string json = JsonConvert.SerializeObject(dragCircle.GetData());
        StartCoroutine(SendCircleData(json));
        IEnumerator SendCircleData(string jsonData)
        {
            UnityWebRequest request = new UnityWebRequest(DatabaseURL, "PUT");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("PUT successful: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("PUT failed: " + request.error);
            }
        }
    }

    public void Load()
    {
        StartCoroutine(GetCircleData());
        IEnumerator GetCircleData()
        {
            Debug.Log("Reading back data from Firebase...");

            UnityWebRequest request = UnityWebRequest.Get(DatabaseURL);
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                Debug.Log("GET successful: " + json);

                var circleData = JsonConvert.DeserializeObject<CircleData>(json);
                dragCircle.SetData(circleData);
            }
            else
            {
                Debug.LogError("GET failed: " + request.error);
            }
        }
    }
}
