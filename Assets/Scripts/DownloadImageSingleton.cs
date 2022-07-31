using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadImageSingleton : MonoBehaviour
{
    public static DownloadImageSingleton Instance { get; private set; }
    public bool isDownloadedImage = false;
    public int downloadingImageCount;
    public delegate void ImageDownloaded();
    public event ImageDownloaded eventImageDownloaded;

    public int count;
    private void Awake()
    {      
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public IEnumerator SetImage(string url, Image image)
    {
        //if (downloadingImageCount <= 3)
        //{
            downloadingImageCount += 1;
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            //request.timeout = 10;
            Debug.Log(request);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
                image.sprite = image.sprite;
                downloadingImageCount -= 1;
            }
            else
            {
                downloadingImageCount -= 1;
                Texture2D img = ((DownloadHandlerTexture)request.downloadHandler).texture;
                image.sprite = Sprite.Create(img, new Rect(0, 0, 800, 800), Vector2.zero);
            }
            isDownloadedImage = true;
            count += 1;
        //}
    }
}
