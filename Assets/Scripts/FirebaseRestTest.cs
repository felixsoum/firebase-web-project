using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class FirebaseRestTest : MonoBehaviour
{
    // Replace this with your actual Firebase Realtime Database URL
    private string DatabaseURL => string.Format("https://{0}.firebaseio.com/test2.json", Secrets.FirebaseURL);

    void Start()
    {
        StartCoroutine(SendTestData());
    }

    IEnumerator SendTestData()
    {
        Debug.Log("Sending { foo: bar } to Firebase...");

        string jsonData = "{\"red\":\"blue\"}";

        UnityWebRequest request = new UnityWebRequest(DatabaseURL, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("PUT successful: " + request.downloadHandler.text);
            StartCoroutine(GetTestData());
        }
        else
        {
            Debug.LogError("PUT failed: " + request.error);
        }
    }

    IEnumerator GetTestData()
    {
        Debug.Log("Reading back data from Firebase...");

        UnityWebRequest request = UnityWebRequest.Get(DatabaseURL);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("GET successful: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("GET failed: " + request.error);
        }
    }
}
