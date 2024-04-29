using UnityEngine;
using UnityEngine.UI;

public class CurrencyController : MonoBehaviour
{
	Text counter;
	public int currency;

	public void AddCurrency(int amount)
	{
		currency += amount;
	}

	public void SubtractCurrency(int amount)
	{
		currency -= amount;
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
