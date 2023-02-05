using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialNormalRoot : RootScript
{
    public TextMeshProUGUI tutoroialTxt;
    public override void GetStomped()
    {
        Health--;
        StartCoroutine(ShakeFunc());
        if (Health <= 0)
        {
            Health = 1;
            ResetState();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MainMenuScript.instance != null) MainMenuScript.instance.PlayerInField.Invoke(collision.gameObject, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (MainMenuScript.instance != null) MainMenuScript.instance.PlayerInField.Invoke(collision.gameObject, false);
    }
}
