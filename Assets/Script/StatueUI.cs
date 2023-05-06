using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueUI : MonoBehaviour
{
    [SerializeField]
    private GameObject UIprefab;
    [SerializeField]
    private Transform parents;
    private List<GameObject> soulSelectorUI = new List<GameObject>();
    [SerializeField]
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < UIManager.GetUIManager().SoulSeclectorUINum; i++)
        {
            soulSelectorUI.Add(Instantiate(UIprefab, parents) as GameObject);
        }
    }
    public void Initialize(List<string> soulList)
    {
        for(int i = 0; i< soulList.Count; i++)
        {
            soulSelectorUI[i].GetComponent<SoulSelectorUI>().Initialize(soulList[i]);
        }
    }
}
