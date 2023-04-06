using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class falling : MonoBehaviour
{
    private static int count;
    private static int ballcount;
    public Text Score;
    public Text Total;
    void Start()
    {
        count = 0;
        Total.text = "";
        Score.text = "";
    }

    void LateUpdate()
    {
        if (transform.position.y <0.75f) //���O�_�I����
        {
            gameObject.SetActive(false); //�O���ܱN���Active�]��false
            count++;   
            if (count == 10)
            {
               Total.text = "Strike!";
            }
            else
            { 
             ScoreSet();
            }
        }
    }
    void ScoreSet()
    {
        Score.text = "Score: " + count.ToString();
    }
}
