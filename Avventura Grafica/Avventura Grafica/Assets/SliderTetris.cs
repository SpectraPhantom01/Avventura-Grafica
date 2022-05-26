using UnityEngine;
using UnityEngine.UI;

public class SliderTetris : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TetrisManager TetrisManager;
    [SerializeField] int maxLevel = 15;

    public void OnValueChange()
    {

        TetrisManager.Difficolta = Mathf.Clamp((int)slider.value, 1, maxLevel);
    }

}
