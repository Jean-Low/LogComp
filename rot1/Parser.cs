using System;

namespace rot1
{
    public static class Parser
    {
        static Tokenizer tokens; //token queue 
        public static int ParseExpression(){

            //Diagram rules
            //1st - num
            //num -> minus | plus
            //num -> EOF
            //minus | plus -> num

            //get actual token;
            //check if follows diagram (num)
            //set var value to token val
            //Tokenizer getNext()
            //while (tokenizer position < tokenizer origin size):
            //    value 
            //    tokenizer getNex

            int expValue = 0;
            bool plus = true;
            bool[] rules = new bool[]{true,false,false}; //can num, can sign, can eof

            while(tokens.SelectNext().type != "EOF"){
                switch(tokens.actual.type){
                    case "INT":
                        if(rules[0]){//INT is allowed
                            expValue += plus ? tokens.actual.value : -tokens.actual.value; //TODO - change line to switch case (support for *,/,^,%, etc...)
                            rules[0] = false; //no number after number
                            rules[1] = true; //can have sign after number
                            rules[2] = true; //can have EOF after number
                        } else {
                            throw new SystemException ($"Invalid expression format. ( {tokens.actual.type} was received at position {tokens.position} when not allowed)");
                        }
                        break;
                    case "MINUS":
                    case "PLUS":
                        if(rules[1]){//SIGN is allowed
                            plus = tokens.actual.type == "PLUS";
                            rules[0] = true; //number required after SIGN
                            rules[1] = false; //no double SIGNs
                            rules[2] = false; //no EOF after SIGN
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
        public static int Run(string code){
            tokens = new Tokenizer();
            tokens.origin = code;
            return(ParseExpression());

            //DEBUg
            /*
            int count = 0;
            tokens.SelectNext();
            while(tokens.actual.type != "EOF"){

                Console.WriteLine("try n " + count);
                Console.WriteLine("Return type was " + tokens.actual.type);
                Console.WriteLine("Return Value was " + tokens.actual.value);
                Console.WriteLine("");

                tokens.SelectNext();
                count ++;
            }
            Console.WriteLine("EOF found");
            */
        }
    }
}