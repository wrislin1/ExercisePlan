/* 
 Widler Rislin
 12/112019
 CEN 4370C
 Module 8 Assignment
This program reads user infop from an input file
caleed exercise.txt found in the /bin/debug folder 
of my project. Th file contains the user name height
and weight.The program then prompts running
and biking times in minutes,and amounts of  
pushups to be done. The program simulates the 
user exercising,then outputs calories burned to the 
console, further info is found in the output file
ExerciseResult.txt found in the /bin/debug folder
 */


using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercisePlan
{
    public delegate void ExercisePrompt();
    class Exercise  // Exercise class
    {

        string first, last;   //private fields
        double weight,height,bmi, caloriesburned;  

        public string First { set { first = value; } get { return first; } }   // Propertie
        public string Last { set { last = value; } get { return last; } }
        public double Weight { set { weight = value; } get { return weight; } }
        public double Height { set { height = value; } get { return height; } }
        public double Bmi { set { bmi = value; } get { return bmi; } } 
        public double Caloriesburned { set { caloriesburned = value; } get { return caloriesburned; } }

        



        public Exercise(string f="none",string l="none", double w=0,double h=1)  // Exercise constructor
        {
            First = f;
            Last = l;
            Weight = w;
            Height = h;
            Bmi = 703 * Weight / Math.Pow(Height, 2);

        }


        

        
    }

    class Running: Exercise   // Running class inherits from exercise
    {
       public event ExercisePrompt Runpromt;  // running event
        double  time;
        
        public double Time { set { time = value; } get { return time; } }

        public Running(string f,string l,double w,double h, double t):base(f,l,w,h)
        {
           
            Time = t;
            Caloriesburned = Time * (25.0 / 6.0);
            Runpromt += new ExercisePrompt(RunningMessage); // starts event
            Runpromt();
        }

        public void RunningMessage()  // running event method
        {
            Console.WriteLine("{0} {1} is now Running", First, Last);
        }

   

    }

    class PushUps : Exercise // Pushup class inherits from exercise
    {
       public event ExercisePrompt Pushpromt;  // push up event
        double  amount; // fields
        public double Amount { set { amount = value; } get { return amount; } }  // Properties
        public PushUps(string f, string l, double w, double h, double a) : base(f, l, w, h)  // push up constructor
        {
            Amount = a;
            Caloriesburned = Amount * (7.0 / 60.0);
            Pushpromt += new ExercisePrompt(PushupMessage);  // starts event
            Pushpromt();

        }

        public void PushupMessage()  // push up event method
        {
            Console.WriteLine("{0} {1} is now doing Push ups", First, Last);
        }


    }



    class Biking:Exercise  // bike class inherits from exercise
    {
        public event ExercisePrompt Bikepromt; // bike event
        
       double time; // private field
    
        public double Time { set { time = value; } get { return time; } }  // property

        public Biking(string f, string l, double w, double h, double t) : base(f, l, w, h)  // Biking constructor
        {

            Time = t;
            Caloriesburned = Time * 10;
           Bikepromt += new ExercisePrompt(this.BikingMessage);  // starts biking event
            Bikepromt();

        }

        public void BikingMessage()
        {
            Console.WriteLine("{0} {1} is now Biking",First,Last);  //biking event message
          
        }


    }
    class Program
    {
        public  delegate void Exercising(string f, string l);   //exercise delegate
        public static event Exercising Myexercise; //exercise event
      
        static void Main(string[] args)
        {
            double r=0, b=0, p=0;
            bool parse=false;
            string first="", last="";
            double weight = 0, height = 0; 
            string[] profile;

     
            using (StreamReader reader = new StreamReader("exercise.txt"))  // opensamdreads info from exercise.txt
            {
                profile = reader.ReadLine().Split(' '); //spilts line by white space, stored in array profile
                
            }
            
             first = profile[0];                        
             last = profile[1];                                    
             weight = double.Parse(profile[2]);                                          
             height = double.Parse(profile[3]);
                        
            Console.WriteLine("Hello {0} {1}",first,last);  // greets user
            Console.WriteLine();
            while (!parse)
            {
                Console.WriteLine("Please enter how long you want to run in minutes");   // exercise length prompt
                try
                {
                    parse = double.TryParse(Console.ReadLine(), out r);

                    if (!parse)
                        throw new Exception();
                }
                catch
                {
                    Console.WriteLine("Please enter a numerical value");
                }
            }
            parse = false;
            while (!parse)
            {
                Console.WriteLine("Please enter how long you want to bike in minutes");
                try
                {
                    parse = double.TryParse(Console.ReadLine(), out b);

                    if (!parse)
                        throw new Exception();
                }
                catch
                {
                    Console.WriteLine("Please enter a numerical value");
                }
            }
            parse = false;
            while (!parse)
            {
                Console.WriteLine("Please enter how many pushups you want to do");
                try
                {
                    parse = double.TryParse(Console.ReadLine(), out p);

                    if (!parse)
                        throw new Exception();
                }
                catch
                {
                    Console.WriteLine("Please enter a numerical value");
                }
            }
            parse = false;
            Console.WriteLine();
            Myexercise += new Exercising(ExerciseStart);  // begin exercise event
            Myexercise(first, last);
            Myexercise -= new Exercising(ExerciseStart);
            Console.WriteLine();
            Running run = new Running(first, last, weight, height, r);  //starts running exercise
            System.Threading.Thread.Sleep(2000); // pause system for 2 second while exercising
            Biking bike = new Biking(first, last, weight, height, b);    //starts biking exercise
            System.Threading.Thread.Sleep(2000);
            PushUps push = new PushUps(first, last, weight, height, p);   //starts push up exercise
            System.Threading.Thread.Sleep(2000);
            Console.WriteLine();
            Myexercise += new Exercising(ExerciseFinish); // end exercise event
            Myexercise(first, last);
            Myexercise -= new Exercising(ExerciseFinish);
            Console.WriteLine("Estimated calories burned: {0}", Math.Round(run.Caloriesburned + bike.Caloriesburned + push.Caloriesburned,2));
            Console.WriteLine("Full results can be seen in the file ExerciseResults.txt");
            using (StreamWriter writer = new StreamWriter("ExerciseResults.txt")) //writes exercise info to output file
            {
                writer.WriteLine("Exercise Results");
                writer.WriteLine("{0} {1} | Height: {2} | Weight: {3}lbs | BMI: {4}", run.First, run.Last, run.Height, run.Weight, Math.Round(run.Bmi,2));
                writer.WriteLine("Running: Time: {0} Estimated Calories burned: {1}", run.Time,Math.Round(run.Caloriesburned,2));
                writer.WriteLine("Biking: Time: {0} Estimated Calories burned: {1}", bike.Time, Math.Round(bike.Caloriesburned,2));
                writer.WriteLine("Push Ups: Amount done: {0} Estimated Calories burned: {1}", push.Amount, Math.Round(push.Caloriesburned,2));
            }

        }
        
        static void ExerciseStart(string f, string l)
        {
            Console.WriteLine("{0} {1} has started their Workout", f, l);
        }

        static void ExerciseFinish(string f, string l)
        {
            Console.WriteLine("{0} {1} has finished their Workout", f, l);
            
        }

    }
}
