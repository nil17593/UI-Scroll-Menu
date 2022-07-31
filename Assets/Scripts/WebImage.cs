using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebImage : MonoBehaviour
{
    private Image image;
    [SerializeField] private string filename;
    [SerializeField] private bool canCache;
    [SerializeField] private bool canloadDownloadedImage;
    [SerializeField] private string imageUrl;
    private void Start()
    {
        image = GetComponent<Image>();
        //if (File.Exists(Application.persistentDataPath + filename) && canloadDownloadedImage)
        //{
        //    Debug.Log("Loading Cached Image");
        //    LoadCacheImage();
        //}
        //else
        //{
        if (DownloadImageSingleton.Instance.count < 3)
        {
            StartCoroutine(DownloadImageSingleton.Instance.SetImage(imageUrl, image));
            DownloadImageSingleton.Instance.count = 0;
            //canCache = true;
            Debug.Log("Image Downloaded");
            if (canCache)
            {
                Debug.Log("Image Cached");
                StartCoroutine(CacheImage(image));
            }
        }
        //}
    }
    
    IEnumerator CacheImage(Image image)
    {
        yield return new WaitForSeconds(30);
        byte[] texturebytes = image.sprite.texture.EncodeToPNG();
        Debug.Log("Texturebyte "+texturebytes);
        if (texturebytes != null)
        {
            File.WriteAllBytes(Application.persistentDataPath+ image.name + filename, texturebytes);
            //if (DownloadImageSingleton.Instance.eventImageDownloaded != null)
            //{

            //}
        }
        
    }


    void LoadCacheImage()
    {
        byte[] texturebytes = File.ReadAllBytes(Application.persistentDataPath + image.name + filename);
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(texturebytes);
        image.sprite = Sprite.Create(tex, new Rect(0, 0, 800, 800), Vector2.zero);
    }
}
