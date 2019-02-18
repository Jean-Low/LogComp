using System;

public class Step{
	public int val;
	public bool positive;
}

public class Program
{
	
	private static char reduceSignal(char sigA, char sigB){ //Reduces double operators to single
		if(sigA == sigB){
			return '+';	
		}
		return '-';
	}
	
	public static void Main()
	{
		//This string is treated as an Input string, change it for testing... (default: "-23  --+ +   5    - 2 ")
		String command = "-23  --+ +   5    - 2 ";
		
		//Removing spaces
		String comA = "";
		for(int i = 0; i < command.Length; i++){
			if(command[i] != ' '){
				comA += command[i];
			}
		}
		
		//Reducing Signals
		String comB = "";
		bool lastWasSignal = false;
		for(int i = 0; i < comA.Length; i++){
			if(comA[i] == '+' || comA[i] == '-'){
				if(lastWasSignal){
					comB = comB.Remove(comB.Length -1,1) + reduceSignal(comB[comB.Length -1],comA[i]);
				} else {
					comB += comA[i];
					lastWasSignal = true;
				}
			
			} else {
				lastWasSignal = false;
				comB += comA[i];
			}
		}
		
		//Adding to Queue
		Step[] queue = new Step[comB.Length]; //The size is surelly greater than needed, still bater than "maybe it is enough".
		int start = 0;
		int currentQ = 0;
		if(comB[start] == '-' || comB[start] == '+'){ //handle first number is negative or hard setted with a + 
			comB = '0' + comB;
		}
		
		Console.WriteLine("Final filtered command: " + comB);
		String currentNumber = "";
		for(int i = start; i < comB.Length; i++){
			if(comB[i] == '+' || comB[i] == '-'){
				queue[currentQ] = new Step();
				queue[currentQ].positive = (comB[i] == '+');
				queue[currentQ].val = int.Parse(currentNumber);
				currentQ++;
				currentNumber = "";
			} else {
				currentNumber += comB[i];
			}
		}
		
		queue[currentQ] = new Step();
		queue[currentQ].val = int.Parse(currentNumber);
		
		//Resolve Queue
		bool sum = true;
		int val = 0;
		for(int i = 0; i < queue.Length; i++){
			if(queue[i] != null){
				
				val += (sum ? queue[i].val : -queue[i].val);
				sum = queue[i].positive;
				Console.WriteLine("\noperation " + i);
				Console.WriteLine("current value " + val);
			}
		}
		
		//Result
		Console.WriteLine("\nFinal Value: " + val);
	}
}
