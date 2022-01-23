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
    public Scrollbar m_ScrollBar_Vibration;
    public Image m_ImgScrollBar;
    public Sprite m_ImgScrollBar_ON;
    public Sprite m_ImgScrollBar_OFF;

    public Sprite m_Volume;
    public Sprite m_Mute;

    void Start()
    {
        m_soundEffectSlider = m_LabelSoundEffect.GetComponentInChildren<Slider>();
        m_BackgroundMusicSlider = m_LabelBackgroundMusic.GetComponentInChildren<Slider>();
    }
    private void OnEnable()
    {
        Time.timeScale = 0; // 게임중 셋팅창이켜지면 시간이 흐르면 안됨.
    }
    private void OnDisable()
    {
        Time.timeScale = 1; // 셋팅창이꺼지면 시간이 흐르면 됨.
    }

    void Update()
    {
        if (m_ScrollBar_Vibration.value >= 0.5f)
            m_ImgScrollBar.sprite = m_ImgScrollBar_OFF;
        else
            m_ImgScrollBar.sprite = m_ImgScrollBar_ON;

        SoundControl(); // 효과음, 배경음 볼륨 조절
    }

    void SoundControl()
    {
        // 싱글톤 게임매니저 접근
        foreach (AudioSource soundEffect in GameManager.Instance.m_SoundEffect)
            soundEffect.volume = m_soundEffectSlider.value;
        GameManager.Instance.m_BackgroundMusic.volume = m_BackgroundMusicSlider.value;
    }
}
