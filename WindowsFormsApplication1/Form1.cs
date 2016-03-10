﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        int valueToRand;
        int i_ = 0;
        private Timer timer1;
        int[] numbers;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1; // in miliseconds
            timer1.Start();
        }

        public Form1()
        {
            InitializeComponent();
            timer1 = new Timer();
        }

        private void Generate(object sender, EventArgs e)
        {
            if(timer1.Enabled)
                timer1.Stop();
            valueToRand = int.Parse(numericUpDown1.Value.ToString());
            numbers = RandomNumbers(valueToRand);
            chart1.Series.Clear();
            for (int i = 0; i < numbers.Length; i++)
            {
                Series series = this.chart1.Series.Add(i.ToString());
                series.Points.Add(numbers[i]);
                series.IsVisibleInLegend = false;
            }
            InitTimer();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            Write();
        }
        void Write()
        {
            numbers = Organize(numbers);
            chart1.Series.Clear();
            for (int i = 0; i < numbers.Length; i++)
            {
                Series series = this.chart1.Series.Add(i.ToString());
                series.Points.Add(numbers[i]);
                series.IsVisibleInLegend = false;
                if (i.Equals(i_))
                {
                    series.Color = Color.Red;
                }
            }
        }
        int[] RandomNumbers (int length)
        {
            int[] myReturn = new int[length];
            for (int i = 0; i < length; i++)
            {
                myReturn[i] = RandomAndOrder(myReturn[i], myReturn, i); 
            }
            return myReturn;
        }
        int RandomAndOrder(int i, int[] numbers, int myPosition)
        {
            i = random.Next(0, valueToRand);
            for (int f = 0; f < myPosition; f++)
            {
                if (i.Equals(numbers[f]))
                {
                    i = RandomAndOrder(i, numbers, myPosition);
                }
            }

            return i;
        }
        int[] Organize(int[] numbers)
        {
            if (i_ < numbers.Length-1)
            {
                if (numbers[i_] > numbers[i_ + 1])
                {
                    int saveNumber = numbers[i_];
                    int saveLocal = i_;
                    numbers[i_] = numbers[i_ + 1];
                    numbers[i_ + 1] = saveNumber;
                    i_++;

                }
                else
                {
                    i_++;
                }
            }
            else
            {
                i_ = 0;
            }
            return numbers;
        }


    }
}
