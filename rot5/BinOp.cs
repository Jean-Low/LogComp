using System;
using System.Collections.Generic;

namespace rot1
{
    public class BinOp : Node{

        public BinOp(){
            this.children = new Node[2]{new NoOp(), new NoOp()};
        }

        override public object Evaluate(SymbolTable symbolTable){
            switch ((char) value){
                case '+':
                    return ((int)children[0].Evaluate(symbolTable) + (int)children[1].Evaluate(symbolTable));
                case '-':
                    return ((int)children[0].Evaluate(symbolTable) - (int)children[1].Evaluate(symbolTable));
                case '*':
                    return ((int)children[0].Evaluate(symbolTable) * (int)children[1].Evaluate(symbolTable));
                case '/':
                    return ((int)children[0].Evaluate(symbolTable) / (int)children[1].Evaluate(symbolTable));
                case '=':
                    object val = children[1].Evaluate(symbolTable);
                    symbolTable.Set((string)children[0].value,val);
                    return(null);
                default:
                    throw new SystemException ($"Invalid Binary Operator ( {value} was received at node on integer operation )");
            }
        }
        
    }
}