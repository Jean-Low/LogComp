using System;
using System.Collections.Generic;

namespace rot1
{
    public class IfNode : Node{

        public IfNode(){
            this.children = new Node[3]{new NoOp(), new NoOp(), new NoOp()};
        }

        override public (string,object) Evaluate(SymbolTable symbolTable){

            int myLabelId = Writer.labelIndex;
            Writer.labelIndex ++;

            Writer.write($"LOOP_{myLabelId}:");
            (string, object) condition = children[0].Evaluate(symbolTable);

            Writer.write($"CMP EBX, False");
            if(children[2].children == null){ //check if there is a else
                Writer.write($"JE ELSE_{myLabelId}");
            } else {
                Writer.write($"JE ENDIF_{myLabelId}");
            }

            children[1].Evaluate(symbolTable);
            Writer.write($"JMP ENDIF_{myLabelId}");
            Writer.write($"ELSE_{myLabelId}:");
            children[2].Evaluate(symbolTable);
            Writer.write($"ENDIF_{myLabelId}:");

            return("none",null);
            
        }
        
    }
}