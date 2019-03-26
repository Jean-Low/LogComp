using System;
using System.Collections.Generic;


namespace rot1
{
    public static class Parser
    {
        static Tokenizer tokens; //token queue 
        public static Node ParseExpression(){
            //Console.WriteLine("parsing expression");
            //Diagram rules
            //1st - term
            //term -> minus | plus
            //term -> EOF
            //minus | plus -> term
            
            Node root = ParseTerm();

            //list to check if token is valid in a term
            List<String> allowedTokens = new List<String>(){"PLUS","MINUS"};

            while(allowedTokens.Contains(tokens.actual.type)){

                //make a new node, current root is the new node left child
                Node temp = root;
                root = new BinOp();
                root.children[0] = temp;
                
                root.value = (tokens.actual.type == "MINUS") ? '-' : '+';
                tokens.SelectNext();
                root.children[1] = ParseTerm();

            }

            return root;
        }

        public static Node ParseTerm(){

            //Console.WriteLine("Parsing term");
            //Term rules
            //1st - factor
            //factor -> times | div
            //factor -> exit term
            //times | div -> factor

            //Get first factor
            Node root = ParseFactor();
            
            //list to check if token is valid in a term
            List<String> allowedTokens = new List<String>(){"TIMES","DIV"};
            //check if token type is allowed
            while(allowedTokens.Contains(tokens.actual.type)){
                //make a new node, current root is the new node left child
                Node temp = root;
                root = new BinOp();
                root.children[0] = temp;
                
                root.value = (tokens.actual.type == "TIMES") ? '*' : '/';
                tokens.SelectNext();
                root.children[1] = ParseFactor();
            }
            return root;
        }

        public static Node ParseFactor(){
            
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

            Node root;

            switch(tokens.actual.type){
                    case "INT":
                        root = new IntVal();
                        root.value = tokens.actual.value;
                        tokens.SelectNext();
                        return(root);
                    case "PLUS":
                    case "MINUS":
                        root = new UnOp();
                        root.value = tokens.actual.type == "PLUS" ? '+': '-';
                        tokens.SelectNext();
                        root.children[0] = ParseFactor();
                        return root;
                    case "POPEN":
                        tokens.SelectNext();
                        root = ParseExpression();
                        if(tokens.actual.type != "PCLOSE"){
                            throw new SystemException ($"Missing closing parentesis (position {tokens.position})");
                        }
                        tokens.SelectNext();
                        return root;
                }

            //End of term reached, but exiting was not allowed
            throw new SystemException ($"Invalid expression format. Expression end was unexpected(position {tokens.position})");
        }
        public static int Run(string code){
            tokens = new Tokenizer();
            tokens.origin = code;
            tokens.SelectNext(); //initialize a token
            Node root = ParseExpression();
            return( (int) root.Evaluate());
        }
    }
}