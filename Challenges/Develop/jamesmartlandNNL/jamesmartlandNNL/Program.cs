using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace jamesmartlandNNL
{
    public class Program
    {
        private const string headings = "model,mpg,cyl,disp,hp,drat,wt,qsec,vs,am,gear,carb";
        private static string csvDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName.Replace("\\", "/") + "/mtcars.csv";
        private static List<Car> CarDatabase = new List<Car>(); // List Was chosen for ease due to the smallness of the dataset 
        
        /** This is the main method for the program
         */
        static void Main(string[] args)
        {            
            Initialise();

            Console.WriteLine("\n\nWelcome to Car Management System");
            bool run = true;
            while (run)
            {
                switch (Askint("\nWhat Would you Like to do:\n1> View all Cars\n2> View a Specific Cars Details\n3> Edit a Car\n4> Delete a Car\n5> Add Car\n6> Exit\n>>> "))
                {
                    case 1:
                        // Show Cars
                        ShowCars();
                        break;
                    case 2:
                        Console.WriteLine("\nWhich Car Would you like to View?");
                        int choice = ShowCarsChoice();
                        if (choice == -1) // Check that choice is possible
                        {
                            Console.WriteLine("Choice Selected is out of range");
                            break;
                        }
                        Car c = CarDatabase[choice];
                        string engineType = c.Vs == true ? "Straight Line" : "V-Shaped";
                        Console.WriteLine($"\nModel: {c.Model}\n\tMpg: {c.Mpg}\n\tCylinders: {c.Cyl}\n\tDisplacement: {c.Disp}\n\tHorsepower: {c.Hp}\n\tRear Axle Ratio: {c.Drat}\n\tWeight: {c.Wt}\n\t1/4 Mile Time: {c.Qsec}\n\tEngine Type: {engineType}\n\tIs Manual: {c.Am}\n\tNumber of Gears: {c.Gear}\n\tNumber of Carburetors: {c.Carb}");
                        break;
                    case 3:
                        // Edit A Car
                        EditCar();
                        break;
                    case 4:
                        // Delete A Car
                        int todelete = ShowCarsChoice();
                        if(todelete == -1)
                        {
                            Console.WriteLine("Choice Selected is out of range");
                            break;
                        }
                        CarDatabase.Remove(CarDatabase[todelete]);
                        break;
                    case 5:
                        AddCar();
                        break;
                    default:
                        // Sorting ?
                        int sortdatasetbeforesave = Askint("Would you like to sort the dataset?\n1> Yes\n2> No\n>>> ");
                        switch (sortdatasetbeforesave)
                        {
                            case 1:
                                Console.WriteLine("Sorting Database");
                                SortDatabase();
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine("Writing Database To CSV");
                        WriteToCsv();
                        Console.WriteLine("\nExiting Program\n");
                        run = false;
                        break;
                }
            }
        }

        /** This method loads the csv into memory
         */
        static void Initialise()
        {
            CarDatabase.Clear(); // Delete All From Database
            Console.WriteLine("Loading From CSV... Please Stand By");
            using (var reader = new StreamReader(csvDirectory))
            {
                reader.ReadLine(); // to get rid of headings
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var split = line.Split(',');
                    try
                    {
                        Car tmpCar = new Car(split[0], decimal.Parse(split[1]), int.Parse(split[2]),
                            decimal.Parse(split[3]), decimal.Parse(split[4]), decimal.Parse(split[5]),
                            decimal.Parse(split[6]), decimal.Parse(split[7]), split[8] == "1" ? true : false, split[9] == "1" ? true : false, int.Parse(split[10]), int.Parse(split[11])); ;
                        CarDatabase.Add(tmpCar);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Line: {line} Could Not be converted to: string decimal int decimal decimal decimal decimal decimal bool bool int int");
                    }

                }
            }
        }

        /** This method writes the data to the CSV file
         */
        static void WriteToCsv()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(headings);
            foreach( Car car in CarDatabase)
            {
                sb.AppendLine(car.toCSV());
            }
            File.WriteAllText(csvDirectory, sb.ToString());
        }

        /** This Method shows the cars stored to the command line
         */
        static void ShowCars()
        {
            foreach (Car car in CarDatabase)
            {
                Console.WriteLine($"\t{car.Model}");
            }
        }
        
        /** This method shows the cars stored to the command line along with a number for selection
         */
        static int ShowCarsChoice()
        {
            int i = 0;
            foreach (Car car in CarDatabase)
            {
                Console.WriteLine($"{i}> {car.Model}");
                i++;
            }
            int choice = Askint(">>> ");
            if (choice > i || choice < 0)
            {
                return -1;
            }
            return choice;
        }
        
        /** This method 
         */
        static void EditCar()
        {
            Console.WriteLine("\nWhich Car Would you like to Edit?");

            int tomodel = ShowCarsChoice();
            if (tomodel == -1) // Check that choice is possible
            {
                Console.WriteLine("Choice Selected is out of range");
                return;
            }

            // Which Field to Edit
            switch (Askint($"\n1> Model\n2> Mpg\n3> Cyl\n4> Disp\n5> Hp\n6> Drat\n7> Wt\n8> Qsec\n9> Vs\n10> Am\n11> Gear\n12> Carb\n>>> "))
            {
                case 1:
                    Console.WriteLine($"Current Model: {CarDatabase[tomodel].Model}");
                    CarDatabase[tomodel].Model = Ask("Please Input the new Model Name\n>>> ");
                    break;
                case 2:
                    Console.WriteLine($"Current MPG: {CarDatabase[tomodel].Mpg}");
                    CarDatabase[tomodel].Mpg = Askdecimal("Please Input the new MPG Value\n>>> ");
                    break;
                case 3:
                    Console.WriteLine($"Current Number of Cylinders: {CarDatabase[tomodel].Cyl}");
                    CarDatabase[tomodel].Cyl = Askint("Please Input the new amount of Cylinders\n>>> ");
                    break;
                case 4:
                    Console.WriteLine($"Current Displacement: {CarDatabase[tomodel].Disp}");
                    CarDatabase[tomodel].Disp = Askdecimal("Please Input the new Displacement Value\n>>> ");
                    break;
                case 5:
                    Console.WriteLine($"Current Horsepower: {CarDatabase[tomodel].Hp}");
                    CarDatabase[tomodel].Hp = Askdecimal("Please Input the new Horsepower Value\n>>> ");
                    break;
                case 6:
                    Console.WriteLine($"Current Gear Axel Ratio: {CarDatabase[tomodel].Drat}");
                    CarDatabase[tomodel].Drat = Askdecimal("Please Input the new Gear Axel Ratio Value\n>>> ");
                    break;
                case 7:
                    Console.WriteLine($"Current Weight: {CarDatabase[tomodel].Wt}");
                    CarDatabase[tomodel].Wt = Askdecimal("Please Input the new Weight Value\n>>> ");
                    break;
                case 8:
                    Console.WriteLine($"Current 1/4 Mile Time: {CarDatabase[tomodel].Qsec}");
                    CarDatabase[tomodel].Qsec = Askdecimal("Please Input the new 1/4 Mile Time Value\n>>> ");
                    break;
                case 9:
                    Console.WriteLine($"Engine is Straight Line rather than V-Shaped: {CarDatabase[tomodel].Vs}");
                    CarDatabase[tomodel].Vs = Askbool("Please Input the new Value\n>>> ");
                    break;
                case 10:
                    Console.WriteLine($"Car is Manual: {CarDatabase[tomodel].Am}");
                    CarDatabase[tomodel].Vs = Askbool("Please Input the new Value\n>>> ");
                    break;
                case 11:
                    Console.WriteLine($"Current Number of Gears: {CarDatabase[tomodel].Gear}");
                    CarDatabase[tomodel].Gear = Askint("Please Input the new amount of gears the car has\n>>> ");
                    break;
                case 12:
                    Console.WriteLine($"Current Number of Carburetors: {CarDatabase[tomodel].Carb}");
                    CarDatabase[tomodel].Carb = Askint("Please Input the new amount of Carburetors\n>>> ");
                    break;
                default:
                    Console.WriteLine("That Was not an option, Exiting Menu");
                    return;
            }
        }

        /** This method adds a car to the database 
         */
        static void AddCar()
        {
            CarDatabase.Add( new Car(
                Ask("Please Input the Model Name\n>>> "),
                Askdecimal("Please Input the MPG Value\n>>> "),
                Askint("Please Input the amount of Cylinders\n>>> "),
                Askdecimal("Please Input the Displacement Value\n>>> "),
                Askdecimal("Please Input the Horsepower Value\n>>> "),
                Askdecimal("Please Input the Gear Axel Ratio Value\n>>> "),
                Askdecimal("Please Input the Weight Value\n>>> "),
                Askdecimal("Please Input the 1/4 Mile Time Value\n>>> "),
                Askbool("Is Engine Straight Line ( True / False )\n>>> "),
                Askbool("Is Car Manual ( True / False )\n>>> "),
                Askint("Please Input the amount of gears the car has\n>>> "),
                Askint("Please Input the amount of Carburetors\n>>> ")));
            Console.WriteLine("Car Has Been Added\n>>> ");
        }

        /** This Method is to call the Quicksort Algorithm that will sort the array
         */
        static void SortDatabase()
        {
            QuickSort(ref CarDatabase, 0, CarDatabase.Count-1);
            //ShowCars(); // for testing

            // Commented out sorting method using built in methods
            //CarDatabase.Sort(delegate(Car x, Car y){
            //    return x.Model.CompareTo(y.Model);
            //});
            //ShowCars();
            //printArr(ref vals);
        }

        /** This Method runs the recursive quicksort algorithm
         */
        static void QuickSort(ref List<Car> arr, int low_index, int high_index)
        {
            //Console.WriteLine("\n\nNew Quicksort");
            //printArr(ref arr);
            // setup for quicksort
            bool hasfirstswap = false; // This is so that the pivot isnt changed when a value that is bigger than it is encountered down the array, as this messed up the orders
            int largerfound = 0; // counts the amount of larger values than the pivot that was found within this method

            if (low_index < high_index) // checks if sublist has one element 
            {
                //partitioning of array
                Car pivot_val = arr[high_index]; // to improve this, the pivot value could be set as a random value between low and high
                int pivot_index = high_index;

                for (int j = low_index; j <= high_index; j++) // iterates through array segment set via parameters
                {
                    switch (hasfirstswap) // more efficient than if - determines whether a value larger than the pivot has been found or not.
                    {
                        case false: // if first larger value hasnt been found
                            if(string.Compare(arr[j].Model, pivot_val.Model) >0) // if current value is larger than pivot value
                            {
                                hasfirstswap = true;
                                //Console.WriteLine($"Swapping: {arr[j]} with {arr[pivot_index]}");
                                Swap(ref arr, j, pivot_index);
                                pivot_index = j;
                                largerfound++;
                            }
                            break;
                        case true: // if first larger value has been found
                            if(string.Compare(arr[j].Model, pivot_val.Model) <0) // if current value is smaller than pivot value
                            {
                                //Console.WriteLine($"Swapping: {arr[j]} with {arr[pivot_index]}");
                                Swap(ref arr, j, pivot_index);
                                pivot_index = j;
                            }
                            else // if current value is larger than pivot value
                            {
                                // while loop to bring to back
                                //swap value with high-largerfound
                                // check new value - repeat if larger
                                while (largerfound < high_index-pivot_index) // if amount of larger values that have been found is smaller than the amount of indexes remaining in the array
                                {
                                    //Console.WriteLine("Larger Value than Pivot Found");
                                    //Console.WriteLine($"Swapping: {arr[j]} with {arr[high_index-largerfound]}");
                                    Swap(ref arr, j, high_index - largerfound); // Swap larger value with one of end values of array
                                    if (string.Compare(arr[j].Model, pivot_val.Model) <0) // if new current value is smaller than the pivot value
                                    {
                                        //Console.WriteLine($"Swapping: {arr[j]} with {arr[pivot_index]}");
                                        Swap(ref arr, j, pivot_index); // swap current item with pivot
                                        pivot_index = j;
                                        break;
                                    }
                                    else
                                    {
                                        largerfound++; // increment number of larger values found
                                    }
                                    if (j > high_index - largerfound) // if loop has checked all elements after pivot and found none smaller 
                                    {
                                        //Console.WriteLine("All Elements after pivot are bigger");
                                        break;
                                        j = high_index;
                                    }
                                }
                            }
                            break;
                    }
                    //Console.WriteLine($"Checking index: {j} Value: {arr[j].Model} with Pivot: {pivot_val.Model}");
                }
                QuickSort(ref arr, low_index, pivot_index-1); // Call Quicksort for sub-array smaller than pivot
                QuickSort(ref arr, pivot_index + 1, high_index); // Call Quicksort for sub-array larger than pivot
            } else
            {
                //Console.WriteLine("Skipped");
            }
        }

        /** This method is for swapping values in the array
         */
        static void Swap(ref List<Car> arr, int pos1, int pos2)
        {
            Car copypos1 = arr[pos1];
            arr[pos1] = arr[pos2];
            arr[pos2] = copypos1;
            //printArr(ref arr);
        }

        /** This method retrieves a response that is string
         */
        static string Ask(string message)
        {
            Console.Write(message);
            string resp;
            while (true)
            {
                resp = Console.ReadLine();
                if (resp.Contains(','))
                {
                    Console.WriteLine("Input must not contain ',' Please try again");
                }
                else
                {
                    break;
                }
            }
            return resp;
        }

        /** This method retrieves a response that is an integer
         */
        static public int Askint(string message)
        {
            Console.Write(message);
            var number = 0;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out number))
                {
                    break;
                }
                else
                {
                    Console.Write("Not a number, Please Try Again\n>> ");
                }
            }
            return number;
        }

        /** This method retrieves a response that is an integer
         */
        static public decimal Askdecimal(string message)
        {
            Console.Write(message);
            decimal number;
            while (true)
            {
                if (decimal.TryParse(Console.ReadLine(), out number))
                {
                    break;
                }
                else
                {
                    Console.Write("Not a decimal, Please Try Again\n>> ");
                }
            }
            return number;
        }

        /** This method retrieves a response that is a boolean
         */
        static public bool Askbool(string message)
        {
            Console.Write(message);
            bool resp;
            while (true)
            {
                if (bool.TryParse(Console.ReadLine(), out resp))
                {
                    break;
                }
                else
                {
                    Console.Write("Not a boolean value, Please Try Again\n>> ");
                }
            }
            return resp;
        }
    }
    public class Car
    {
        public string Model { get; set; } // model name
        public decimal Mpg { get; set; } // fuel efficiency 
        public int Cyl { get; set; } // amount of cylinders in car
        public decimal Disp { get; set; } // engine Displacement
        public decimal Hp { get; set; } // Horsepower
        public decimal Drat { get; set; } // gear axle ratio
        public decimal Wt { get; set; } // Weight
        public decimal Qsec { get; set; }// 1/4 mile time
        public bool Vs { get; set; } // V-shape vs Straight Line
        public bool Am { get; set; } // is Manual
        public int Gear { get; set; } // Number of forward gears
        public int Carb { get; set; } // Number of Carburetor barrels

        /** This is a blank constructor
         */
        public Car()
        { }

        /** This is a polymorphic constructor that is used to initialise this object
         */
        public Car(string imodel, decimal impg, int icyl, decimal idisp, decimal ihp,
            decimal idrat, decimal iwt, decimal iqsec, bool ivs, bool iam, int igear, int icarb)
        {
            Model = imodel;
            Mpg = impg;
            Cyl = icyl;
            Disp = idisp;
            Hp = ihp;
            Drat = idrat;
            Wt = iwt;
            Qsec = iqsec;
            Vs = ivs;
            Am = iam;
            Gear = igear;
            Carb = icarb;
        }

        /** This method converts this Car to a csv format
         */
        public string toCSV()
        {
            string stringVs = Vs == true ? "1" : "0";
            string stringAm = Am == true ? "1" : "0";
            return $"{Model},{Mpg},{Cyl},{Disp},{Hp},{Drat},{Wt},{Qsec},{stringVs},{stringAm},{Gear},{Carb}";
        }

    }
}

