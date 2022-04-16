using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] Sprite Stop;
    [SerializeField] Sprite Play;
    bool enable = true;

    public void OnClick()
    {
        if (enable)
        {
            GetComponent<SpriteRenderer>().sprite = Stop;
            enable = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Play;
            enable = true;
        }
    }
}
