using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttackZoneView : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}
