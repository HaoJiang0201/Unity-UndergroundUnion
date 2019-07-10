using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadStory : MonoBehaviour
{
    [SerializeField] Image m_imgBGP;
    [SerializeField] Image m_imgPlayer;
    [SerializeField] Image m_imgEnemy;
    [SerializeField] GameObject m_imgDialog;
    [SerializeField] Text m_textName;
    [SerializeField] Text m_textSentence1;
    [SerializeField] Text m_textSentence2;
    [SerializeField] Dialog m_dlgStart;
    [SerializeField] GameObject m_btnComments;
    [SerializeField] GameObject m_textComments;

    int m_bFadeInOut = 0;
    float m_fFadeStep = 0.02f;
    float m_fCurrentAlpha = 0.0f;

    int m_iCurrentStory = 0;
    Dialog m_dlgNow;
    int m_iStoryStep = 0;   // 0-Fade In, 1-Playing Story, 2-Fade Out

    string m_strCurrentSceneName = "StoryBefore";

    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        m_strCurrentSceneName = scene.name;
        
        m_imgBGP.color = new Color(1f, 1f, 1f, 0f);
        m_imgPlayer.color = new Color(1f, 1f, 1f, 0f);
        if(m_strCurrentSceneName == "StoryBefore")
            m_imgEnemy.color = new Color(1f, 1f, 1f, 0f);
        else
        {
            m_btnComments.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            m_textComments.SetActive(false);
        }
        m_imgDialog.SetActive(false);
        m_textName.text = "";
        m_textSentence1.text = "";
        m_textSentence2.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_iStoryStep)
        {
            case 0:
            {
                // 背景图 载入
                if (m_imgBGP.color.a < 1.0f)
                {
                    m_fCurrentAlpha += m_fFadeStep;
                    m_imgBGP.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                    if (m_imgBGP.color.a >= 1.0f)
                    {
                        m_fCurrentAlpha = 0.0f;
                        m_imgBGP.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    }
                }
                else
                {
                    // 人物淡出
                    if (m_imgPlayer.color.a < 1.0f)
                    {
                        m_fCurrentAlpha += m_fFadeStep;
                        m_imgPlayer.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                        if (m_strCurrentSceneName == "StoryBefore")
                            m_imgEnemy.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha / 2f);
                        if (m_imgPlayer.color.a >= 1.0f)
                        {
                            m_fCurrentAlpha = 0.0f;
                            m_imgPlayer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            if (m_strCurrentSceneName == "StoryBefore")
                                m_imgEnemy.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                            m_imgDialog.SetActive(true);
                            m_dlgNow = m_dlgStart;
                            m_textSentence1.text = m_dlgNow.GetCurrentStory()[0];
                            m_textSentence2.text = m_dlgNow.GetCurrentStory()[1];
                            ImageDisplay();
                            m_iStoryStep = 1;
                        }
                    }
                }
                break;
            }
            case 1:
            {
                DialogDisplay();
                break;
            }
            case 2:
            {
                if (m_strCurrentSceneName == "StoryBefore")
                {
                    if (m_imgPlayer.color.a > 0.0f)
                    {
                        m_fCurrentAlpha -= m_fFadeStep;
                        m_imgPlayer.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                        m_imgEnemy.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha / 2f);
                        if (m_imgPlayer.color.a <= 0.0f)
                        {
                            m_fCurrentAlpha = 1.0f;
                            m_imgPlayer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                            m_imgEnemy.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                            FindObjectOfType<SceneJumper>().JumpToScene(2);
                        }
                    }
                }
                else
                {
                    if (m_imgPlayer.color.a > 0.0f)
                    {
                        m_fCurrentAlpha -= m_fFadeStep;
                        m_imgPlayer.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                        m_imgBGP.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                        if (m_imgPlayer.color.a <= 0.0f)
                        {
                            m_fCurrentAlpha = 0.0f;
                            m_imgPlayer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                            m_imgBGP.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                            m_iStoryStep = 3;
                            m_btnComments.SetActive(true);
                        }
                    }
                }
                break;
            }
            case 3:
        {
                if (m_btnComments.GetComponent<Image>().color.a < 1.0f)
                {
                    m_fCurrentAlpha += m_fFadeStep;
                    m_btnComments.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                }
                else
                {
                    m_btnComments.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    m_textComments.SetActive(true);
                    FindObjectOfType<Canvas>().GetComponent<AudioSource>().Stop();
                }
                break;
            }
            default:
                break;
        }
    }

    void DialogDisplay()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_dlgNow.bEnd)
            {
                m_imgDialog.SetActive(false);
                m_iStoryStep = 2;
                m_fCurrentAlpha = 1.0f;
            }
            else
            {
                var dlgNext = m_dlgNow.GetNextStory();
                m_dlgNow = dlgNext;
                m_textSentence1.text = m_dlgNow.GetCurrentStory()[0];
                m_textSentence2.text = m_dlgNow.GetCurrentStory()[1];
                ImageDisplay();
            }
        }
    }

    void ImageDisplay()
    {
        // Player Face
        if (m_dlgNow.iFaceHalf >= 1 && m_dlgNow.iFaceHalf <= 9)
        {
            m_textName.text = "Zheng Qin";
            m_imgPlayer.color = new Color(1f, 1f, 1f, 1f);
            if (m_strCurrentSceneName == "StoryBefore")
                m_imgEnemy.color = new Color(1f, 1f, 1f, 0.5f);
            m_imgPlayer.sprite = Resources.Load("Image/PlayerFace" + m_dlgNow.iFaceHalf, typeof(Sprite)) as Sprite;

        }
        // Enemy Face
        if (m_dlgNow.iFaceHalf >= 10 && m_dlgNow.iFaceHalf <= 19)
        {
            m_textName.text = "Spy A";
            m_imgPlayer.color = new Color(1f, 1f, 1f, 0.5f);
            if (m_strCurrentSceneName == "StoryBefore")
                m_imgEnemy.color = new Color(1f, 1f, 1f, 1f);
            m_imgEnemy.sprite = Resources.Load("Image/EnemyHalf" + m_dlgNow.iFaceHalf, typeof(Sprite)) as Sprite;
        }

    }

}
