using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICounter : MonoBehaviour
{
    Image counterImage;
    GameController gameController;
    Text label;
	void Start ()
    {
        counterImage = GetComponent<Image>();
        label = GetComponentInChildren<Text>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }
	
	void Update ()
    {
        float food = gameController.ActivePlayer.GetCurrentFood();
        int foodInt = Mathf.FloorToInt(food);

        //if (foodInt > 0)
        {
            label.text = "" + foodInt;

            if (food - foodInt > 0f)
                counterImage.fillAmount = food - foodInt;
            else if(foodInt > 0)
                counterImage.fillAmount = 1f;
        }
        /*else
        {
            label.text = "0";
            counterImage.fillAmount = 0f;
        }*/
    }
}
