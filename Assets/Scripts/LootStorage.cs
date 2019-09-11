using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootStorage : MonoBehaviour
{
    private uint endo;

    Text endoQuantityText;

    public LootStorage()
    {
        endo = 0;
        endoQuantityText = GameObject.Find("UI/EndoContainer/Canvas/EndoQuantity").GetComponent<Text>();
        endoQuantityText.text = "0";
    }

    public void putEndo(uint endoQuantity)
    {
        endo += endoQuantity;
        endoQuantityText.text = endo.ToString();
    }
}
