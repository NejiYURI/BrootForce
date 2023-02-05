using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialScript : MonoBehaviour
{
    public GameObject FollowTarget;
    public TextMeshProUGUI TutorialTxt;
    public string MovementString;
    public string StompString;


    void Start()
    {
        if (TutorialTxt != null)
            TutorialTxt.text = MovementString;
        if (MainMenuScript.instance != null) MainMenuScript.instance.PlayerInField.AddListener(PlayerInField);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = FollowTarget.transform.position;
    }

    void PlayerInField(GameObject obj,bool isIn)
    {
        if (obj == this.FollowTarget)
        {
            if (TutorialTxt != null)
            {
                if(isIn) TutorialTxt.text = StompString;
                else TutorialTxt.text = MovementString;
            }
               
        }
    }
}
