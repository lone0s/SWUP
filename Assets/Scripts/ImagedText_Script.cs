using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagedText_Script : MonoBehaviour
{
    public string iconPath;
    public string text;
    private Image image;
    private Text iconName;

    void Start()
    {
        iconName = transform.GetComponentInChildren<Text>();

        Text[] components = transform.GetComponentsInChildren<Text>();
        iconName.text = this.text;
        image = transform.Find("Icon").GetComponent<Image>();
        float imgWidth, imgHeight;
        getWidthHeightOfImg(image, out imgWidth, out imgHeight);
        setIconImage(makeSpriteOfPngFile(iconPath, imgWidth, imgHeight));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void getWidthHeightOfImg(Image image, out float width, out float height)
    {
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
    } 


    public static Sprite makeSpriteOfPngFile(string imgPath, float width, float height)
    {
        Texture2D texture = new Texture2D((int)width, (int)height);
        byte[] imgData = System.IO.File.ReadAllBytes(imgPath);
        texture.LoadImage(imgData);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    void setIconImage(Sprite icon)
    {
        image.sprite = icon;
    }
}
