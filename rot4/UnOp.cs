using System;
using System.Collections.Generic;

namespace rot1
{
    public class UnOp : Node{
        
        public UnOp(){
            this.children = new Node[1]{new NoOp()};
        }

        override public object Evaluate(){
            switch (value){
                case '+':
                    return ( (int)children[0].Evaluate());
                case '-':
                    return (-(int)children[0].Evaluate());
                case '~':
                    return (~(int)children[0].Evaluate());
                default:
                    throw new SystemException ($"Invalid Unary Operator ( {value} was received at node on integer operation )");
            }
            
        }
    }
}