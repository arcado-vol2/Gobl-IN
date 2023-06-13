using UnityEngine;
using UnityEngine.UI;
public class goblin_button : MonoBehaviour
{

    public GameObject text;
    public Button button;

    public void Enable()
    {
        text.SetActive(true);
        button.interactable = true;
    }
    public void Disable()
    {
        text.SetActive(false);
        button.interactable = false;
    }
}
