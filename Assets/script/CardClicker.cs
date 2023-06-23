using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardClicker : MonoBehaviour
{
    private CardsToShow showScript;
    // Start is called before the first frame update
    void Start()
    {
        showScript = GameObject.Find("SelfPlayer").GetComponent<CardsToShow>();
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

   private void OnClick()
    {
        showScript.HandleSelect(gameObject);

    }
}
