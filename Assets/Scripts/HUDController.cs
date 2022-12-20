using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    Player player;

    private TextMeshProUGUI hp_text;
    private Slider hp_slider;

    float curVel;

    void Start(){
        player = GameObject.Find("Player").GetComponent<Player>();
        hp_text = GameObject.Find("HPSliderText").GetComponent<TextMeshProUGUI>();
        hp_slider = GameObject.Find("HPSlider").GetComponent<Slider>();
        hp_slider.maxValue = player.max_hp;
        hp_slider.value = player.hp;
        hp_text.text = player.hp.ToString() + "/" + player.max_hp.ToString();
    }

    void Update(){
        hp_text.text = player.hp.ToString() + "/" + player.max_hp.ToString();
        hp_slider.maxValue = player.max_hp;
        hp_slider.value = Mathf.SmoothDamp(hp_slider.value, player.hp, ref curVel, 6 * Time.deltaTime);
    }
}
