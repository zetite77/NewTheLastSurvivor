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

    public Button m_Btn_Return;
    public Button m_Btn_Exit;

    public Button m_Label_Vibration;
    public Scrollbar m_ScrollBar_Vibration;
    public Text m_ScrollBar_Text;

    void Start()
    {
        m_Btn_Exit.onClick.AddListener(()=>GameManager.Instance.GameOver("asdf", 11, 222));
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
            m_ScrollBar_Text.text = "OFF";
        else
            m_ScrollBar_Text.text = "ON";

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
