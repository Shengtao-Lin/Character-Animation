using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rb;
	public float speed;
	public Text countText1;
	public Text countText2;
	private static int count1=0;
	private static int count2=0;
	private static int score1=0;
	private static int score2=0;
	public Text winT;
	private float jump;
	private bool onGround;
	public enum Player{
		Player1,
		Player2
	}
	public Player player = Player.Player1;
	private int Playernum=1;
	public Button Startb;
	public Button Restartb;
	public Text announcement;
	private static double time=120.5;
	public Text timerT;

	void Start(){

		rb = GetComponent<Rigidbody>();
		//count1=0;
		//count2=0;
		SetCT(1);
		SetCT(2);
		winT.text="";
		onGround=true;
		jump=0.0f;

		switch(player){
			case Player.Player1:
				Playernum=1;
				break;
			case Player.Player2:
				Playernum=2;
				break;
		}

		Pause();

		Startb.onClick.AddListener(UnPause);
		Restartb.onClick.AddListener(Restart);
	}
//Fixed
	void Update(){

		move();
		
	}

	void move(){
		float moveHorizontal = Input.GetAxis("Horizontal"+Playernum);
		float moveVertical = Input.GetAxis("Vertical"+Playernum);
		jump = Input.GetAxis("Jump"+Playernum)*15f;

		if(onGround&&jump>0){
			//rb.AddForce (Vector3.up*jump, ForceMode.Impulse);
			jump=20.1f;
			
			onGround=false;
		}else{
			jump=0f;
			//onGround=true;
		}

		Vector3 movement = new Vector3 (moveHorizontal,jump,moveVertical);
		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("PickUps"))
        {
          	other.gameObject.SetActive (false);
          	if(Playernum==1){
          		count1++;
          		score1++;
          		SetCT(Playernum);
          	}
           	 	
        }
	    if (other.gameObject.CompareTag ("PickUps"))
        {
          	other.gameObject.SetActive (false);
          	if(Playernum==2){
          		count2++;
          		score2++;
       	 		SetCT(Playernum);
          	}
           	
        }





    }

    void SetCT(int a){
    	if(a==1){
    		countText1.text = "Player 1 Score: " + score1.ToString();
    	}
    	if(a==2){
    		countText2.text = "Player 2 Score: " + score2.ToString();
    	}

    	//print("Player 1 Score: " + count1.ToString());
    	//print("Player 2 Score: " + count2.ToString());

    	if((count1+count2)==15){
    		if(score1>score2){
    		winT.text="Player1 Win!!";
    		}
    		else if(score1<score2){
    		winT.text="Player2 Win!!";
    		}else{
    			winT.text="Dual";
    		}
    		Pause();
    	}
    	
    		
    	
    }

    void OnCollisionEnter(Collision other){
    	if(other.gameObject.CompareTag ("GD")){
    		onGround=true;
    	}

    	if (other.gameObject.CompareTag ("Player2"))
        {

          	if((GameObject.FindGameObjectWithTag("Player1").transform.position.y>GameObject.FindGameObjectWithTag("Player2").transform.position.y)&&(GameObject.FindGameObjectWithTag("Player1").transform.position.y-GameObject.FindGameObjectWithTag("Player2").transform.position.y)>0.02){
          		score2--;
          		announcement.text = "Player1 wins the collision!";
          	}else if ((GameObject.FindGameObjectWithTag("Player1").transform.position.y<GameObject.FindGameObjectWithTag("Player2").transform.position.y)&&(GameObject.FindGameObjectWithTag("Player2").transform.position.y-GameObject.FindGameObjectWithTag("Player1").transform.position.y)>0.02){
          		score1--;
          		announcement.text = "Player2 wins the collision!";
          	}
          	SetCT(1); 	
          	SetCT(2); 
        }

        if (other.gameObject.CompareTag ("Wall"))
        {

          	if(Playernum==2){
          		score2--;
          		announcement.text = "Player2 hits the wall!";
          	}
          	if (Playernum==1){
          		score1--;
          		announcement.text = "Player1 hits the wall!";
          	}
          	SetCT(1); 	
          	SetCT(2); 
        }

    }

    void Pause(){
    	Time.timeScale=0;
    	announcement.text = "Game Paused!";
    }

    void UnPause(){
    	Time.timeScale=1;
    	announcement.text = "Game Started!";
    	time=120.5;
    	CountDown();
    	InvokeRepeating("CountDown",1f,1F);
    }

    void Restart(){
    	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    	count1=0;
		count2=0;
		score1=0;
		score2=0;
    	Time.timeScale = 1f;
    	announcement.text = "Game Restarted!";
    	time=120.5;
    }

    void CountDown(){
    	if(time > 0){
    		time-=0.5;
    		timerT.text = "Time left: " + time.ToString()+ "s";
    	}else{

    		if(score1>score2){
    		winT.text="Player1 Win!!";
    		}
    		else if(score1<score2){
    		winT.text="Player2 Win!!";
    		}else{
    			winT.text="Dual";
    		}
    		CancelInvoke();
    		Pause();
    	
    	}

    	if(time==20){
    		announcement.text = "Tiem is almost up!!";
    	}
    }
}
