using System;
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
        #region Property
        private Random random = new Random();
        private Timer timer;
        private Series series;

        private int[] numbers;
        private int valueToRand, comparisons, 
            selectNumber = 1, numberChecked = 1;

        bool finished;
        #endregion

        #region Methods
        #region Initialize
        public Form1()
        {
            InitializeComponent();
            // Initialize the timer to prevent bugs.
            timer = new Timer();
        }
        #endregion

        #region Start All
        // When the button "Go" is clicked, this method is called.
        // This execute just in the first time.
        private void Generate(object sender, EventArgs e)
        {
            // Reset the properties.
            finished = false; numberChecked = 1; selectNumber = 1;comparisons = 0;
            // If timer is in execute, stop him.
            if(timer.Enabled)
                timer.Stop();
            // Get the number of the "NumericUpDown" and use him to be the number of array. 
            // Generate Array with this number.
            valueToRand = int.Parse(numericUpDown1.Value.ToString());
            numbers = GenerateArray(valueToRand);

            // Reset graphic.
            chart1.Series.Clear();
            // Add all array elements in the graphic and hide the legend.
            for (int i = 0; i < numbers.Length; i++)
            {
                series = this.chart1.Series.Add(i.ToString());
                series.Points.Add(numbers[i]);
                series.IsVisibleInLegend = false;
            }
            // Starts the timer.
            InitTimer();
        }
        #endregion

        #region Timer
        // Create the nem timer with 1ms of interval in every tick.
        public void InitTimer()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Interval = 1; // in miliseconds
            timer.Start();
        }
        // When is passed a interval, this is called.
        // This call every time until finish to organize.
        private void timer1_Tick(object sender, EventArgs e)
        {
            OganizeNumbers();
            Write();
        }
        #endregion

        #region Write
        void Write()
        {
            // Reset graphic.
            chart1.Series.Clear();
            // Add all array elements in the graphic and hide the legend.
            for (int i = 0; i < numbers.Length; i++)
            {
                series = this.chart1.Series.Add(i.ToString());
                series.Points.Add(numbers[i]);
                series.IsVisibleInLegend = false;
                // If finished, color lime the checked numbers.
                if (finished && i < selectNumber)
                    series.Color = Color.Lime;
                // Color the select number to red.
                if (i.Equals(selectNumber))
                {
                    series.Color = Color.Red;
                }
            }
            // Write the actual comparisons.
            label1.Text = "Comparisons: " + comparisons;
        }
        #endregion

        #region Numbers
        #region Reorganize Numbers
        void OganizeNumbers()
        {
            // Check that you aren't on the end of list.
            if (selectNumber < numbers.Length - numberChecked)
            {
                // If select number is greater than the next, exchange their positions. 
                if (numbers[selectNumber] > numbers[selectNumber + 1])
                {
                    int saveNumber = numbers[selectNumber];
                    int saveLocal = selectNumber;
                    numbers[selectNumber] = numbers[selectNumber + 1];
                    numbers[selectNumber + 1] = saveNumber;
                    // Go to next number.
                    selectNumber++;
                    // Sum comparisons.
                    if (!finished)
                        comparisons++;
                }
                else
                {
                    // Go to next number.
                    selectNumber++;
                    // Sum comparisons.
                    if (!finished)
                        comparisons++;
                }
            }
            // If you are on the end of the list.
            // This happens when the biggest is placed to your position
            else
            {
                // If finished, stop the timer.
                if (finished)
                    timer.Stop();
                // Reset the select number.
                selectNumber = 0;
                // Add a number checked.
                numberChecked++;
                // if the numbers checked is equals the total numbers, finished.
                if (numberChecked >= numbers.Length)
                {
                    numberChecked = 1;
                    finished = true;
                }
            }
        }
        #endregion 

        #region Raffle Array Elements
        int[] GenerateArray (int length)
        {
            // Creat a int array with my length.
            int[] myReturn = new int[length];
            // Random all array elements.
            for (int i = 0; i < length; i++)
            {
                myReturn[i] = RandomArrayElements(myReturn[i], myReturn, i); 
            }
            // Return this array.
            return myReturn;
        }
        int RandomArrayElements(int i, int[] numbers, int myPosition)
        {
            // Random a number, between 0 and total numbers.
            i = random.Next(0, valueToRand);
            // Checks this number repeats.
            for (int f = 0; f < myPosition; f++)
            {
                if (i.Equals(numbers[f]))
                {
                    i = RandomArrayElements(i, numbers, myPosition);
                }
            }

            return i;
        }
        #endregion
        #endregion
        #endregion

    }
}
