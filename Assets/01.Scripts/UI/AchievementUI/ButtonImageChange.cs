using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImageChange : MonoBehaviour
{
    private Button button;
    public Image image;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        if (button == null)
            return;

        if (button.interactable)
        {
            image.gameObject.SetActive(true);
        }
        else
        {
            image.gameObject.SetActive(false);
        }
    }
}
