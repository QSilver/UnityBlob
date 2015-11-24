using UnityEngine;
public class BlobDNA : MonoBehaviour
{
    string DNA = "";

    public void setDNA(string s)
    {
        this.DNA = s;
    }

    public string getDNA()
    {
        return this.DNA;
    }
}