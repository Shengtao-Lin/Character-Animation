using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;
using RootMotion.FinalIK;
using System;
using UnityEngine.UI;

public class StoryBT : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject daniel, daniel_green, daniel_blue, daniel_player;

    private BehaviorAgent daniel_agent, danielGreen_agent, danielBlue_agent, daniel_player_agent;

    public Transform wanderPt1, wanderPt2, wanderPt3, blueDanielGo, foo, foo2, location1,location2,location3;

    public Button moveButton;


 
    // IK Initializations
    public GameObject ball;
    public InteractionObject ikBall;
    public FullBodyBipedEffector hand;

    // test IK area

    public GameObject testDaniel1,testDaniel2;
    public InteractionObject ikHandshake_1,ikHandshake_2;
    public FullBodyBipedEffector daniel_1_Hand, daniel_2_Hand;

    private BehaviorAgent testDan1_agent, testDan2_agent;

    private bool butU;


    void Start()
    {
        //daniel_agent = new BehaviorAgent (this.BuildTreeRoot_daniel ());
        daniel_agent = new BehaviorAgent (BuildTreeRoot_daniel());
		BehaviorManager.Instance.Register (daniel_agent);
		daniel_agent.StartBehavior ();

        danielGreen_agent = new BehaviorAgent (this.BuildTreeRoot_danielGreen ());
		BehaviorManager.Instance.Register (danielGreen_agent);
		danielGreen_agent.StartBehavior ();
        
        //danielBlue_agent = new BehaviorAgent (this.BuildTreeRoot_danielBlue ());
        danielBlue_agent = new BehaviorAgent (BuildTreeRoot_danielBlue());
		BehaviorManager.Instance.Register (danielBlue_agent);
		danielBlue_agent.StartBehavior ();

        testDan1_agent = new BehaviorAgent (this.buildTreeRoot_testDaniel1());
		BehaviorManager.Instance.Register (testDan1_agent);
		testDan1_agent.StartBehavior ();

        
        testDan2_agent = new BehaviorAgent (this.buildTreeRoot_testDaniel2());
		BehaviorManager.Instance.Register (testDan2_agent);
		testDan2_agent.StartBehavior ();

        daniel_player_agent = new BehaviorAgent (this.buildTreeRoot_player());
		BehaviorManager.Instance.Register (daniel_player_agent);
		daniel_player_agent.StartBehavior ();

        moveButton.onClick.AddListener(buttonP);

    }

    // Update is called once per frame
    void Update()
    {
 
    }


    private void buttonP(){
        butU = true;
    }

   

    #region Arcs
    protected Node WanderArc(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(daniel_green.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node LookArc(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(daniel.GetComponent<BehaviorMecanim>().Node_HeadLook(position), new LeafWait(1000));
    }

    protected Node WanderArc_BlueDaniel(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(daniel_blue.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    protected Node WanderArc_player(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(daniel_player.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }


    protected Node WanderArc_daniel(Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(daniel.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(1000));
    }

    #endregion

    #region IKArcs
    protected Node PickUp(GameObject p)
    {
        return new Sequence(this.Node_BallStop(),
                            p.GetComponent<BehaviorMecanim>().Node_StartInteraction(hand, ikBall),
                            new LeafWait(1000),
                            p.GetComponent<BehaviorMecanim>().Node_StopInteraction(hand));
    }

    public Node Node_BallStop()
    {
        return new LeafInvoke(() => this.BallStop());
    }
    public virtual RunStatus BallStop()
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;

        return RunStatus.Success;
    }

    protected Node PutDown(GameObject p)
    {
        return new Sequence(p.GetComponent<BehaviorMecanim>().Node_StartInteraction(hand, ikBall),
                            new LeafWait(300),
                            this.Node_BallMotion(),
                            new LeafWait(500), p.GetComponent<BehaviorMecanim>().Node_StopInteraction(hand));
    }

    public Node Node_BallMotion()
    {
        return new LeafInvoke(() => this.BallMotion());
    }

    public virtual RunStatus BallMotion()
    {
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        ball.transform.parent = null;
        return RunStatus.Success;
    }

    public Node test_Node()
    {
        return new LeafInvoke(() => this.testAnim());
    }

    public Node test_Node2()
    {
        return new LeafInvoke(() => this.testAnim2());
    }
    public virtual RunStatus testAnim()
    {
        Animator anim = daniel_blue.GetComponent<Animator>();
        anim.SetBool("B_Breakdance",true);

        return RunStatus.Success;
    }

    public virtual RunStatus testAnim2()
    {
        Animator anim = daniel_blue.GetComponent<Animator>();
        anim.SetBool("B_Breakdance",false);

        return RunStatus.Success;
    }
    protected Node testArc(GameObject p)
    {
        return new Sequence(test_Node(),
                            new LeafWait(5000),
                            test_Node2(),
                            new LeafWait(5000)
        );
    }


    #endregion


    #region testArcs
    protected Node shakeHandsArcDaniel1()
    {
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);
        return new Sequence(
            testDaniel1.GetComponent<BehaviorMecanim>().Node_HeadLookTurnFirst(testDaniel2.GetComponent<Transform>().position + height),
            //new LeafWait(3000),
            testDaniel1.GetComponent<BehaviorMecanim>().Node_StartInteraction(daniel_1_Hand,ikHandshake_1),
            new LeafWait(3000),
            testDaniel1.GetComponent<BehaviorMecanim>().Node_StopInteraction(daniel_1_Hand)
        );
    }

    protected Node shakeHandsArcDaniel2()
    {
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);
        return new Sequence(
            testDaniel2.GetComponent<BehaviorMecanim>().Node_HeadLookTurnFirst(testDaniel1.GetComponent<Transform>().position + height),
            //new LeafWait(3000),
            testDaniel2.GetComponent<BehaviorMecanim>().Node_StartInteraction(daniel_2_Hand,ikHandshake_2),
            new LeafWait(2000),
            testDaniel2.GetComponent<BehaviorMecanim>().Node_StopInteraction(daniel_2_Hand)
        );
    }

    protected Node buildTreeRoot_testDaniel1()
    {
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);
        Func<bool> act = () => (Vector3.Distance(daniel_player.GetComponent<Transform>().position,testDaniel1.GetComponent<Transform>().position) < 3f);
        Node waver = 
       
        new Sequence(
                    shakeHandsArcDaniel1(),
                    testDaniel1.GetComponent<BehaviorMecanim>().Node_HeadLookTurnFirst(location2.position + height),
                    testDaniel1.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("wave",1000)
                    
                    );
        Node trigger = new DecoratorLoop (new LeafAssert (act));
        Node root = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, waver)));
        return root;
    }

    protected Node buildTreeRoot_testDaniel2()
    {
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);
        Func<bool> act = () => (Vector3.Distance(daniel_player.GetComponent<Transform>().position,testDaniel1.GetComponent<Transform>().position) < 3f);
        Node waver = 
        
        new Sequence(
                    shakeHandsArcDaniel2(),
                    testDaniel2.GetComponent<BehaviorMecanim>().Node_HeadLookTurnFirst(location2.position + height),
                    testDaniel2.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("wave",1000)
                    
                    );
        Node trigger = new DecoratorLoop (new LeafAssert (act));
        Node root = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, waver)));
        return root;
    }

  

    #endregion

    #region RootNodes
/*
     protected Node buildTreeRoot_testDaniel()
    {
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);
        Func<bool> act = () => (Vector3.Distance(daniel_player.GetComponent<Transform>().position,testDaniel1.GetComponent<Transform>().position) < 5f);
        Node waver = 
        new SequenceParallel(
            buildTreeRoot_testDaniel1(),
            buildTreeRoot_testDaniel2()
            );
        Node trigger = new DecoratorLoop (new LeafAssert (act));
        Node root = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, waver)));
        return root;

    }*/
    protected Node BuildTreeRoot_daniel()
	{
        Func<bool> act = () => (Vector3.Distance(daniel.GetComponent<Transform>().position,daniel_blue.GetComponent<Transform>().position) < 5f);
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);

		Node lookEachOther = 
        new DecoratorLoop(
        new Sequence(
            daniel.GetComponent<BehaviorMecanim>().Node_HeadLookTurnFirst(daniel_blue.GetComponent<Transform>().position + height),
            
            daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("shock",3000),
            daniel.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("cry",3000)
            //daniel_blue.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("cry",3000)
        ));

        Node trigger = new DecoratorLoop (new LeafAssert (act));

		Node root = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, lookEachOther)));

		return root;
	}

    
    protected Node BuildTreeRoot_danielGreen()
	{
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);
		Node roaming = 
        new DecoratorLoop (
            new Sequence(
						this.WanderArc(this.wanderPt1),
                        daniel_green.GetComponent<BehaviorMecanim>().Node_HeadLookTurnFirst(location2.position + height),
						this.WanderArc(this.wanderPt2),
						this.WanderArc(this.wanderPt3)
                    )
        );
                        
		return roaming;
	}

    protected Node BuildTreeRoot_danielBlue()
    {
        Vector3 daniel_pos = daniel.GetComponent<Transform>().position;
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);

        Node roaming = new Sequence(
                        this.WanderArc_BlueDaniel(this.blueDanielGo),
                        //this.testArc(daniel_blue),
                        
                        this.PickUp(daniel_blue),
                        this.WanderArc_BlueDaniel(foo),
                        this.PutDown(daniel_blue),
                        daniel_blue.GetComponent<BehaviorMecanim>().Node_HeadLook(daniel_pos+height),
                        
                        new DecoratorLoop(
                            new Sequence(
                                daniel_blue.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("PISTOLAIM",1000),
                                daniel_blue.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CUTTHROAT",1000)
                                ))
                        
                        );
                        
                        
                        
        return roaming;
        
    }

    protected Node buildTreeRoot_player(){
        Vector3 height = new Vector3(0.0f, 1.85f, 0.0f);
        //yield return new WaitUntil( butU|butD);
        Func<bool> actC = () => (butU);
        Node move= new Sequence( 
                this.WanderArc_player(location1),
                daniel_player.GetComponent<BehaviorMecanim>().Node_HeadLookTurnFirst(daniel.GetComponent<Transform>().position + height),
                new SequenceN(
                    2,
                    daniel_player.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("CUTTHROAT",2000),
                    daniel_player.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("FIGHT",3000),
                    daniel_player.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("PISTOLAIM",1000)
                    )
                );
        Node move2= new Sequence( 
                this.WanderArc_player(location2),
                daniel_player.GetComponent<BehaviorMecanim>().Node_HeadLookTurnFirst(testDaniel2.GetComponent<Transform>().position + height),
                daniel_player.GetComponent<BehaviorMecanim>().ST_PlayHandGesture("wave",3000));
        Node move1= new Sequence( 
                this.WanderArc_player(location3),
                daniel_player.GetComponent<BehaviorMecanim>().ST_PlayBodyGesture("TALKING ON PHONE",13000));


        Node rootmove = new SelectorShuffle(
            move,
            move1,
            move2
        );


        Node trigger = new DecoratorLoop (new LeafAssert (actC));
		Node root = new DecoratorLoop (new DecoratorForceStatus (RunStatus.Success, new SequenceParallel(trigger, rootmove)));
        return root;

        
    }


    #endregion
}
