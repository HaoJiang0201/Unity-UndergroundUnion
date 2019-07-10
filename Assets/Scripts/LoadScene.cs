using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] Image m_imgBGP1;
    [SerializeField] Image m_imgBGP2;
    [SerializeField] Image m_imgTitle;
    [SerializeField] Button m_btnStart;
    [SerializeField] Button m_btnExit;

    int m_bFadeInOut = 0;
    float m_fFadeStep = 0.02f;
    float m_fCurrentAlpha = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_bFadeInOut = 1;
        m_imgBGP1.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_imgBGP2.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_imgTitle.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_btnStart.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_btnExit.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_bFadeInOut)
        {
            case 1:
            {
                SceneFadeIn();
                break;
            }
            case -1:
            {
                SceneFadeOut();
                break;
            }
            default:
                break;
        }
    }

    void SceneFadeIn()
    {
        // 背景图1 载入
        if (m_imgBGP1.color.a < 1.0f)
        {
            m_fCurrentAlpha += m_fFadeStep;
            m_imgBGP1.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            if (m_imgBGP1.color.a >= 1.0f)
            {
                m_fCurrentAlpha = 0.0f;
                m_imgBGP1.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            // 背景图2 载入
            if (m_imgBGP2.color.a < 1.0f)
            {
                m_fCurrentAlpha += m_fFadeStep;
                m_imgBGP2.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                if (m_imgBGP2.color.a >= 1.0f)
                {
                    m_fCurrentAlpha = 0.0f;
                    m_imgBGP2.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }
            else
            {
                // 标题和按钮载入
                if (m_fCurrentAlpha < 1.0f)
                {
                    m_fCurrentAlpha += m_fFadeStep;
                    m_imgTitle.color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                    m_btnStart.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                    m_btnExit.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                }
                // 场景淡入完成
                else
                {
                    m_fCurrentAlpha = 0.0f;
                    m_bFadeInOut = 0;
                }
            }
        }
    }

    void SceneFadeOut()
    {

    }

}
