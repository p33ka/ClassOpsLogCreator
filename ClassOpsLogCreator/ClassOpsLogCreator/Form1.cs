﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ClassOpsLogCreator
{
    public partial class LogCreator : Form
    {
        // Values to read in the excel file, leave static so the excel file is 
        // accessable everywhere. 
        private static Excel.Application roomSched = null;
        private static Excel.Workbook roomWorkBook = null;
        private static Excel.Worksheet roomSheet1 = null;

        /** Constructor for the system. (Changes here should be confirmed with everyone first) */
        public LogCreator()
        {
            InitializeComponent();
        }

        /** When the user clicks the "Create" Button this is what will happen
         */
        private void createBTN_Click(object sender, EventArgs e)
        {
            //Open the room excel file
            roomSched = new Excel.Application();
            roomSched.Visible = false;
            try
            {
                //This should look for the file one level up. (Temporary to keep everything local)
                roomWorkBook = roomSched.Workbooks.Open(@"C:\Users\Jhan\Documents\Visual Studio 2015\Projects\ClassOpsLogCreator\room schedule.xlsx");
                //Work in worksheet number 1
                roomSheet1 = roomWorkBook.Sheets[1];

            }
            catch (Exception ex)
            {
                //File not found...
                textBox1.Text = "error: FILE NOT FOUND" + ex.ToString();
                roomSched.Quit();
                return;

            }
            //Get the range we are working within. (A1, A.LastRow)
            Excel.Range last = roomSheet1.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing);
            Excel.Range range1 = roomSheet1.get_Range("A5", "A" + last.Row);
            Excel.Range range2 = roomSheet1.get_Range("C5", "C" + last.Row);

            //Lets export the whole range of raw data into an array. (Cell.Value2 is a fast and accurate operation to use)
            System.Array array = (System.Array)range1.Cells.Value2;
            System.Array array2 = (System.Array)range2.Cells.Value2;

            //Now we extract all the raw data to strings 
            string[] arrayClassRooms = this.ConvertToStringArray(array, 0);
            string[] arrayTimes = this.ConvertToStringArray(array2, 1);
            //DO WORK HERE

            //This is the last time for each classroom in the list
            string[] arrayLastTimes = this.extract_last_time(arrayTimes);




            //DEGUB CODE
            //textBox1.Text = DateTime.FromOADate(double.Parse(arrayTimes[0])).ToString("hh:mm:tt");
            textBox1.Text = arrayClassRooms.Length.ToString() + " " + arrayLastTimes.Length.ToString();
        }


        /**A Helper converter that will take our "values" and convert them into a string array. 
         * String parsing IS requires for now until we make it smart. 
         * 
         * A string array is returned by with white spaces.
         * flag = 0 means no null/white space, 1 means leave white space and null and we work with doubles
         **/
        private string[] ConvertToStringArray(System.Array values, int flag)
        {
            string[] newArray = new string[values.Length];
            int index = 0;

            //The fun of double nester for loops, this is O(x^2)
            //This is the slowest part of the program at the moment
            for (int i = values.GetLowerBound(0);
                  i <= values.GetUpperBound(0); i++)
            {
                for (int j = values.GetLowerBound(1);
                          j <= values.GetUpperBound(1); j++)
                {
                    //This takes care of white space
                    if (values.GetValue(i, j).ToString().Trim().Length == 0)
                    {
                        //Add an empty string so we can parse for last time later
                         if(flag == 1)
                        {
                            newArray[index] = "";
                            index++;
                        }
                    }
                    //this puts in the sting representaion of what is in cell i, j
                    // can be 1 of three types: a normal string, an integer, or a double. 
                    else
                    {
                        newArray[index] = (string)values.GetValue(i, j).ToString();
                        //Move to the next position in the new array.
                        index++;
                    }
                }
            }
            //Return an array with no null characters
            return newArray = newArray.Where(n => n != null).ToArray();
        }

        /* A  helper method to get the last time in our time array
         */
        private string[] extract_last_time(string[] array)
        {
            string[] newArray = new string[array.Length];
            int index = 0;
            
            for (int i = array.GetLowerBound(0); i <= array.GetUpperBound(0) - 4; i++)
            {
                //if the next cell is empty we found the last time, add it to the array
                if ((array[i].ToString().Length != 0) && (array[i + 1].ToString().Length == 0))
                {
                    //add the last time to the list
                    newArray[index] = DateTime.FromOADate(double.Parse(array[i])).ToString("hh:mm tt");
                    index++;
                }
            } 
            return newArray = newArray.Where(n => n != null).ToArray();
        }
    }
}
