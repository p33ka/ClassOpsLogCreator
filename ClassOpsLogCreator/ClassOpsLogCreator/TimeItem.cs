﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// Author: Jhan Perera
/// Department: UIT Client Services
/// 
/// 
/// Description of class: This class is my Time format 
/// It is used for filling the combo boxes in a quick way. 
///
/// Class Version: 0.1.0.0 - BETA - 7152016
/// 
/// System Version: 0.1.0.0 - BETA - 7152016
/// 
/// </summary>
namespace ClassOpsLogCreator
{
    class TimeItem
    {
        /// <summary>
        /// Private attributes
        /// </summary>
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Am { get; set; }

        /// <summary>
        /// This is the toString method that will be invoked when the class
        /// is called on the combo box. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Hour + ":" + this.Minute;
        }
    }
}
