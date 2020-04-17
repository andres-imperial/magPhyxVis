using System;
using System.Numerics;
using HilbertTransformation;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.IO;


namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {   



            makeNewCommandFiles(1);

            // int[] coords1 = new int[]{0,0,0,1,0};
            // int[] coords2 = new int[]{0,1,0,0,0};
            // int[] coords3 = new int[]{0,3,0,0,0};
            // int bpd = 2;

            // var hIndex1 = new HilbertPoint(coords1, bpd);
            // var hIndex2 = new HilbertPoint(coords2, bpd);
            // var hIndex3 = new HilbertPoint(coords3, bpd);

            // Console.WriteLine(hIndex1.HilbertIndex);
            // Console.WriteLine(hIndex2.HilbertIndex);
            // Console.WriteLine(hIndex3.HilbertIndex);



            

            



        }

        public static List<Tuple<string, BigInteger, int>> makeSortedTuples(int folderNumber){
                
            List<Tuple<string,BigInteger,int>> commandLines_HI_index = new List<Tuple<string, BigInteger,int>>();
            for (int i=0; i<100; i++){
                string index;
                if (i<=9){
                    index = String.Format("0{0}", i);
                }else{index = i.ToString();}
                string address_i = String.Format(@"..\data\data{0}\commands\commands{1}.csv", folderNumber, index);
                string[] command_line_i = readCommandFile(address_i);
                commandLines_HI_index.Add(commandLine_HI_initIndex(command_line_i, i));
                
            }
            commandLines_HI_index.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            return commandLines_HI_index;
        }

        public static void makeNewCommandFiles(int folderNumber){
            List<Tuple<string,BigInteger,int>> tuples = makeSortedTuples(folderNumber);

            for (int i=0; i<tuples.Count; i++){
                string index;
                if (i<=9){
                    index = String.Format("0{0}", i);
                }else{index = i.ToString();}
                string address = String.Format(@"..\data\data{0}\commands\hilbert_sorted_commands{1}.csv",folderNumber, index);               
                string[] data = new string[1];
                data[0] = tuples[i].Item1;
                System.IO.File.WriteAllLines(address, data);
            }
            correlateEventsAndCommands(tuples, folderNumber);
            Console.WriteLine("finished dataset " + folderNumber);

        }

        public static void correlateEventsAndCommands(List<Tuple<string, BigInteger, int>> tuples, int folderNumber){
            for (int i=0; i<tuples.Count; i++){
                string index;
                string tuplesIndex;
                if (i<=9){
                    index = String.Format("0{0}", i);
                }else{index = i.ToString();}
                if (tuples[i].Item3 <= 9){
                    tuplesIndex = String.Format("0{0}", tuples[i].Item3);
                }else{tuplesIndex = tuples[i].Item3.ToString();}
            string eventAddress = String.Format(@"..\data\data{0}\events\events{1}.csv", folderNumber, index);
            string newEventAddress = String.Format(@"..\data\data{0}\events\hilbert_sorted_events{1}.csv", folderNumber, tuplesIndex); 
            string[] data = System.IO.File.ReadAllLines(eventAddress);
            System.IO.File.WriteAllLines(newEventAddress, data);
            }
        }
        
        public static Tuple<string, BigInteger, int> commandLine_HI_initIndex(string[] command_line, int index){
            int bpd = FindBitsPerDimension(10);// pick so that 2^bits exceeds the largest value in any coordinate
            float SCALAR = (float)Math.Pow(10,3);
            var coords = parseCommandLine(command_line);
            int[] scaledIntCoordinates = new int[6];
            

            for(int i=0; i<coords.Length; i++){
                float floatCoordinate = (float)Double.Parse(coords[i], System.Globalization.NumberStyles.Float);
                float scaledCoordinate = floatCoordinate * SCALAR;
                int coordinate = (int)scaledCoordinate;
                // if (coordinate < 0){coordinate = coordinate * -1;}
                scaledIntCoordinates[i] = coordinate;
            }
            foreach(var i in scaledIntCoordinates){
                Console.Write(i + " ");
            }Console.WriteLine(" ");
            

            

            string[] coordinatesWithAppendedHilbertIndex = new string[command_line.Length + 1];
            for(int i=0; i<command_line.Length; i++){
                coordinatesWithAppendedHilbertIndex[i]=command_line[i];
            }
            HilbertPoint hilbertPoint = new HilbertPoint(scaledIntCoordinates, bpd);
            Tuple<string, BigInteger, int> line_HI_index = new Tuple<string, BigInteger, int>(lineToString(command_line), hilbertPoint.HilbertIndex, index);
            return line_HI_index;
        }


        public static string[] parseCommandLine(string[] line){
            string[] coords = new string[6];
            for (int i=6; i<=11; i++){
                coords[i-6] = line[i];
            }
            return coords;

        }

        public static string[] readCommandFile(string address){
            string[] returnLine;
            string[] inputLine = System.IO.File.ReadAllLines(address);
            using (TextFieldParser parser = new TextFieldParser(address)){
                parser.Delimiters = new string[] { " " };
                returnLine = parser.ReadFields();
            }
            return returnLine;

        }
        public static string lineToString(string[] line){
            string toReturn = "";
            for (int i=0; i<line.Length; i++){
                toReturn += line[i]+" ";
            }
            return toReturn;
        }

        public static int FindBitsPerDimension(int max)
		{
			// Add one, because if the range is 0 to N, we need to represent N+1 different values.
			return (max + 1).SmallestPowerOfTwo();
		}
    }
}
