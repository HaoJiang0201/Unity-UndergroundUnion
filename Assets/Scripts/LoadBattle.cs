using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBattle : MonoBehaviour
{
    // General Object
    [SerializeField] GameObject m_spBGP;
    [SerializeField] GameObject m_spPlayer;
    // Enemy Relevants
    [SerializeField] GameObject m_spEnemyBlood;
    [SerializeField] GameObject m_spEnemyHalf2;
    [SerializeField] GameObject m_spEnemyHalf1;
    [SerializeField] GameObject m_spEnemyHPCover;
    [SerializeField] GameObject m_spEnemyHP;
    [SerializeField] GameObject m_spEnemyFire;
    // Player Relevants
    [SerializeField] GameObject m_spPlayerHand;
    [SerializeField] GameObject m_spPlayerPanel;
    [SerializeField] GameObject m_spPlayerHead3;
    [SerializeField] GameObject m_spPlayerHead2;
    [SerializeField] GameObject m_spPlayerHead1;
    [SerializeField] GameObject m_spPlayerHP;
    [SerializeField] GameObject m_spPlayerMP;
    [SerializeField] GameObject m_spAuxPoint;
    [SerializeField] GameObject m_spAuxArea;
    [SerializeField] GameObject m_spPlayerFire;
    // UI Buttons
    Canvas myCanvas;
    [SerializeField] Button m_btnReload;
    [SerializeField] Button m_btnBag;
    [SerializeField] Image m_imgPlayerBlood;
    [SerializeField] GameObject m_uiGameOver;
    [SerializeField] Image m_imgFadeOutMask;

    // Fade In Relevants
    int m_iBattleStatus = 0;  // 0 - Fade In, 1 - PlayerBattling, 2 - Battle Completed, 3 - Fade Out
    float m_fFadeStep = 0.01f;
    float m_fCurrentAlpha = 0.0f;

    /******** Battleing ********/
    // Sight Bead
    float m_fAuxPointPosX = -1.875f;
    float m_fAuxPointPosLeft = -1.875f;
    float m_fAuxPointPosRight = 5.125f;
    float m_fAuxPointMoveStep = 0.1f;
    int m_iAuxPointStatus = 0;  // 0 - Stop, -1 - Left, 1 - Right
    bool m_bAuxPointHit = false;  // 是否命中
    float m_fAuxAreaLeft = 0.75f;
    float m_fAuxAreaRight = 2.45f;
    // Player Hands
    AudioSource m_asPlayerGun;
    bool m_bPlayerShooting = false;
    int m_iPlayerShootStatus = 0;  // 0 - Stop, -1 - Hands Down, 1 - Shoot Up
    float m_fHandsMoveStep = 0.025f;
    float m_fHandX = 3.0f;
    float m_fHandY = -1.5f;
    // Shake Basic
    float m_fShakeTime = 0.15f;  //震动时间
    float m_fShakeAmount = 0.15f;  //振幅
    // Enemy Hurt
    bool m_bEnemyHurting = false;
    bool m_bEnemyBlooding = false;
    float m_fEnemyHP = 100f;
    // Player Bullet
    int m_iBulletLeft = 10;
    float m_fGunFireFadeStep = 0.05f;
    float m_fGunFireAlpha = 1.0f;
    // Enemy Attack
    AudioSource m_asEnemyGun;
    bool m_bEnemyAttacking = false;
    float m_fEnemyAttackFadeStep = 0.01f;
    float m_fEnemyAttackAlpha = 0.0f;
    // Player Hurt
    AudioSource m_asPlayerHurt;
    AudioSource m_asPlayerDeath;
    bool m_bPlayerHurting = false;
    bool m_bPlayerBlooding = false;
    float m_fPlayerHP = 100f;
    // Result and Scene Jump
    int m_iNextScene = 0;
    int m_iResult = 0;


    // Start is called before the first frame update
    void Start()
    {
        // Enemy Relevants
        m_spEnemyBlood.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spEnemyHalf2.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spEnemyHalf1.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spEnemyHPCover.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spEnemyHP.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spEnemyFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        // Player Relevants
        m_spPlayerHand.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spPlayerPanel.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spPlayerHead1.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spPlayerHP.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spPlayerMP.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spAuxPoint.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spAuxArea.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_spPlayerFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        // UI Buttons
        myCanvas = FindObjectOfType<Canvas>();
        m_btnReload.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_btnBag.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_imgPlayerBlood.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        m_uiGameOver.SetActive(false);
        // Audio Relevants
        m_asPlayerGun = m_spPlayerHand.GetComponent<AudioSource>();
        m_asEnemyGun = m_spEnemyHalf2.GetComponent<AudioSource>();
        m_asPlayerHurt = m_spPlayerHead1.GetComponent<AudioSource>();
        // Parameters Init
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(m_iBattleStatus)
        {
            case 0: BattleLoading(); break;
            case 1:
            {
                PlayerBattling();
                EnemyAI();
                if (m_fPlayerHP <= 0.0f)
                {
                    BattleResult(-1);
                }
                if (m_fEnemyHP <= 0.0f)
                {
                    m_fCurrentAlpha = 1.0f;
                    m_fFadeStep = 0.01f;
                    m_spEnemyBlood.GetComponent<AudioSource>().Play();
                    BattleResult(1);
                }
                break;
            }
            case 2:
            {
                if(m_iResult == 1)
                {
                    BattleResult(1); // 敌人淡出过程
                }
               break;
            } 
            case 3: SceneFadeOut(m_iNextScene);  break;
            default: break;
        }
       
    }
    
    void BattleLoading()
    {
        // 各个精灵载入
        if (m_fCurrentAlpha < 1.0f)
        {
            m_fCurrentAlpha += m_fFadeStep;

            // Enemy Relevants
            m_spEnemyHalf1.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spEnemyHPCover.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spEnemyHP.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            // Player Relevants
            m_spPlayerHand.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spPlayerPanel.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spPlayerHead1.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spPlayerHP.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spPlayerMP.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spAuxPoint.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spAuxArea.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            // UI Buttons
            m_btnReload.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_btnBag.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);

            if (m_fCurrentAlpha >= 1.0f)
            {
                m_fCurrentAlpha = 0.0f;
                m_iBattleStatus = 1;
                m_iAuxPointStatus = 1;  // Moving Right at first
            }
        }
    }

    void PlayerBattling()
    {
        // 玩家受伤震动
        if (m_bPlayerHurting)
        {
            m_asPlayerHurt.Play();
            //Handheld.Vibrate();
            if (m_fShakeTime > 0f)
            {
                float fRandomX = UnityEngine.Random.Range(m_fShakeAmount * -1f, m_fShakeAmount);
                float fRandomY = UnityEngine.Random.Range(m_fShakeAmount * -1f, m_fShakeAmount);
                m_spBGP.transform.position = new Vector3(0f, 0f, 0f) + new Vector3(fRandomX, fRandomY, m_spBGP.transform.position.z);
                m_spPlayer.transform.position = new Vector3(0f, 0f, 0f) + new Vector3(fRandomX, fRandomY, m_spPlayer.transform.position.z);
                m_fShakeTime -= Time.deltaTime;
            }
            else
            {
                m_bPlayerHurting = false;
                m_fShakeTime = 0.1f;
                m_spBGP.transform.position = new Vector3(0f, 0f, m_spBGP.transform.position.z);
                m_spPlayer.transform.position = new Vector3(0f, 0f, m_spPlayer.transform.position.z);
            }
        }
        // 玩家流血效果
        if (m_bPlayerBlooding)
        {
            if (m_fCurrentAlpha > 0.0f)
            {
                // 玩家血液淡出
                m_fCurrentAlpha -= m_fFadeStep;
                m_imgPlayerBlood.color = new Color(1.0f, 0.2f, 0.2f, m_fCurrentAlpha);
                // 玩家血条移动
                m_fPlayerHP -= (m_fFadeStep / 5f) * 200f;
                float fScaleX = m_fPlayerHP / 100f;
                m_spPlayerHP.transform.localScale = new Vector3(fScaleX, m_imgPlayerBlood.transform.localScale.y, m_imgPlayerBlood.transform.localScale.z);
            }
            else
            {
                m_bPlayerBlooding = false;
                m_fCurrentAlpha = 0.0f;
                m_imgPlayerBlood.color = new Color(1.0f, 0.2f, 0.2f, 0.0f);
                m_spPlayerHP.transform.localScale = new Vector3(m_fPlayerHP / 100f, m_imgPlayerBlood.transform.localScale.y, m_imgPlayerBlood.transform.localScale.z);
            }
            return;
        }

        // 玩家射击过程
        if (m_bPlayerShooting)
        {
            // 手上的动作
            switch (m_iPlayerShootStatus)
            {
                // 抬手
                case 1:
                {
                    // 还未到达最右端
                    if (m_spPlayerHand.transform.position.x < 3.25f)
                    {
                        m_fHandX += m_fHandsMoveStep;
                        m_fHandY -= m_fHandsMoveStep * 2;
                        m_spPlayerHand.transform.position = new Vector3(m_fHandX, m_fHandY, m_spPlayerHand.transform.position.z);
                    }
                    // 到达最右端之后
                    else
                    {
                        m_iPlayerShootStatus = -1;
                        // 打中敌人
                        if (m_bAuxPointHit)
                        {
                            m_bEnemyHurting = true;
                            m_bEnemyBlooding = true;
                            m_fCurrentAlpha = 1.0f;
                            m_fFadeStep = 0.025f;
                            m_spEnemyBlood.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        }
                        m_iBulletLeft -= 1;
                        m_spPlayerMP.transform.localScale = new Vector3(m_iBulletLeft/10f, m_spPlayerMP.transform.localScale.y, m_spPlayerMP.transform.localScale.z);
                    }
                    break;
                }
                // 回落
                case -1:
                {
                    // 还未到达最左端
                    if (m_spPlayerHand.transform.position.x > 3f)
                    {
                        m_fHandX -= m_fHandsMoveStep;
                        m_fHandY += m_fHandsMoveStep * 2;
                        m_spPlayerHand.transform.position = new Vector3(m_fHandX, m_fHandY, m_spPlayerHand.transform.position.z);
                    }
                    // 到达最左端之后
                    else
                    {
                        m_iPlayerShootStatus = 0;
                        m_bPlayerShooting = false;
                        m_bAuxPointHit = false;
                        // Reload
                        if (m_iBulletLeft <= 0)
                        {
                            m_iBulletLeft = 10;
                            m_btnReload.GetComponent<AudioSource>().Play();
                            m_spPlayerMP.transform.localScale = new Vector3(1.0f, m_spPlayerMP.transform.localScale.y, m_spPlayerMP.transform.localScale.z);
                        }
                     }
                    break;
                }
                default: break;
            }
            // 枪击火焰
            if(m_fGunFireAlpha > 0.0f)
            {
                m_fGunFireAlpha -= m_fGunFireFadeStep;
                m_spPlayerFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fGunFireAlpha);
            }
            else
            {
                m_fGunFireAlpha = 0.0f;
                m_spPlayerFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
            return;
        }
        // 敌人攻击中不做后续判断
        if (m_bEnemyAttacking)
            return;
        // 瞄准星移动
        switch (m_iAuxPointStatus)
        {
            case 0: break;
            case 1:
                {
                    // 还未到达最右端
                    if (m_fAuxPointPosX < m_fAuxPointPosRight)
                    {
                        m_fAuxPointPosX += m_fAuxPointMoveStep;
                        m_spAuxPoint.transform.position = new Vector3(m_fAuxPointPosX, m_spAuxPoint.transform.position.y, m_spAuxPoint.transform.position.z);
                    }
                    // 到达最右端之后
                    else
                    {
                        m_iAuxPointStatus = -1;
                    }
                    break;
                }
            case -1:
                {
                    // 还未到达最左端
                    if (m_fAuxPointPosX > m_fAuxPointPosLeft)
                    {
                        m_fAuxPointPosX -= m_fAuxPointMoveStep;
                        m_spAuxPoint.transform.position = new Vector3(m_fAuxPointPosX, m_spAuxPoint.transform.position.y, m_spAuxPoint.transform.position.z);
                    }
                    // 到达最右端之后
                    else
                    {
                        m_iAuxPointStatus = 1;
                    }
                    break;
                }
            default: break;
        }
        // 鼠标点击射击（射击过程中无效）
        if (Input.GetMouseButtonDown(0))
        {
            m_asPlayerGun.Play();
            m_bPlayerShooting = true;
            m_iPlayerShootStatus = 1;
            if (m_fAuxPointPosX >= m_fAuxAreaLeft && m_fAuxPointPosX <= m_fAuxAreaRight)
            {
                m_bAuxPointHit = true;
                //Handheld.Vibrate();
            }
            else
            {
                m_bAuxPointHit = false;
            }
            m_spPlayerFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    void EnemyAI()
    {
        // 敌人受伤震动
        if(m_bEnemyHurting)
        {
            if(m_fShakeTime > 0f)
            {
                float fRandomX = UnityEngine.Random.Range(m_fShakeAmount * -1f, m_fShakeAmount);
                float fRandomY = UnityEngine.Random.Range(m_fShakeAmount * -1f, m_fShakeAmount);
                m_spEnemyHalf1.transform.position = new Vector3(-1f, 0.75f, 10f) + new Vector3(fRandomX, fRandomY, 0f);
                m_spEnemyHalf2.transform.position = new Vector3(-1f, 0.75f, 9f) + new Vector3(fRandomX, fRandomY, 0f);
                m_fShakeTime -= Time.deltaTime;
            }
            else
            {
                m_bEnemyHurting = false;
                m_fShakeTime = 0.1f;
                m_spEnemyHalf1.transform.position = new Vector3(-1f, 0.75f, 10f);
                m_spEnemyHalf2.transform.position = new Vector3(-1f, 0.75f, 9f);
            }
        }
        // 敌人流血效果
        if (m_bEnemyBlooding)
        {
            if (m_fCurrentAlpha > 0.0f)
            {
                // 敌人血液淡出
                m_fCurrentAlpha -= m_fFadeStep;
                m_spEnemyBlood.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
                // 敌人血条移动
                m_fEnemyHP -= (m_fFadeStep / 5f) * 100f;
                float fScaleX = m_fEnemyHP / 100f;
                m_spEnemyHP.transform.localScale = new Vector3(fScaleX, m_spEnemyHP.transform.localScale.y, m_spEnemyHP.transform.localScale.z);
            }
            else
            {
                m_bEnemyBlooding = false;
                m_fCurrentAlpha = 0.0f;
                m_spEnemyBlood.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                m_spEnemyHP.transform.localScale = new Vector3(m_fEnemyHP / 100f, m_spEnemyHP.transform.localScale.y, m_spEnemyHP.transform.localScale.z);
            }
            return;
        }
        // 玩家攻击中不做后续判断
        if (m_bPlayerShooting)
            return;
        // 敌人进攻中
        if (m_bEnemyAttacking)
        {
            // 敌人枪击火焰
            if (m_fGunFireAlpha > 0.0f)
            {
                m_fGunFireAlpha -= m_fGunFireFadeStep;
                m_spEnemyFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fGunFireAlpha);
            }
            else
            {
                // 玩家受伤标志开启
                m_bPlayerHurting = true;
                m_bPlayerBlooding = true;
                m_fCurrentAlpha = 0.5f;
                m_fFadeStep = 0.025f;
                m_imgPlayerBlood.color = new Color(1.0f, 0.2f, 0.2f, 0.5f);
                // 敌人枪击火焰重置
                m_fGunFireAlpha = 0.0f;
                m_spEnemyFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                // 敌人攻击状态重置
                m_fEnemyAttackAlpha = 0.0f;
                m_spEnemyHalf2.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                m_bEnemyAttacking = false;
            }
            return;
        }
        // 敌人准备攻击的过程
        if(m_spEnemyHalf2.GetComponent<SpriteRenderer>().color.a < 1.0f)
        {
            m_fEnemyAttackAlpha += m_fEnemyAttackFadeStep;
            m_spEnemyHalf2.GetComponent<SpriteRenderer>().color  = new Color(1.0f, 1.0f, 1.0f, m_fEnemyAttackAlpha);
        }
        // 敌人开始攻击
        else
        {
            m_fEnemyAttackAlpha = 1.0f;
            m_spEnemyHalf2.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            m_bEnemyAttacking = true;
            m_asEnemyGun.Play();
            m_fGunFireAlpha = 1.0f;
            m_spEnemyFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    void BattleResult(int iResult)
    {
        m_iResult = iResult;
        m_iBattleStatus = 2;
        // 失败
        if (iResult == -1)
        {
            m_spPlayerHP.GetComponent<AudioSource>().Play();
            myCanvas.GetComponent<AudioSource>().Stop();
            m_uiGameOver.SetActive(true);
            m_imgFadeOutMask.GetComponent<AudioSource>().Play();
            m_fCurrentAlpha = 0.0f;
            m_fFadeStep = 0.01f;
        }
        // 成功，跳转到下一段剧情
        else
        {
            // 场景淡出
            if (m_fCurrentAlpha > 0.0f)
            {
                m_fCurrentAlpha -= m_fFadeStep;
            }
            else
            {
                // 敌人淡出后，场景淡出并跳转
                m_fCurrentAlpha = 0.0f;
                m_iBattleStatus = 3;
                m_iNextScene = 3;
            }
            m_spEnemyBlood.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spEnemyHalf2.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spEnemyHalf1.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spEnemyHPCover.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
            m_spEnemyHP.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, m_fCurrentAlpha);
        }
    }
    
    public void SceneFadeOut(int iSceneID)
    {
        m_uiGameOver.SetActive(false);
        m_iBattleStatus = 3;
        m_iNextScene = iSceneID;
        if (iSceneID != 2)
        {
            // 场景淡出
            if (m_fCurrentAlpha < 1.0f)
            {
                m_fCurrentAlpha += m_fFadeStep;
                m_imgFadeOutMask.color = new Color(0f, 0f, 0f, m_fCurrentAlpha);
            }
            else
            {
                m_fCurrentAlpha = 0.0f;
                m_imgFadeOutMask.color = new Color(0f, 0f, 0f, 1.0f);
                myCanvas.GetComponent<AudioSource>().Stop();
                m_imgFadeOutMask.GetComponent<AudioSource>().Stop();
                FindObjectOfType<SceneJumper>().JumpToScene(iSceneID);
            }
        }
        else
        {
            FindObjectOfType<SceneJumper>().JumpToScene(iSceneID);
        }
    }

    public void BulletReload()
    {
        m_btnReload.GetComponent<AudioSource>().Play();
    }

    public void BagLocked()
    {
        m_btnBag.GetComponent<AudioSource>().Play();
    }

}
