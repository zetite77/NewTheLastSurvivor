using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{
    public GameObject m_LabelSoundEffect;
    private Slider m_soundEffectSlider;
    public GameObject m_LabelBackgroundMusic;
    private Slider m_BackgroundMusicSlider;

    public Button m_Label_Vibration;
    public Button m_Btn_Return;
    public Scrollbar m_ScrollBar_Vibration;
    public Text m_ScrollBar_Text;

    void Start()
    {
        m_soundEffectSlider = m_LabelSoundEffect.GetComponentInChildren<Slider>();
        m_BackgroundMusicSlider = m_LabelBackgroundMusic.GetComponentInChildren<Slider>();
        m_Btn_Return.onClick.AddListener(() => Time.timeScale = 1); // 리턴버튼
    }
    private void OnEnable()
    {
        Time.timeScale = 0; // 게임중 셋팅창이켜지면 시간이 흐르면 안됨.
    }

    void Update()
    {
        if (m_ScrollBar_Vibration.value >= 0.5f)
            m_ScrollBar_Text.text = "OFF";
        else
            m_ScrollBar_Text.text = "ON";

        SoundControl(); // 효과음, 배경음 볼륨 조절
    }

    void SoundControl()
    {
        // 싱글톤 게임매니저 접근
        foreach (AudioSource soundEffect in GameManager.Instance.m_NightSound)
            soundEffect.volume = m_soundEffectSlider.value;
        GameManager.Instance.m_BackgroundMusic.volume = m_BackgroundMusicSlider.value;
    }
}
