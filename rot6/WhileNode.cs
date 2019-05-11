using System;
using System.Collections.Generic;

namespace rot1
{
    public class WhileNode : Node{

        public WhileNode(){
            this.children = new Node[2]{new NoOp(), new NoOp()};
        }

        override public object Evaluate(SymbolTable symbolTable){
            while((bool) children[0].Evaluate(symbolTable)){
                children[1].Evaluate(symbolTable);
            }
            return null;
        }
        
    }
}