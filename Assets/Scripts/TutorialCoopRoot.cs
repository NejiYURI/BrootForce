using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialCoopRoot : RootScript
{
    private int StompCount;

    private bool IsCoroutineRunning;

    public GameObject TutorialTxt;

    private int Incount;

    public override void GetStomped()
    {
        StompCount++;
        StartCoroutine(ShakeFunc());
        if (!IsCoroutineRunning) StartCoroutine(StompCounter());
        if (StompCount >= 2)
        {
            Health--;
            if (Health <= 0)
            {
                Health = 1;
                ResetState();
            }
        }
    }

    IEnumerator StompCounter()
    {
        IsCoroutineRunning = true;
        yield return new WaitForSeconds(0.2f);
        StompCount = 0;
        IsCoroutineRunning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (MainMenuScript.instance != null) MainMenuScript.instance.PlayerInField.Invoke(collision.gameObject, true);
        Incount++;
        setTxt();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (MainMenuScript.instance != null) MainMenuScript.instance.PlayerInField.Invoke(collision.gameObject, false);
        Incount--;
        setTxt();
    }

    void setTxt()
    {
        if (TutorialTxt != null)
            TutorialTxt.SetActive(Incount > 0);

    }
}
