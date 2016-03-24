using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        #region Property
        private Random random = new Random();
        private Thread backgroundWorker;

        private int valueToRand, comparisons;

        private bool finished;
        #endregion

        #region Methods
        #region Initialize
        public Form1()
        {
            finished = true;
            InitializeComponent();
            chart1.Series.Clear();
        }
        #endregion

        #region Begin
        
        private void Start()
        {
            if (finished)
            {
                finished = false;
                chart1.Series.Clear();
                comparisons = 0;
                backgroundWorker = new Thread(DrawGraph);
                backgroundWorker.IsBackground = true;
                backgroundWorker.Start();
            }
        }


        //// When the button "Go" is clicked, this method is called.
        //// This execute just in the first time.
        private void Generate(object sender, EventArgs e)
        {
            Start();
        }
        #endregion

        #region Timer
        //// Create the nem timer with 1ms of interval in every tick.
        //public void InitTimer()
        //{
        //    timer = new Timer();
        //    if (domainUpDown1.Text.Equals("BubbleSort"))
        //    {
        //        timer.Tick += new EventHandler(buble_Tick);
        //        timer.Interval = 1; // in miliseconds
        //    }
        //    else if (domainUpDown1.Text.Equals("MergeSort"))
        //    {
        //        timer.Tick += new EventHandler(merge_Tick);
        //        timer.Interval = 100; // in miliseconds
        //    }
        //    timer.Start();
        //}
        //// When is passed a interval, this is called.
        //// This call every time until finish to organize.
        //private void buble_Tick(object sender, EventArgs e)
        //{
        //    BubbleSort();
        //    Write();
        //}
        //private void merge_Tick(object sender, EventArgs e)
        //{
        //    MergeSort(0, numbers.Length - 1);
        //    Write();
        //}
        #endregion

        #region Draw
        private void DrawGraph()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                chart1.Series.Add("Execution: " + domainUpDown1.Text);
                chart1.Series[0].ChartType = SeriesChartType.Column;
                chart1.Series[0].Points.Clear();
            }));

            valueToRand = int.Parse(numericUpDown1.Value.ToString());
            int[] data = GenerateArray(valueToRand);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart1.Series[0].Points.AddXY(k, data[k]);
                }));
            }
            if (domainUpDown1.Text.Equals("BubbleSort"))
                BubbleSort(data);
            else if (domainUpDown1.Text.Equals("MergeSort"))
                MergeSort(data, 0, data.Length-1);
            else
                MessageBox.Show("Select a Type");

            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart1.Series[0].Points[k].Color = Color.Green;
                }));
            }
            finished = true;
        }


        void Write()
        {
            //// Reset graphic.
            //chart1.Series.Clear();
            //// Add all array elements in the graphic and hide the legend.
            //for (int i = 0; i < numbers.Length; i++)
            //{
            //    series = this.chart1.Series.Add(i.ToString());
            //    series.Points.Add(numbers[i]);
            //    series.IsVisibleInLegend = false;
            //    // If finished, color lime the checked numbers.
            //    if (finished && i < selectNumber)
            //        series.Color = Color.Lime;
            //    // Color the select number to red.
            //    if (i.Equals(selectNumber))
            //    {
            //        series.Color = Color.Red;
            //    }
            //}
            //// Write the actual comparisons.
        }
        #endregion

        #region Algorithm
        #region BubbleSort
        private void BubbleSort(int[] data)
        {
            for (int i = data.Length - 1; i >= 1; i--)
            {
                for (int j = 0; j < i; j++)
                {

                    if (data[j] > data[j + 1])
                    {
                        int aux = data[j];
                        data[j] = data[j + 1];
                        data[j + 1] = aux;
                        comparisons++;
                        for (int k = 0; k < data.Length; k++)
                        {
                            this.Invoke(new MethodInvoker(() =>
                            {
                                if (k.Equals(j+1) || k.Equals(j+2))
                                    chart1.Series[0].Points[k].Color = Color.Red;
                                else
                                    chart1.Series[0].Points[k].Color = Color.Black;
                                chart1.Series[0].Points[k].SetValueXY(k, data[k]);
                                label1.Text = "Comparisons: " + comparisons;
                            }));
                        }

                    }
                }
            }

            
        }
        #endregion 

        #region MergeSort
        void MergeSort(int[] data, int inicialPosition, int endPosition)
        {
            int i, j, k, mid;
            int []sup;

            if (inicialPosition == endPosition) return;
            
            mid = (inicialPosition + endPosition) / 2;
            MergeSort(data, inicialPosition, mid);
            MergeSort(data, mid + 1, endPosition);
            
            i = inicialPosition;
            j = mid + 1;
            k = 0;
            sup = new int[endPosition - inicialPosition + 1];

            while (i < mid + 1 || j < endPosition + 1)
            {
                if (i == mid + 1)
                {
                    sup[k] = data[j];
                    j++;
                }
                else if (j == endPosition + 1)
                {
                    sup[k] = data[i];
                    i++;
                }
                else if (data[i] < data[j])
                {
                    sup[k] = data[i];
                    i++; 
                }
                else
                {
                    sup[k] = data[j];
                    j++;
                }
                k++;
                comparisons++;
            }

            // copia vetor intercalado para o vetor original
            for (i = inicialPosition; i <= endPosition; i++)
            {
                data[i] = sup[i - inicialPosition];
                this.Invoke(new MethodInvoker(() =>
                {
                    chart1.Series[0].Points[i].Color = Color.Red;
                    chart1.Series[0].Points[i].SetValueXY(i, data[i]);
                    label1.Text = "Comparisons: " + comparisons;
                }));
            }
            for (int n = 0; n < data.Length; n++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (n.Equals(i) || n.Equals(j))
                        chart1.Series[0].Points[n].Color = Color.Red;
                    else
                        chart1.Series[0].Points[n].Color = Color.Black;
                }));
            }
        }
        #endregion 
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

    }
}
