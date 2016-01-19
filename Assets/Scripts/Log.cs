using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Log : MonoBehaviour {
    String log;
    static String Event;
    //static int aux;
    //static int reset;
    float q;

    public static void Reset()
    {
        //reset = 1;
    }

    public static void PassString(String s)
    {
        //Event += s + " ";
        //aux = 1;
    }

	void Update ()
    {
        /*
        if (reset == 1)
        {
            log = "";
            q = 0;
            reset = 0;
            this.GetComponent<Text>().text = log;
        }
        if (aux == 1)
        {
            q = Main.timestamp;
            log = "[" + q.ToString("0.000") + "]" + " " + Event + System.Environment.NewLine + log;
            if (log.Split(System.Environment.NewLine[0]).Length - 1 > 20)
            {
                int count = 0, i;
                for (i = 0; i < log.Length; i++)
                {
                    if (log[i] == '\n')
                    {
                        count++;
                        if (count == 20) break;
                    }
                }
                log = log.Substring(0, i);
            }
            this.GetComponent<Text>().text = log;
            Event = ""; aux = 0;
        }
         */
	}
}
