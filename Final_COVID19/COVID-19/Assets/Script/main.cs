using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class main : MonoBehaviour
{
    public int scale;
    private Text timeText,dayText;
    public double minute,hour,day;

    public static List<GameObject> agents=new List<GameObject>();
    // Start is called before the first frame update
    private static List<GameObject> doors=new List<GameObject>();
    private static List<GameObject> functionalAreaDoor=new List<GameObject>();
    public static List<GameObject> closeContact=new List<GameObject>();
    void Start()
    {
        #region initialtime
        day=0;
        hour=0;
        dayText=GameObject.Find("day").GetComponent<Text>();
        timeText=GameObject.Find("time").GetComponent<Text>();   
        #endregion

        #region initialAgents
        var tempName="";
        for(int i=0;i<40;i++){
            tempName="P"+i.ToString();
            agents.Add(GameObject.Find(tempName));
        }
        #endregion
    
        #region initialDoors
        for(int i=1;i<=20;i++){
            tempName="door"+i.ToString();
            doors.Add(GameObject.Find(tempName));
        }
        #endregion
        
        #region initialFunctionalDoor
        for(int i=1;i<=3;i++){
            tempName="Fdoor"+i.ToString();
            functionalAreaDoor.Add(GameObject.Find(tempName));
        }
        #endregion



    
    }

    // Update is called once per frame
    void Update()
    {
        calculateTime();

        
        
    }
    public static bool IsAgent(GameObject obj){
        return agents.Contains(obj);
    }

    void CellDoor(){
        if(hour>=8 & hour<=20){
            foreach (var item in doors)
            {
                item.SetActive(false);
            }
        }else{
            foreach (var item in doors)
            {
                item.SetActive(true);
            }
        }
//Dining room door
        if((hour>=8 & hour<=10)|(hour>=11 & hour<=14)|(hour>=18 & hour<=21)){
            functionalAreaDoor[2].SetActive(false);
        }else{
            functionalAreaDoor[2].SetActive(true);
        }
        //working area door
        if((hour>=9 & hour<=12)|(hour>=13 & hour<=17)){
            functionalAreaDoor[1].SetActive(false);
        }else{
            functionalAreaDoor[1].SetActive(true);
        }
        //yard door
        if(hour>=16 & hour<=19){
            functionalAreaDoor[0].SetActive(false);
        }else{
            functionalAreaDoor[0].SetActive(true);
        }
    }

    void TextCal(){
        dayText.text="Day: "+day.ToString();
        timeText.text="Time: "+hour.ToString()+":"+((int)minute).ToString();
    }
    void calculateTime(){
        minute+=Time.deltaTime*scale;
        if(minute>=60){
            hour++;
            minute=0;
            CellDoor();
        }else if(hour>=24){
            day++;
            hour=0;
        }
        TextCal();
    }
}