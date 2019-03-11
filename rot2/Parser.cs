using System;
using System.Collections.Generic;


namespace rot1
{
    public static class Parser
    {
        static Tokenizer tokens; //token queue 
        public static int ParseExpression(){

            //Diagram rules
            //1st - term
            //term -> minus | plus
            //term -> EOF
            //minus | plus -> term

            int expValue = 0;
            bool plus = true;
            bool[] rules = new bool[]{true,false,true}; //can term, can sign, can eof
            tokens.SelectNext(); //initialize

            while(tokens.actual.type != "EOF"){
                switch(tokens.actual.type){
                    case "MINUS":
                    case "PLUS":
                        if(rules[1]){//SIGN is allowed
                            plus = tokens.actual.type == "PLUS";
                            tokens.SelectNext();//go to next token
                            rules[0] = true; //number required after SIGN
                            rules[1] = false; //no double SIGNs
                            rules[2] = false; //no EOF after SIGN
                        } else {
                            throw new SystemException ($"Invalid expression format. ( {tokens.actual.type} was received at position {tokens.position} when not allowed)");
                        }
                        break;
                    default:
                        if(rules[0]){//term is allowed
                            expValue += plus ? ParseTerm() : -ParseTerm(); //parseTerm already changes the token
                            rules[0] = false; //no term after term
                            rules[1] = true; //can have sign after number
                            rules[2] = true; //can have EOF after number
                        } else {
                            throw new SystemException ($"Invalid expression format. ( {tokens.actual.type} was received at position {tokens.position} when not allowed)");
                        }
                        break;
                }
            }
            //EOF reached
            if(!rules[2]){ //if EOF was not allowed
                throw new SystemException ($"Invalid expression format. Expression end was unexpected(position {tokens.position})");
            }
            return expValue;
        }

        public static int ParseTerm(){

            //Term rules
            //1st - num
            //num -> times | div
            //num -> exit term
            //times | div -> num

            int expValue = 1;
            bool times = true;
            bool[] rules = new bool[]{true,false,false}; //can num, can sign, can exit term

            //list to check if token is valid in a term
            List<String> allowedTokens = new List<String>(){"INT","TIMES","DIV"};
            //check if token type is allowed
            while(allowedTokens.Contains(tokens.actual.type)){
                switch(tokens.actual.type){
                    case "INT":
                        if(rules[0]){//INT is allowed
                            expValue = times ? (expValue * tokens.actual.value) : (expValue / tokens.actual.value); 
                            rules[0] = false; //no number after number
                            rules[1] = true; //can have sign after number
                            rules[2] = true; //can exit the term after number
                        } else {
                            throw new SystemException ($"Invalid expression format. ( {tokens.actual.type} was received at position {tokens.position} when not allowed)");
                        }
                        break;
                    case "DIV":
                    case "TIMES":
                        if(rules[1]){//SIGN is allowed
                            times = tokens.actual.type == "TIMES";
                            rules[0] = true; //number required after SIGN
                            rules[1] = false; //no double SIGNs
                            rules[2] = false; //no exit after SIGN
                        } else {
                            throw new SystemException ($"Invalid expression format. ( {tokens.actual.type} was received at position {tokens.position} when not allowed)");
                        }
                        break;
                }
                tokens.SelectNext();
            }
            //End of term reached
            if(!rules[2]){ //if exiting was not allowed
                throw new SystemException ($"Invalid expression format. Expression end was unexpected(position {tokens.position})");
            }
            return expValue;
        }
        public static int Run(string code){
            tokens = new Tokenizer();
            tokens.origin = code;
            return(ParseExpression());
        }
    }
}