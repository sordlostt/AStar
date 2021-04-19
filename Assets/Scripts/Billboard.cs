using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    TMP_Text tileText;

    private void Awake()
    {
        tileText.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void SetText(string text)
    {
        tileText.text = text;
        tileText.gameObject.SetActive(true);
    }

    public void DisableText()
    {
        tileText.gameObject.SetActive(false);
    }
}
