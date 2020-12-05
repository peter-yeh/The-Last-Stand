using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{

    private Transform bar;

    private void Start()
    {
        bar = transform.Find("Bar");
    }


    public void SetSize(float sizeNormalised)
    {
        bar.
            localScale = 
            new Vector2(
                sizeNormalised, 1f);
    }


    public void SetColor(Color color)
    {
        bar.Find("BarImage").GetComponent<Image>().color = color;
    }


}