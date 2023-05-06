using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statue : MonoBehaviour
{
    private int[] randomNum = new int[2];
    private List<string> soulList = new List<string>();
    private List<string> playerSoulList = new List<string>();
    private bool isConnectedPlayer = false;
    private bool isActivatedUI = false;
    private bool isSelectedSoul = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (!isConnectedPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isSelectedSoul)
            {
                SelectSoul();
                isSelectedSoul = true;
            }
            if (!isActivatedUI)
            {
                UIManager.GetUIManager().ShowStatueUI(soulList);
                isActivatedUI = true;
            }
            else
            {
                UIManager.GetUIManager().HideStatueUi();
                isActivatedUI = false;
            }
        }
    }

    private void SelectSoul()
    {
        int i = 0;
        bool result = false;
        while (i != UIManager.GetUIManager().SoulSeclectorUINum)
        {
            randomNum[i] = Random.Range(0, DataManager.Instance().SoulList.Count);
            if (FindNumber(i))
                continue;
            foreach(string name in playerSoulList)
            {
                result = name == DataManager.Instance().SoulList[randomNum[i]];
                if (result)
                    break;
            }
            if (result)
                continue;
            soulList.Add(DataManager.Instance().SoulList[randomNum[i]]);
            i++;
        }
    }

    private bool FindNumber(int index)
    {
        for (int j = 0; j < index; j++)
        {
            if (randomNum[j] == randomNum[index])
                return true;
        }
        return false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerSoulList.Clear();
            isConnectedPlayer = true;
            playerSoulList = collision.GetComponent<PlayerController>().GetPlayerSoulNameList();
            foreach(string name in playerSoulList)
            {
                Debug.Log(name);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isConnectedPlayer = false;
            isActivatedUI = false;
            UIManager.GetUIManager().HideStatueUi();
        }
    }
}
