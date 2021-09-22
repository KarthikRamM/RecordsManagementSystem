using System;
using System.Collections.Generic;
using System.IO;

namespace ManagementSystem
{
    class FileOperations
    {
        static string filename = Directory.GetCurrentDirectory() + "\\Teacher Record Management System.txt";
        static List<string> lines;
        static List<int> id = new List<int>();
        static void Main()
        {
            setup();
            while (true)
            {
                Console.WriteLine("Rainbow School Teacher Record Management System\n\nTo Store new teacher/s data\tEnter 1\nTo Retrive teacher record/s\tEnter 2");
                Console.WriteLine("To Update teacher data\t\tEnter 3\nTo Delete teacher record/s\tEnter 4\nTo Exit\t\t\t\tEnter 5");
                Console.Write("Your choice : ");
                int oc = int.Parse(Console.ReadLine()), ic;
                switch (oc)
                {
                    case 1: Console.WriteLine("\nRainbow School Teacher RMS -> New record/s\n\nTo Store a new teacher's data\t\tEnter 1");
                            Console.WriteLine("To Store multiple teachers data\t\tEnter 2\nTo go back to the previous menu\t\tEnter 0");
                            Console.Write("Your choice : ");
                            ic = int.Parse(Console.ReadLine());
                            if (ic == 1)
                                storeSingle();
                            else if (ic == 2)
                                storeMulti();
                            break;
                    case 2: if (fileEmpty("retrive"))
                                break;
                            Console.WriteLine("\nRainbow School Teacher RMS -> Retrive record/s\n\nTo Retrive a teacher's record\t\t\tEnter 1");
                            Console.WriteLine("To Retrive a Range of teacher records\t\tEnter 2\n(I.e., of those ID's in a specific range)");
                            Console.WriteLine("To Retrive all the teacher records\t\tEnter 3");
                            Console.WriteLine("To go back to the previous menu\t\t\tEnter 0");
                            Console.Write("Your choice : ");
                            ic = int.Parse(Console.ReadLine());
                            switch (ic)
                            {
                                case 1: retriveSingle();
                                        break;
                                case 2: retriveMulti();
                                        break;
                                case 3: retriveAll();
                                        break;
                            }
                            break;
                    case 3: if (fileEmpty("update"))
                                break;
                            Console.WriteLine("\nRainbow School Teacher RMS -> Update record");
                            updateSingle();
                            break;
                    case 4: if (fileEmpty("delete"))
                                break;
                            Console.WriteLine("\nRainbow School Teacher RMS -> Delete record/s\n\nTo Delete a teacher's record\t\t\tEnter 1");
                            Console.WriteLine("To Delete a Range of teacher records\t\tEnter 2\n(I.e., of those ID's in a specific range)");
                            Console.WriteLine("To Delete all the teacher records\t\tEnter 3");
                            Console.WriteLine("To go back to the previous menu\t\t\tEnter 0");
                            Console.Write("Your choice : ");
                            ic = int.Parse(Console.ReadLine());
                            switch (ic)
                            {
                                case 1: deleteSingle();
                                        break;
                                case 2: deleteMulti();
                                        break;
                                case 3: deleteAll();
                                        break;
                            }
                            break;
                    case 5: Environment.Exit(0);
                            break;
                    default:    Console.WriteLine("\nInvalid Input\nPlease Enter a valid Input");
                                break;
                }
                Console.WriteLine("\n");
            }
        }
        static void setup()
        {
            if (File.Exists(filename))
            {
                lines = new List<string>(File.ReadAllLines(filename));
                int n = lines.Count;
                for (int i = 0; i < n; i++)
                    id.Add(int.Parse(lines[i].Split('\t')[0]));
            }
            else
                lines = new List<string>();
        }
        static void storeSingle()
        {
            Console.Write("\nEnter Id : ");
            int newId = int.Parse(Console.ReadLine());
            int index = id.BinarySearch(newId);
            if (index > -1)
            {
                Console.WriteLine("Record with Id = " + newId + " is already present\n(Two Records cannot have same ID; Consider updating the Record)");
                return;
            }
            Console.Write("Enter Name : ");
            string newName = Console.ReadLine();
            Console.Write("Enter Class and Section : ");
            string newClassSec = Console.ReadLine();
            string newRecord = newId.ToString() + "\t" + newName + "\t" + newClassSec;
            index = ~index;
            id.Insert(index, newId);
            lines.Insert(index, newRecord);

            File.WriteAllLines(filename, lines);
            Console.WriteLine("\nNew teacher data inserted");
        }
        static void storeMulti()
        {
            int temp, c = 0, rv;
            string newName, newClassSec, newRecord;
            Console.Write("\nEnter all the Id values (separated by space) : ");
            string[] inpId = Console.ReadLine().Split(" ");
            if (inpId.Length == 1)
            {
                if (int.TryParse(inpId[0], out temp))
                    Console.WriteLine("\nUse store a new teacher's record option");
                else
                    Console.WriteLine("\nInvalid input");
                return;
            }
            foreach (string t in inpId)
            {
                temp = int.Parse(t);
                rv = id.BinarySearch(temp);
                if (rv < 0)
                {
                    Console.Write("\nId-" + temp + " Enter Name : ");
                    newName = Console.ReadLine();
                    Console.Write("Id-" + temp + " Enter Class and Section : ");
                    newClassSec = Console.ReadLine();
                    newRecord = temp + "\t" + newName + "\t" + newClassSec;
                    rv = ~rv;
                    id.Insert(rv, temp);
                    lines.Insert(rv, newRecord);
                    c++;
                }
                else
                    Console.WriteLine("\n" + "Id " + temp + " already present\n(Two Records cannot have same ID; Consider updating the Record)");
            }
            if (c == 0)
            {
                Console.WriteLine("\n No new data");
                return;
            }

            File.WriteAllLines(filename, lines);
            Console.WriteLine("\n" + c + " New teacher record/s inserted");
        }
        static void retriveSingle()
        {
            int retriveId, index;
            if (recordCheck("retrieved", out retriveId, out index))
                Console.WriteLine("\nRecord :\nID\tName\tClass & Section\n" + lines[index]);
        }
        static void retriveMulti()
        {
            int lb, ub, posLb = 0, posUb = 0;
            if (checkForMulti("retrive", out lb, out ub, ref posLb, ref posUb))
            {
                Console.WriteLine("\nRecords in range " + lb + "-" + ub + " :\n\nID\tName\tClass & Section");
                for (int i = posLb; i < posUb; i++)
                    Console.WriteLine(lines[i]);
                Console.WriteLine("\n" + (posUb - posLb) + " records retrived");
            }
        }
        static void retriveAll()
        {
            int n = id.Count;
            Console.WriteLine("\nDisplaying all teacher records\n\nID\tName\tClass & Section");
            for (int i = 0; i < n; i++)
                Console.WriteLine(lines[i]);
            Console.WriteLine("\n" + n + " records retrived");
        }
        static void updateSingle()
        {
            int updateId, index;
            if (recordCheck("updated", out updateId, out index))
            {
                string newName, newClassSec;
                Console.Write("\nEnter new Name : ");
                newName = Console.ReadLine();
                Console.Write("Enter new Class & Section : ");
                newClassSec = Console.ReadLine();
                lines[index] = updateId + "\t" + newName + "\t" + newClassSec;

                File.WriteAllLines(filename, lines);
                Console.WriteLine("\nTeacher record updated");
            }
        }
        static void deleteSingle()
        {
            int deleteId, index;
            if (recordCheck("deleted", out deleteId, out index))
            {
                lines.RemoveAt(index);
                id.RemoveAt(index);

                File.WriteAllLines(filename, lines);
                Console.WriteLine("\nRecord deleted");
            }
        }
        static void deleteMulti()
        {
            int lb, ub, posLb = 0, posUb = 0;
            if (checkForMulti("delete", out lb, out ub, ref posLb, ref posUb))
            {
                int count = posUb - posLb;
                lines.RemoveRange(posLb, count);
                id.RemoveRange(posLb, count);

                File.WriteAllLines(filename, lines);
                Console.WriteLine("\nRecords in range " + lb + "-" + ub + " deleted\n(" + count + " record/s deleted)");
            }
        }
        static void deleteAll()
        {
            Console.WriteLine("\nAll records will be deleted\nTo continue\tEnter 1\nTo cancel\tEnter 0");
            Console.Write("Your choice : ");
            if (1 == int.Parse(Console.ReadLine()))
            {
                lines.Clear();
                id.Clear();
                File.WriteAllText(filename, string.Empty);
                Console.WriteLine("\nAll Records deleted");
            }
        }
        static bool fileEmpty(string msg)
        {
            if (id.Count == 0)
            {
                Console.WriteLine("\nNo Records present to " + msg);
                return true;
            }
            return false;
        }
        static bool recordCheck(string msg, out int idVal, out int index)
        {
            Console.Write("\nEnter Id of the record to be " + msg + " : ");
            idVal = int.Parse(Console.ReadLine());
            index = id.BinarySearch(idVal);
            if (index < 0)
            {
                Console.WriteLine("\nNo record with Id = " + idVal);
                return false;
            }
            return true;
        }
        static bool checkForMulti(string msg, out int lb, out int ub, ref int posLb, ref int posUb)
        {
            Console.Write("\nEnter the lower bound of the Id range : ");
            lb = int.Parse(Console.ReadLine());
            Console.Write("Enter the upper bound of the Id range : ");
            ub = int.Parse(Console.ReadLine());
            if (lb == ub)
            {
                Console.WriteLine("\nBoth input values are same\nUse " + msg + " a teacher's record option");
                return false;
            }
            if (lb > ub)
            {
                int t = lb;
                lb = ub;
                ub = t;
                Console.WriteLine("Swapping entered upper and lower bounds (incorrectly input)");
            }
            posLb = id.BinarySearch(lb);
            posLb = posLb < 0 ? ~posLb : posLb;
            if (posLb != id.Count)
            {
                posUb = id.BinarySearch(ub);
                posUb = posUb < 0 ? ~posUb : ++posUb;
                if (posUb != posLb)
                    return true;
            }
            Console.WriteLine("\nNo records present in range " + lb + "-" + ub);
            return false;
        }
    }
}
