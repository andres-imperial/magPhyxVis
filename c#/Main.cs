﻿//https://github.com/paulchernoch/HilbertTransformation
using System;
using System.Numerics;
using HilbertTransformation;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.IO;

using System.Collections;


namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {   



            makeNewCommandFiles(1);

  


            // List<decimal[]> coordinates = new List<decimal[]>();
            // for (decimal i=1; i<=5; i+=1m){
            //     for (decimal j=-2; j<=2; j+=1.25m){
            //         coordinates.Add(new decimal[]{i,j});
            //     }
            // }
            // multiplyCoordinates(coordinates);
            // shiftCoordinates(coordinates);

            // foreach(decimal[] decArr in coordinates){
            //     foreach(decimal coord in decArr){
            //         Console.Write(coord + " ");
            //     }Console.WriteLine(" ");
            // }

            
            

            

            

        }



        public static void multiplyCoordinates(List<decimal[]> arr){
            int[] dimensionHighestDecimalPlaces = getDimensionHighestDecimalPlaces(arr);
            for(int i=0; i<dimensionHighestDecimalPlaces.Length; i++){
                for (int j=0; j<arr.Count; j++){
                    // arr[j][i] *= 10^dimensionHighestDecimalPlaces[i];
                    arr[j][i] *= Convert.ToInt64(Math.Pow(10, Convert.ToDouble(dimensionHighestDecimalPlaces[i])));
                }
            }

        }

        public static int[] getDimensionHighestDecimalPlaces(List<decimal[]> arr){
            int[] dimensionHighestDecimalPlaces = new int[arr[0].Length];
            for (int i=0; i<dimensionHighestDecimalPlaces.Length; i++){
                int maxPlaces = int.MinValue;
                for (int j=0; j<arr.Count; j++){
                    if (getDecimalPlaces(arr[j][i]) > maxPlaces){
                        maxPlaces = getDecimalPlaces(arr[j][i]);
                    }
                }
                dimensionHighestDecimalPlaces[i] = maxPlaces;
            }
         return dimensionHighestDecimalPlaces;   
        }

        public static int getDecimalPlaces(decimal n){
            n = Math.Abs(n); //make sure it is positive.
            n -= (int)n;     //remove the integer part of the number.
            var decimalPlaces = 0;
            while (n > 0)
            {
                decimalPlaces++;
                n *= 10;
                n -= (int)n;
            }
            return decimalPlaces;
        }

        public static void shiftCoordinates(List<decimal[]> arr){
            decimal[] dimensionMins = getDimensionMins(arr);
            for (int i=0; i<dimensionMins.Length; i++){
                if (dimensionMins[i] < 0){
                    for (int j=0; j<arr.Count; j++){
                        arr[j][i] += dimensionMins[i]*-1;
                    }
                }
            }
        }
        public static decimal[] getDimensionMins(List<decimal[]> arr){
            decimal[] dimensionMins = new decimal[arr[0].Length];
            for (int i=0; i<dimensionMins.Length; i++){
                decimal min = decimal.MaxValue;
                for (int j=0; j<arr.Count; j++){
                    if (arr[j][i] < min){
                        min = arr[j][i];
                    }
                }
                dimensionMins[i] = min;
            }
            return dimensionMins;
        }

        /*
            shift all coordinates per on a dimension by dimension basis so all values are equal to or greater than 0
        */
        // public static void shiftCoordinates(int[][] arr){
        //     int[] dimensionMins = getDimensionMins(arr);
        //     for (int i=0; i<dimensionMins.Length; i++){
        //         for (int j=0; j<arr.Length; j++){
        //             arr[j][i] += dimensionMins[i]*-1;
        //         }
        //     }
        // }

        /*
            return array of mins in each dimension of two dimensional array
        */
        // public static int[] getDimensionMins(int[][] arr){
        //     int[] dimensionMins = new int[arr[0].Length];
        //     for (int i=0; i<dimensionMins.Length; i++){
        //         int min = int.MaxValue;
        //         for (int j=0; j<arr.Length; j++){
        //             if (arr[j][i] < min){
        //                 min = arr[j][i];
        //             }
        //         }
        //         dimensionMins[i] = min;
        //     }
        //     return dimensionMins;
        // }

        public static List<Tuple<string, BigInteger, int>> makeSortedTuples(int folderNumber){
                
            List<Tuple<string,BigInteger,int>> commandLines_HI_index = new List<Tuple<string, BigInteger,int>>();
            List<string[]> command_lines = new List<string[]>();
            for (int i=0; i<100; i++){
                string index;
                if (i<=9){
                    index = String.Format("0{0}", i);
                }else{index = i.ToString();}
                string address_i = String.Format(@"..\data\data{0}\commands\commands{1}.csv", folderNumber, index);
                command_lines.Add(readCommandFile(address_i));                
            }
            commandLines_HI_index = commandLine_HI_initIndex(command_lines);
            commandLines_HI_index.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            return commandLines_HI_index;
        }


        public static List<Tuple<string, BigInteger, int>> commandLine_HI_initIndex(List<string[]> command_lines){
            List<decimal[]> decimal_command_lines =  new List<decimal[]>();
            List<int[]> int_command_lines = new List<int[]>();
            List<Tuple<string, BigInteger, int>> commandLine_HI_initIndexes = new List<Tuple<string, BigInteger, int>>();
            int bpd = FindBitsPerDimension(10);// pick so that 2^bits exceeds the largest value in any coordinate

            for(int i=0; i<command_lines.Count; i++){
                string[] parsed_command_line = parseCommandLine(command_lines[i]);
                decimal[] arr = new decimal[parsed_command_line.Length];
                for(int j=0; j<arr.Length; j++){
                    arr[j] = Convert.ToDecimal(parsed_command_line[j]);
                }
                decimal_command_lines.Add(arr);
            }

            multiplyCoordinates(decimal_command_lines);
            shiftCoordinates(decimal_command_lines);

            // foreach(decimal[] dec in decimal_command_lines){
            //     foreach(decimal d in dec){
            //         Console.Write("{0} ", d);
            //     }Console.WriteLine(" ");
            // }

            for (int i=0; i<decimal_command_lines.Count; i++){
                int[] arr = new int[decimal_command_lines[i].Length];
                for (int j=0; j<decimal_command_lines[i].Length; j++){
                    arr[j] = (int)(decimal_command_lines[i][j]);
                }
                int_command_lines.Add(arr);
            }

            for(int i=0; i<int_command_lines.Count; i++){
                var hIndex = new HilbertPoint(int_command_lines[i], bpd).HilbertIndex;
                Tuple<string, BigInteger, int> arrangedCommandLine = new Tuple<string, BigInteger, int>(lineToString(command_lines[i]), hIndex, i);
                
            }





            // string[] coordinatesWithAppendedHilbertIndex = new string[command_line.Length + 1];
            // for(int i=0; i<command_line.Length; i++){
            //     coordinatesWithAppendedHilbertIndex[i]=command_line[i];
            // }
            // HilbertPoint hilbertPoint = new HilbertPoint(scaledIntCoordinates, bpd);
            // Tuple<string, BigInteger, int> line_HI_index = new Tuple<string, BigInteger, int>(lineToString(command_line), hilbertPoint.HilbertIndex, index);
            return commandLine_HI_initIndexes;
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
        
        // public static Tuple<string, BigInteger, int> commandLine_HI_initIndex(string[] command_line, int index){
        //     int bpd = FindBitsPerDimension(10);// pick so that 2^bits exceeds the largest value in any coordinate
        //     float SCALAR = (float)Math.Pow(10,3);
        //     var coords = parseCommandLine(command_line);
        //     int[] scaledIntCoordinates = new int[6];
            

        //     for(int i=0; i<coords.Length; i++){
        //         float floatCoordinate = (float)Double.Parse(coords[i], System.Globalization.NumberStyles.Float);
        //         float scaledCoordinate = floatCoordinate * SCALAR;
        //         int coordinate = (int)scaledCoordinate;
        //         // if (coordinate < 0){coordinate = coordinate * -1;}
        //         scaledIntCoordinates[i] = coordinate;
        //     }
            
        //     string[] coordinatesWithAppendedHilbertIndex = new string[command_line.Length + 1];
        //     for(int i=0; i<command_line.Length; i++){
        //         coordinatesWithAppendedHilbertIndex[i]=command_line[i];
        //     }
        //     HilbertPoint hilbertPoint = new HilbertPoint(scaledIntCoordinates, bpd);
        //     Tuple<string, BigInteger, int> line_HI_index = new Tuple<string, BigInteger, int>(lineToString(command_line), hilbertPoint.HilbertIndex, index);
        //     return line_HI_index;
        // }


        






























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
