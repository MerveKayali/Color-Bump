using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{ //benim
    private Text cLevelText, nLevelText;
    private Image fill;

    private float startDistance, distance;
    private GameObject player, finish,hand;
    private int level=1;

    private TextMesh levelNo;
    void Awake()
    {
        cLevelText = GameObject.Find("CurrentLevelText").GetComponent<Text>();
        nLevelText = GameObject.Find("NextLevelText").GetComponent<Text>();
        fill = GameObject.Find("Fill").GetComponent<Image>();

        player = GameObject.Find("Player");
        finish = GameObject.Find("Finish");
        hand = GameObject.Find("Hand");

        levelNo = GameObject.Find("LevelNo").GetComponent<TextMesh>();
        
        
    }
   

    void Start()
    {
        
        level = PlayerPrefs.GetInt("Level",1);
        
      
        levelNo.text = "LEVEL " + level;
        nLevelText.text = level + 1 + "";
        cLevelText.text = level.ToString();

        startDistance = Vector3.Distance(player.transform.position, finish.transform.position);
    }

    private void Update()
    {
        //level ilerleme çubuðunu doldurmak için
       

        distance = Vector3.Distance(player.transform.position, finish.transform.position);
        if(player.transform.position.z<finish.transform.position.z)
            fill.fillAmount = 1 - (distance / startDistance);
    }

    public void RemoveUI()
    {
        hand.SetActive(false);
        GameObject.Find("SwipeText").SetActive(false);
        
    }

}
