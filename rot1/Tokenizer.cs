using System;
using System.Linq;

namespace rot1
{
    public class Tokenizer
    {
        public string origin; //source string
        public int position = 0; //actual token separation pos
        public Token actual; //last parsed token

        public Token SelectNext(){
            Token ret =  new Token();
            string tokenizable = "";
            while(true){
                //If origin got to the end
                if(position >= origin.Length){
                    if(tokenizable.Length > 0){
                        int value;
                        if(int.TryParse(tokenizable, out value)){
                            ret.type = "INT";
                            ret.value = value;
                            actual = ret;
                            return actual;
                        } else {
                            throw new SystemException($"Unexpected EOF could not convert to Integer ( {tokenizable} )");
                        }
                    }
                    ret.type = "EOF";
                    actual = ret;
                    return ret;
                }
                //Handle +|- case
                if("+-".Contains(origin[position])){
                    if(tokenizable.Length == 0){ //token is a sign
                        ret.type = origin[position] == '+' ? "PLUS" : "MINUS";
                        position++;
                        actual = ret;
                        return ret;
                    }
                    //token was a number and ended in a sign (so the sign is not included and the token ended)
                    int value;
                    if(int.TryParse(tokenizable, out value)){
                        ret.type = "INT";
                        ret.value = value;
                        actual = ret;
                        return actual;
                    } else {
                        throw new SystemException($"Unexpected not parseble integer found ( {tokenizable} )");
                    }
                }
                //char is part of a number
                if(int.TryParse(origin[position].ToString(), out int dummy)) { //not +|- and is a number;
                    tokenizable += origin[position];
                    position++;
                }

                else { //if this branch runs, this char is not in my alphabet, so it will be ignored;
                    position++;
                }

            }
        }
    }
}