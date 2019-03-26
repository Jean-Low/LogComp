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
            //Console.WriteLine("next token");
            Token ret =  new Token();
            string tokenizable = "";
            while(true){
                //If origin got to the end or comment starter (') detected
                if(position >= origin.Length || origin[position] == '\''){
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
                //Handle sign(+-*/) case
                if("+-*/()".Contains(origin[position])){
                    if(tokenizable.Length == 0){ //token is a sign or simbol
                        switch(origin[position]){
                            case '+':
                                ret.type = "PLUS";
                                break;
                            case '-':
                                ret.type = "MINUS";
                                break;
                            case '*':
                                ret.type = "TIMES";
                                break;
                            case '/':
                                ret.type = "DIV";
                                break;
                            case '(':
                                ret.type = "POPEN";
                                break;
                            case ')':
                                ret.type = "PCLOSE";
                                break;
                        }
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
                //char is a space (ignore spaces)
                else if(origin[position] == ' ' || origin[position] == '\t'){
                    position++;
                }

                //char is invalid
                else { 
                    throw new SystemException($"Unexpected token found, use ' (single quote) for comments. ( {origin[position]} )");
                    
                }
                /* (V1.1.1) (Changed to last two if statements. Now throws an error if invalid char to showcase comment feature better)
                else { //if this branch runs, this char is not in my alphabet, so it will be ignored;
                    position++;
                }
                */

            }
        }
    }
}