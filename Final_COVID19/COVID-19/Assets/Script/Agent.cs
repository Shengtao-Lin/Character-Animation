using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    private int infection=0;//0 is no S, 1 is I, 2 is IP
    // Start is called before the first frame update
    void Start()
    {
        updateI();
    }

    // Update is called once per frame
    void Update()
    {
        updateI();
    }

    void updateI(){
        if(main.IsS(gameObject)){
            infection=0;
        }else if(main.IsInfected(gameObject)){
            infection=1;
        }else if(main.IsIP(gameObject)){
            infection=2;
        }
    }
    public void OnTriggerEnter(Collider other)
    {    
        if((infection==1)||(infection==2)){
            if(main.IsS(other.gameObject)){
                if(!(main.IsClose(other.gameObject))){
                    main.closeContact.Add(other.gameObject);
                }                
            }
        }
        if((infection==2)){
            if(main.IsS(other.gameObject)){
                if(!(main.IsCloseIP(other.gameObject))){
                    main.closeContactIP.Add(other.gameObject);
                }                
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {     
        if((infection==1)||(infection==2)){
            if(main.IsS(other.gameObject)){
                if(!(main.IsClose(other.gameObject))){
                    main.closeContact.Add(other.gameObject);
                }                
            }
        }
        if((infection==2)){
            if(main.IsS(other.gameObject)){
                if(!(main.IsCloseIP(other.gameObject))){
                    main.closeContactIP.Add(other.gameObject);
                }                
            }
        }
    }    
    
}
