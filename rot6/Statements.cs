using System;
using System.Collections.Generic;

namespace rot1
{
    public class Statements : Node{
        public LinkedList<Node> StatementList = new LinkedList<Node>();
        
        override public object Evaluate(SymbolTable symbolTable){

            foreach(Node statement in StatementList){
                
                statement.Evaluate(symbolTable);
            }
            return(1);

        }

        public void Add(Node node){
            StatementList.AddLast(node);
        }
        
    }
}