using System;
using System.Collections.Generic;

namespace rot1
{
    public class BinOp : Node{

        public BinOp(){
            this.children = new Node[2]{new NoOp(), new NoOp()};
        }

        override public (string,object) Evaluate(SymbolTable symbolTable){

            if (value.ToString() == "="){
                (string, object) assign = children[1].Evaluate(symbolTable);
                Writer.assign(children[0].value.ToString());
                return ("none",null);
            }

            if (value.ToString() ==  "vardec") {
    //          get value of type node and interact with ident node to sabe the var type
                string type = (string)children[1].value;
                string key = (string)children[0].value;

                symbolTable.Set(key,null,type);
                Writer.declare(key);

                return ("none",null);
            }
                    

            (string, object) ret1 = children[0].Evaluate(symbolTable);
            Writer.write("PUSH EBX");
            (string, object) ret2 = children[1].Evaluate(symbolTable);
            Writer.write("POP EAX");

            switch ( value.ToString() ){
                case "+":
                    God.VerifyType("integer",ret1);
                    God.VerifyType("integer",ret2);
                    Writer.write("ADD EAX, EBX");
                    Writer.write("MOV EBX, EAX");
                    return ("integer", null);
                case "-":
                    God.VerifyType("integer",ret1);
                    God.VerifyType("integer",ret2);
                    Writer.write("SUB EAX, EBX");
                    Writer.write("MOV EBX, EAX");
                    return ("integer", null);
                case "*":
                    God.VerifyType("integer",ret1);
                    God.VerifyType("integer",ret2);
                    Writer.write("IMUL EBX");
                    Writer.write("MOV EBX, EAX");
                    return ("integer", null);
                case "/":
                    God.VerifyType("integer",ret1);
                    God.VerifyType("integer",ret2);
                    Writer.write("IDIV EBX");
                    Writer.write("MOV EBX, EAX");
                    return ("integer", null);
                case ">":
                    God.VerifyType("integer",ret1); 
                    God.VerifyType("integer",ret2);
                    Writer.write("CMP EAX, EBX");
                    Writer.write("CALL binop_jg");
                    return ("boolean", null);
                case "<":
                    God.VerifyType("integer",ret1);
                    God.VerifyType("integer",ret2);
                    Writer.write("CMP EAX, EBX");
                    Writer.write("CALL binop_jl");
                    return ("boolean", null);
                case "&":
                    God.VerifyType("boolean",ret1);
                    God.VerifyType("boolean",ret2);
                    Writer.write("AND EAX, EBX");
                    return ("boolean", null);
                case "|":
                    God.VerifyType("boolean",ret1);
                    God.VerifyType("boolean",ret2);
                    Writer.write("OR EAX, EBX");
                    return ("boolean", null);
                case "==":
                    if(ret1.Item1 != ret2.Item1){
                        throw new SystemException ($"Invalid relative operation == between variables of types {ret1.Item1} and {ret2.Item1}.");
                    }
                    Writer.write("CMP EAX, EBX");
                    Writer.write("CALL binop_je");
                    switch(ret1.Item1){
                        case "boolean":
                            return ("boolean", null);
                        case "integer":
                            return ("boolean", null);
                        default:
                            throw new SystemException ($"Invalid variable type for == operation. {ret1.Item1}.");
                    }
                    
                default:
                    throw new SystemException ($"Invalid Binary Operator ( {value} was received at node on integer operation )");
            }
        }
        
    }
}