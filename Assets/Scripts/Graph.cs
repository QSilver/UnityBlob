using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    static Texture2D texture = new Texture2D(1, 1);
    static Color color = Color.magenta;

    void OnGUI()
    {
        int numranges = (int)Mathf.Pow(2, DNAOperations.DNARANGE);
        int[] ranges = new int[numranges];

        float w = 0;
        float step = 150 / numranges;

        for (int i = 0; i < numranges; i++)
        {
            ranges[i] = 0;
        }

        foreach (GameObject blob in BlobManager.blobs)
        {
            ranges[(int)blob.GetComponent<BlobLogic>().getRange()]++;
        }

        if (this.GetComponent<Toggle>().isOn)
        {
            for (int i = 0; i < numranges; i++)
            {
                for (int j = 0; j < ranges[i]; j++)
                        DrawQuad(new Rect(w, 8*j, step - 2, 6));
                w += step;
            }
        }
    }

    static void DrawQuad(Rect position)
    {
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(position, GUIContent.none);
    }
}
