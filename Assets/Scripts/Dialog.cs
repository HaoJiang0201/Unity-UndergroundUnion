using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog")] // 在Unity界面下方Assets区域的右键菜单中创建一个“Dialog”子菜单

public class Dialog : ScriptableObject
{
    [SerializeField] public int iFaceHalf = 0;  // 0-Intro, 1-10 Player face, 11-20 Enemy Half
    [SerializeField] public bool bEnd = false;  // End of story in current scene
    [TextArea(1, 1)] [SerializeField] string strSentence1; // 在Inspector中创建一个可以输入文本的地方，且设置了输入框的大小
    [TextArea(1, 1)] [SerializeField] string strSentence2; // 在Inspector中创建一个可以输入文本的地方，且设置了输入框的大小
    [SerializeField] Dialog dlgNext; // 在Inspector中创建了一个可以链接下一个游戏“状态”的地方

    // 获取文本输入框中内容的函数
    public string[] GetCurrentStory()
    {
        string stringWholeSentence = strSentence1 + "|" + strSentence2;
        return stringWholeSentence.Split('|');
    }

    // 获取下一个游戏“状态”的函数
    public Dialog GetNextStory()
    {
        return dlgNext;
    }
}
