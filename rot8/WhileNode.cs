using System;
using System.Collections.Generic;

namespace rot1
{
    public class WhileNode : Node{

        public WhileNode(){
            this.children = new Node[2]{new NoOp(), new NoOp()};
        }

        override public (string,object) Evaluate(SymbolTable symbolTable){
            int myLabelId = Writer.labelIndex;
            Writer.labelIndex ++;

            Writer.write($"LOOP_{myLabelId}:");

            (string,object) ret = children[0].Evaluate(symbolTable);
            God.VerifyType("boolean",ret);

            Writer.write($"CMP EBX, False");
            Writer.write($"JE EXIT_{myLabelId}");
            
            children[1].Evaluate(symbolTable);

            Writer.write($"JMP LOOP_{myLabelId}");
            Writer.write($"EXIT_{myLabelId}:");


            /*
            while(whileCheck(symbolTable)){
                children[1].Evaluate(symbolTable);
            }
            */
            return ("none",null);
        }

        bool whileCheck (SymbolTable symbolTable) {
            (string,object) ret = children[0].Evaluate(symbolTable);
            God.VerifyType("boolean",ret);
            return (bool) ret.Item2;
        }

    }
}