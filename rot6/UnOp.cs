using System;
using System.Collections.Generic;

namespace rot1
{
    public class UnOp : Node{
        
        public UnOp(){
            this.children = new Node[1]{new NoOp()};
        }

        override public object Evaluate(SymbolTable symbolTable){
            switch (value){
                case '+':
                    return ( (int)children[0].Evaluate(symbolTable));
                case '-':
                    return (-(int)children[0].Evaluate(symbolTable));
                case '~':
                    return (~(int)children[0].Evaluate(symbolTable));
                case "print":
                    Console.WriteLine(children[0].Evaluate(symbolTable));
                    return null;
                case "input":
                    return int.Parse(Console.ReadLine());
                default:
                    throw new SystemException ($"Invalid Unary Operator ( {value} was received at node on integer operation )");
            }
            
        }
    }
}