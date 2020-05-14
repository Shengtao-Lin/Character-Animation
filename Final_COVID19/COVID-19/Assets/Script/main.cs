using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class main : MonoBehaviour
{
    public int scale;
    private Text timeText,dayText,SText,IText,IPText,modeText;
    public double minute,hour,day;
    public Material MaterialI,MaterialS,MaterialIP;

    public enum Mode{ noMask=1,MaskforAll=2,MaskforInfected=3,}
    public Mode mask;
    
    private int dining;

    public static List<GameObject> agents=new List<GameObject>();
    // Start is called before the first frame update
    private static List<GameObject> doors=new List<GameObject>();
    private static List<GameObject> functionalAreaDoor=new List<GameObject>();
    public static List<GameObject> closeContact=new List<GameObject>();
    public static List<GameObject> closeContactIP=new List<GameObject>();
    public static List<GameObject> IList=new List<GameObject>();
    public static List<GameObject> SList=new List<GameObject>();
    public static List<GameObject> IPList=new List<GameObject>();
    void Start()
    {
        #region initialtime
        day=0;
        hour=0;
        dayText=GameObject.Find("day").GetComponent<Text>();
        timeText=GameObject.Find("time").GetComponent<Text>(); 
        SText=GameObject.Find("S").GetComponent<Text>();
        IText=GameObject.Find("I").GetComponent<Text>();
        IPText=GameObject.Find("IP").GetComponent<Text>();
        modeText=GameObject.Find("Mode").GetComponent<Text>();
        #endregion

        #region initialAgents
        var tempName="";
        for(int i=0;i<40;i++){
            tempName="P"+i.ToString();
            agents.Add(GameObject.Find(tempName));
            SList.Add(agents[i]);
        }
        System.Random rnd = new System.Random();
        int lucky  = rnd.Next(0, 39);
        IList.Add(SList[lucky]);
        SList.RemoveAt(lucky);
       foreach (var item in IList)
       {
           item.GetComponent<MeshRenderer>().material = MaterialI;
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

        dining=0;

    
    }

    // Update is called once per frame
    void Update()
    {
        calculateTime();
        //print(closeContact.Count);
        
    }

    void InfectionCal(){
        System.Random rnd = new System.Random();
        int chance  = 0;
        if(((int)mask)==1){
            foreach (var item in closeContact)
            {
                chance  = rnd.Next(0, 100);
                //print(chance);
                if(chance>97){
                    SList.Remove(item);
                    chance  = rnd.Next(0, 100);
                    if(chance>97){
                        IPList.Add(item);
                        item.GetComponent<MeshRenderer> ().material = MaterialIP;
                    }else{
                        IList.Add(item);
                        item.GetComponent<MeshRenderer> ().material = MaterialI;
                    }
                }
            }
        }else if(((int)mask==2)&&(dining==1)){
            foreach (var item in closeContact)
            {
                chance  = rnd.Next(0, 100);
                if(chance>97){
                    SList.Remove(item);
                    chance  = rnd.Next(0, 100);
                    if(chance>97){
                        IPList.Add(item);
                        item.GetComponent<MeshRenderer> ().material = MaterialIP;
                    }else{
                        IList.Add(item);
                        item.GetComponent<MeshRenderer> ().material = MaterialI;
                    }
                }
            }
        }
        if(((int)mask)==3){
            foreach (var item in closeContactIP)
            {
                chance  = rnd.Next(0, 100);
                if(chance>97){
                    SList.Remove(item);
                    chance  = rnd.Next(0, 100);
                    if(chance>97){
                        IPList.Add(item);
                        item.GetComponent<MeshRenderer> ().material = MaterialIP;
                    }else{
                        IList.Add(item);
                        item.GetComponent<MeshRenderer> ().material = MaterialI;
                    }
                }
            }
        }
        if((((int)mask)==3)&&(dining==1)){
            foreach (var item in closeContact)
            {
                chance  = rnd.Next(0, 100);
                if(chance>97){
                    SList.Remove(item);
                    chance  = rnd.Next(0, 100);
                    if(chance>97){
                        IPList.Add(item);
                        item.GetComponent<MeshRenderer> ().material = MaterialIP;
                    }else{
                        IList.Add(item);
                        item.GetComponent<MeshRenderer> ().material = MaterialI;
                    }
                }
            }
        }
        closeContactIP.Clear();
        closeContact.Clear();
    }
    public static bool IsAgent(GameObject obj){
        return agents.Contains(obj);
    }
    public static bool IsInfected(GameObject obj){
        return IList.Contains(obj);
    }
    public static bool IsIP(GameObject obj){
        return IPList.Contains(obj);
    }
    public static bool IsS(GameObject obj){
        return SList.Contains(obj);
    }
    public static bool IsClose(GameObject obj){
        return closeContact.Contains(obj);
    }
    public static bool IsCloseIP(GameObject obj){
        return closeContactIP.Contains(obj);
    }
    void CellDoor(){
        if(hour>=8 && hour<=23){
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
        if((hour>=12 && hour<=23)){
            functionalAreaDoor[1].SetActive(false);
            dining=1;
        }else{
            functionalAreaDoor[1].SetActive(true);
            dining=0;
        }
        //working area door
        if((hour>=8 && hour<=23)){
            functionalAreaDoor[2].SetActive(false);
        }else{
            functionalAreaDoor[2].SetActive(true);
        }
        //yard door
        if(hour>=15 && hour<=23){
            functionalAreaDoor[0].SetActive(false);
        }else{
            functionalAreaDoor[0].SetActive(true);
        }

        if((hour>=14 && hour<=17)){
            dining=1;
        }else{
            dining=0;
        }
    }

    void TextCal(){
        dayText.text="Day: "+day.ToString();
        timeText.text="Time: "+hour.ToString()+":"+((int)minute).ToString();
        SText.text="Susceptible Population:" + (SList.Count).ToString();
        IPText.text="People in Incubation Period: " + (IPList.Count).ToString();
        IText.text="Infected Population: "+ (IList.Count).ToString();
        if ((int)mask==1)
        {
            modeText.text="No Mask";
        }else if ((int)mask==2)
        {
            modeText.text="Mask For All";
        }else if ((int)mask==3)
        {
            modeText.text="Mask For Infected";
        }
        
    }
    void calculateTime(){
        minute+=Time.deltaTime*scale;
        if(minute>=60){
            hour++;
            minute=0;
            CellDoor();
            InfectionCal();
        }else if(hour>=24){
            day++;
            hour=0;
            print("Day:"+day.ToString()+" S: "+ (SList.Count).ToString()+" I: "+ (IList.Count).ToString()+" IP: "+ (IPList.Count).ToString());
        }
        TextCal();
    }
}