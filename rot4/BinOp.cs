using System;
using System.Collections.Generic;

namespace rot1
{
    public class BinOp : Node{

        public BinOp(){
            this.children = new Node[2]{new NoOp(), new NoOp()};
        }

        override public object Evaluate(){
            switch ((char) value){
                case '+':
                    return ((int)children[0].Evaluate() + (int)children[1].Evaluate());
                case '-':
                    return ((int)children[0].Evaluate() - (int)children[1].Evaluate());
                case '*':
                    return ((int)children[0].Evaluate() * (int)children[1].Evaluate());
                case '/':
                    return ((int)children[0].Evaluate() / (int)children[1].Evaluate());
                default:
                    throw new SystemException ($"Invalid Binary Operator ( {value} was received at node on integer operation )");
            }
        }
        
    }
}