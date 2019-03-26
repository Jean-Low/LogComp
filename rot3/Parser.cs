using System;
using System.Collections.Generic;


namespace rot1
{
    public static class Parser
    {
        static Tokenizer tokens; //token queue 
        public static int ParseExpression(){
            //Console.WriteLine("parsing expression");
            //Diagram rules
            //1st - term
            //term -> minus | plus
            //term -> EOF
            //minus | plus -> term
            
            tokens.SelectNext(); //initialize
            int expValue = ParseTerm();
            bool plus = true;
            bool[] rules = new bool[]{false,true,true}; //can term, can sign, can eof
            

            //list to check if token is valid in a term
            List<String> allowedTokens = new List<String>(){"PLUS","MINUS", "INT"};
            
            while(allowedTokens.Contains(tokens.actual.type)){
                //Console.WriteLine($"exp token:{tokens.actual.value}");
                switch(tokens.actual.type){
                    case "MINUS":
                    case "PLUS":
                        //Console.WriteLine("Got a SIGN");
                        if(rules[1]){//SIGN is allowed
                            plus = tokens.actual.type == "PLUS";
                            tokens.SelectNext();//go to next token
                            rules[0] = true; //term required after SIGN
                            rules[1] = false; //no double SIGNs
                            rules[2] = false; //no EOF after SIGN
                        } else {
                            throw new SystemException ($"Invalid expression format. ( {tokens.actual.type} was received at position {tokens.position} when not allowed)");
                        }
                        break;
                    default:
                        //Console.WriteLine("GOT A NOT SIGN");
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
            //End of expression reached
            if(!rules[2]){ //if EOE was not allowed
                throw new SystemException ($"Invalid expression format. Expression end was unexpected(position {tokens.position})");
            }
            return expValue;
        }

        public static int ParseTerm(){

            //Console.WriteLine("Parsing term");
            //Term rules
            //1st - factor
            //factor -> times | div
            //factor -> exit term
            //times | div -> factor

            //Get first factor
            int termValue = ParseFactor();//first factor
            bool times = true;
            

            //list to check if token is valid in a term
            List<String> allowedTokens = new List<String>(){"TIMES","DIV"};
            //check if token type is allowed
            while(allowedTokens.Contains(tokens.actual.type)){
                //Console.WriteLine($"term token:{tokens.actual.value}");
                times = tokens.actual.type == "TIMES";
                tokens.SelectNext();
                termValue = times ? (termValue * ParseFactor()) : (termValue / ParseFactor());
                
                
            }
            return termValue;
        }

        public static int ParseFactor(){
            
            //Console.WriteLine("Parsing Factor");

            //Factor rules
            //1st -> num
            //1st -> +
            //1st -> -
            //1st -> (
            //+ -> factor
            //- -> factor
            //( -> expression
            //expression -> )

            bool negative = false; //switch every - sign and reset to false after a number or exp

            //list to check if token is valid in a term
            List<String> allowedTokens = new List<String>(){"INT","MINUS","PLUS","POPEN"};
            //check if token type is allowed
            while(allowedTokens.Contains(tokens.actual.type)){
                //Console.WriteLine($"factor token:{tokens.actual.value}");
                switch(tokens.actual.type){
                    case "INT":
                        int factorVal = tokens.actual.value;
                        tokens.SelectNext();
                        negative = false;
                        return( negative ? (-factorVal) : (factorVal));
                    case "PLUS":
                    case "MINUS":
                        if(tokens.actual.type == "MINUS")
                                negative = !negative;
                        break;
                    case "POPEN":
                        int expValue = ParseExpression();
                        if(tokens.actual.type != "PCLOSE"){
                            throw new SystemException ($"Missing closing parentesis (position {tokens.position})");
                        }
                        tokens.SelectNext();
                        return (negative ? -expValue : expValue);
                }
                tokens.SelectNext();
            }
            //End of term reached
            //if exiting was not allowed
            throw new SystemException ($"Invalid expression format. Expression end was unexpected(position {tokens.position})");
        }
        public static int Run(string code){
            tokens = new Tokenizer();
            tokens.origin = code;
            return(ParseExpression());
        }
    }
}