using System;
using System.Collections.Generic;

namespace rot1
{
    public class IfNode : Node{

        public IfNode(){
            this.children = new Node[3]{new NoOp(), new NoOp(), new NoOp()};
        }

        override public object Evaluate(SymbolTable symbolTable){
            if((bool)children[0].Evaluate(symbolTable)){
                children[1].Evaluate(symbolTable);
                return(true);
            } else {
                children[2].Evaluate(symbolTable);
                return false;
            }
        }
        
    }
}