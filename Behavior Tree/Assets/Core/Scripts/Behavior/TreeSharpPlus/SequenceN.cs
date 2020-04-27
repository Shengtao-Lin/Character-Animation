using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace TreeSharpPlus
{
    public class SequenceN : NodeGroupWeighted
    {
        
        public int num { get; set; }

        public SequenceN(int n,params Node[] children)
            : base(children)
        {
            this.num=n;
        }

        public override void Start()
        {
            base.Start();
            this.Shuffle();
        }

        public override IEnumerable<RunStatus> Execute()
        {

            for(int i = 0; i < this.num; i++)
            {
                Node node=this.Children[i];
                this.Selection = node;
                node.Start();


                RunStatus result;
                while ((result = this.TickNode(node)) == RunStatus.Running)
                    yield return RunStatus.Running;

                node.Stop();

                this.Selection.ClearLastStatus();
                this.Selection = null;

                if (result == RunStatus.Failure)
                {
                    yield return RunStatus.Failure;
                    yield break;
                }

                yield return RunStatus.Running;
            }
            yield return RunStatus.Success;
            yield break;
        }
    }
}

        