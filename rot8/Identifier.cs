using System;
using System.Collections.Generic;

namespace rot1
{
    public class Identifier : Node{
        public Identifier(string _key){
            this.value = _key;
        }
        override public (string,object) Evaluate(SymbolTable symbolTable){
            int index = Writer.varTable[(string)this.value];
            Writer.write($"MOV EBX, [EBP-{index}]");
            return (symbolTable.Get((string)this.value));
        }
    }
}