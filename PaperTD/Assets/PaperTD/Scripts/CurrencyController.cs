using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyController : MonoBehaviour
{
    Text counter;
    public int currency = 0;

    public void addCurrency(int amount)
    {
        currency += amount;
    }

    void Start()
    {
        counter = GameObject.FindGameObjectWithTag("CurrencyCounter").GetComponent<Text>();
    }
    void LateUpdate()
    {
        counter.text = "Currency: " + currency;
    }
}
