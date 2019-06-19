using UnityEngine;

public class HexagonHandler : MonoBehaviour
{
    public SpriteRenderer sr;

    public int hexagonColor;

    public void ChangeColor(int colorId)
    {
        switch (colorId)
        {
            case 0:
                sr.sprite = MainHandler.Resources.hexagonSprite;
                break;
            case 1:
                sr.sprite = MainHandler.Resources.blueHexagonSprite;
                break;
            case 2:
                sr.sprite = MainHandler.Resources.redHexagonSprite;
                break;
        }
        
        hexagonColor = colorId;
    }
    
}
