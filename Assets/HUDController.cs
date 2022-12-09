using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Player player;

    private TextMeshProUGUI level_text;
    private TextMeshProUGUI hp_text;
    private TextMeshProUGUI stamina_text;
    private Slider hp_slider;
    private Slider stamina_slider;

    float curVel;

    void Start(){
        level_text = GameObject.Find("LevelText").GetComponent<TextMeshProUGUI>();
        hp_text = GameObject.Find("HPSliderText").GetComponent<TextMeshProUGUI>();
        stamina_text = GameObject.Find("StaminaSliderText").GetComponent<TextMeshProUGUI>();
        hp_slider = GameObject.Find("HPSlider").GetComponent<Slider>();
        stamina_slider = GameObject.Find("StaminaSlider").GetComponent<Slider>();
        hp_slider.maxValue = player.max_hp;
        hp_slider.value = player.hp;
        stamina_slider.maxValue = player.max_stamina;
        stamina_slider.value = player.stamina;
        level_text.text = player.level.ToString();
        hp_text.text = player.hp.ToString() + "/" + player.max_hp.ToString();
        stamina_text.text = player.stamina.ToString() + "/" + player.max_stamina.ToString();
    }

    void Update(){
        hp_text.text = player.hp.ToString() + "/" + player.max_hp.ToString();
        stamina_text.text = player.stamina.ToString() + "/" + player.max_stamina.ToString();
        hp_slider.maxValue = player.max_hp;
        hp_slider.value = Mathf.SmoothDamp(hp_slider.value, player.hp, ref curVel, 6 * Time.deltaTime);
        stamina_slider.maxValue = player.max_stamina;
        stamina_slider.value = Mathf.SmoothDamp(stamina_slider.value, player.stamina, ref curVel, 6 * Time.deltaTime);
        level_text.text = (player.level).ToString();
    }
}
