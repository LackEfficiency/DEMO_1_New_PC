using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Sound : Singleton<Sound>
{
    protected override void Awake()
    {
        base.Awake();
        m_bgSound = this.gameObject.AddComponent<AudioSource>();
        m_bgSound.playOnAwake = false;
        m_bgSound.loop = true;

        m_effectSound = this.gameObject.AddComponent<AudioSource>();

    }

    public string ResourceDir = "Sound";

    AudioSource m_bgSound;
    AudioSource m_effectSound;

    //音乐大小
    public float BgVolume
    {
        get { return m_bgSound.volume; }
        set { m_bgSound.volume = value; }
    }

    //音效大小
    public float EffectVolume
    {
        get { return m_effectSound.volume; }
        set { m_effectSound.volume = value; }
    }

    //播放音乐
    public void PlayBg(string audioName)
    {
        //获取当前背景音乐
        string oldname;
        if (m_bgSound.clip == null)
            oldname = "";
        else
            oldname = m_bgSound.clip.name;

        //当需要切换音乐时，才触发
        if (oldname != audioName)
        {
            //音乐文件路径
            string path;
            if (string.IsNullOrEmpty(ResourceDir))
                path = "";
            else
                path = ResourceDir + "/" + audioName;

            //加载音乐
            AudioClip clip = Resources.Load<AudioClip>(path);

            //播放
            if (clip != null)
            {
                m_bgSound.clip = clip;
                m_bgSound.Play();
            }
        }


    }


    //停止音乐


    //播放特效

}