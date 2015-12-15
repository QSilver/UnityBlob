using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BehaviourTree : MonoBehaviour
{
    public Texture2D blank;
    public Texture2D rmove;
    public Texture2D reprod;
    public Texture2D fmove;

    public void Display(int state)
    {
        if (state == 0) { this.GetComponent<Image>().sprite = Sprite.Create(blank, new Rect(0, 0, 1, 1), new Vector2(0, 0)); }//this.GetComponent<Panel>.GetComponent<Image>().material.color = new Color(255, 255, 255, 255); }
        else if (state == 1) { this.GetComponent<Image>().sprite = Sprite.Create(rmove, new Rect(0,0,rmove.width, rmove.height), new Vector2(0,0)); }//this.GetComponent<Image>().material.color = new Color(255, 255, 255, 255); }
        else if (state == 2) { this.GetComponent<Image>().sprite = Sprite.Create(reprod, new Rect(0, 0, reprod.width, reprod.height), new Vector2(0, 0)); ; }//this.GetComponent<Image>().material.color = new Color(255, 255, 255, 255); }
        else if (state == 3) { this.GetComponent<Image>().sprite = Sprite.Create(fmove, new Rect(0, 0, fmove.width, fmove.height), new Vector2(0, 0)); ; }//this.GetComponent<Image>().material.color = new Color(255, 255, 255, 255); }
    }
}
