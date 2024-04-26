using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionCanvasButtonsBugFix : MonoBehaviour
{
    [System.Obsolete]
    private void OnEnable()
    {
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.transform.position = new Vector2(850, child.transform.position.y);
            //child.transform.localScale = new Vector2(1, 1);
        }
    }
}
