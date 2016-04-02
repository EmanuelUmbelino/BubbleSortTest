using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        #region Property
        private Random random = new Random();
        private Thread[] backgroundWorker;

        private int waitTime;
        private int[] valueToRand, comparisons;
        
        private bool[] finished;
        #endregion

        #region Methods
        #region Initialize
        public Form1()
        {
            finished = new bool[4];
            comparisons = new int[4];
            backgroundWorker = new Thread[4];
            valueToRand = new int[4];
            for (int i = 0; i < 4; i++)
            {
                finished[i] = true;
            }
            InitializeComponent();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
        }

        private void ChangeVelocity(object sender, EventArgs e)
        {
            if(tabControl1.SelectedTab.Name.Equals("tabPage1"))
                waitTime = int.Parse(numericUpDown8.Value.ToString());
            else
                waitTime = int.Parse(numericUpDown7.Value.ToString());
        }

        #endregion

        #region Start Sorting

        private void GoSorting(object sender, EventArgs e)
        {
            StartSorting();
        }
        private void StartSorting()
        {
            if (finished[0] && finished[1])
            {
                finished[0] = false;
                finished[1] = false;
                chart1.Series.Clear();
                chart2.Series.Clear();
                comparisons[0] = 0;
                comparisons[1] = 0;
                waitTime = 5;
                backgroundWorker[0] = new Thread(Call1);
                backgroundWorker[0].IsBackground = true;
                backgroundWorker[0].Start();
                backgroundWorker[1] = new Thread(Call2);
                backgroundWorker[1].IsBackground = true;
                backgroundWorker[1].Start();
                waitTime = int.Parse(numericUpDown8.Value.ToString());
            }
        }
        private void Call1()
        {
            DrawGraph(0, chart1, domainUpDown1, numericUpDown1, label1);
        }
        private void Call2()
        {
            DrawGraph(1, chart2, domainUpDown2, numericUpDown2, label2);
        }

        #endregion

        #region Start Search
        private void StartSearch()
        {
            if (finished[2] && finished[3])
            {
                finished[2] = false;
                finished[3] = false;
                chart3.Series.Clear();
                chart4.Series.Clear();
                comparisons[2] = 0;
                comparisons[3] = 0;
                backgroundWorker[2] = new Thread(Call3);
                backgroundWorker[2].IsBackground = true;
                backgroundWorker[2].Start();
                backgroundWorker[3] = new Thread(Call4);
                backgroundWorker[3].IsBackground = true;
                backgroundWorker[3].Start();
                waitTime = int.Parse(numericUpDown7.Value.ToString());
            }
        }

        private void GoSearch(object sender, EventArgs e)
        {
            StartSearch();
        }
        private void Call3()
        {
            DrawGraph(2, chart3, domainUpDown3, numericUpDown3, label3, int.Parse(numericUpDown5.Value.ToString()));
        }
        private void Call4()
        {
            DrawGraph(3, chart4, domainUpDown4, numericUpDown4, label4, int.Parse(numericUpDown6.Value.ToString()));
        }
        #endregion

        #region Draw Sorting


        private void DrawGraph(int myThread, Chart myChart, DomainUpDown myDomain, NumericUpDown myNumeric, Label myLabel)
        {

            this.Invoke(new MethodInvoker(() =>
            {
                myChart.Series.Add("Execution: " + myDomain.Text);
                myChart.Series[0].IsVisibleInLegend = false;
                myChart.Series[0].ChartType = SeriesChartType.Column;
                myChart.Series[0].Points.Clear();
            }));
            valueToRand[myThread] = int.Parse(myNumeric.Value.ToString());
            int[] data = GenerateArray(valueToRand[myThread], false);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    myChart.Series[0].Points.AddXY(k, data[k]);
                    myChart.Series[0].Points[k].Color = Color.White;
                }));
            }
            if (myDomain.Text.Equals("BubbleSort"))
                BubbleSort(data, myThread,myChart, myLabel);
            else if (myDomain.Text.Equals("MergeSort"))
                MergeSort(data, 0, data.Length - 1, myThread, myChart, myLabel);
            else if (myDomain.Text.Equals("QuickSort"))
                QuickSort(data, 0, data.Length - 1, myThread, myChart, myLabel);
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    myLabel.Text = "Select a Type";
                }));
            }

            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    myChart.Series[0].Points[k].Color = Color.Lime;
                }));
            }

            finished[myThread] = true;
        }
        #endregion

        #region Draw Search
        private void DrawGraph(int myThread, Chart myChart, DomainUpDown myDomain, NumericUpDown myNumeric, Label myLabel, int myCompare)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                myChart.Series.Add("Execution: " + myDomain.Text);
                myChart.Series[0].IsVisibleInLegend = false;
                myChart.Series[0].ChartType = SeriesChartType.Column;
                myChart.Series[0].Points.Clear();
            }));

            valueToRand[myThread] = int.Parse(myNumeric.Value.ToString());
            int[] data = GenerateArray(valueToRand[myThread], true);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    myChart.Series[0].Points.AddXY(k, data[k]);
                    if (k.Equals(myCompare))
                        myChart.Series[0].Points[k].Color = Color.Lime;
                    else
                        myChart.Series[0].Points[k].Color = Color.White;

                }));
            }
            if (myDomain.Text.Equals("Linear"))
                Linear(data, myThread, myChart,myLabel, myCompare);
            else if (myDomain.Text.Equals("Binary"))
                Binary(data, 0, data.Length - 1, myThread, myChart, myLabel,myCompare);
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    myLabel.Text = "Select a Type";
                }));
            }


            finished[myThread] = true;
        }
        #endregion

        #region Sorting Algorithms
        #region BubbleSort
        private void BubbleSort(int[] data, int myThread, Chart myChart, Label myLabel)
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
                    }
                    comparisons[myThread]++;
                    this.Invoke(new MethodInvoker(() =>
                    {
                        if (j + 1 < i)
                            myChart.Series[0].Points[j + 1].Color = Color.Red;
                        if (j + 2 < i)
                            myChart.Series[0].Points[j + 2].Color = Color.Red;
                        myChart.Series[0].Points[j].Color = Color.White;
                        if (j > 0)
                            myChart.Series[0].Points[j - 1].Color = Color.White;
                        for (int k = 0; k < data.Length; k++)
                        {
                            myChart.Series[0].Points[k].SetValueXY(k, data[k]);
                        }
                        myLabel.Text = "Comparisons: " + comparisons[myThread];
                    }));

                    Thread.Sleep(waitTime);
                }
            }

            
        }
        #endregion 

        #region MergeSort
        void MergeSort(int[] data, int left, int right, int myThread, Chart myChart, Label myLabel)
        {
            int i, j, k, mid;
            int []sup;

            if (left == right) return;
            
            mid = (left + right) / 2;
            MergeSort(data, left, mid, myThread, myChart, myLabel);
            MergeSort(data, mid + 1, right, myThread, myChart, myLabel);
            
            i = left;
            j = mid + 1;
            k = 0;
            sup = new int[right - left + 1];

            while (i < mid + 1 || j < right + 1)
            {
                if (i == mid + 1)
                {
                    sup[k] = data[j];
                    j++;
                }
                else if (j == right + 1)
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
                comparisons[myThread]++;
            }
            for (i = left; i <= right; i++)
            {
                data[i] = sup[i - left];

                this.Invoke(new MethodInvoker(() =>
                {
                    myChart.Series[0].Points[i].Color = Color.Red;
                    myChart.Series[0].Points[i].SetValueXY(i, data[i]);
                    myLabel.Text = "Comparisons: " + comparisons[myThread];
                }));

                Thread.Sleep(waitTime);
            }

            for (int n = 0; n < data.Length; n++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    if (n.Equals(i) || n.Equals(j))
                        myChart.Series[0].Points[n].Color = Color.Red;
                    else
                        myChart.Series[0].Points[n].Color = Color.White;
                }));
            }
            Thread.Sleep(waitTime);
        }
        #endregion

        #region QuickSort
        void QuickSort(int[] data, int left, int right, int myThread, Chart myChart, Label myLabel)
        {
            int i, j, x, y;
            i = left;
            j = right;
            x = data[(left + right) / 2];

            while (i <= j)
            {
                while (data[i] < x && i < right)
                {
                    i++;
                }
                while (data[j] > x && j > left)
                {
                    j--;
                }
                if (i <= j)
                {
                    y = data[i];
                    data[i] = data[j];
                    data[j] = y;
                    i++;
                    j--;
                    if (j >= 0)
                    {
                        comparisons[myThread]++;
                        this.Invoke(new MethodInvoker(() =>
                        {
                            myChart.Series[0].Points[i].Color = Color.Red;
                            myChart.Series[0].Points[j].Color = Color.Red;
                            myLabel.Text = "Comparisons: " + comparisons[myThread];
                        }));
                        Thread.Sleep(waitTime);
                    }
                }
            }
            for (int n = 0; n < data.Length; n++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    myChart.Series[0].Points[n].Color = Color.White;
                    myChart.Series[0].Points[n].SetValueXY(n, data[n]);
                    myLabel.Text = "Comparisons: " + comparisons[myThread];
                }));
            }
            Thread.Sleep(waitTime);
            if (j > left)
            {
                QuickSort(data, left, j, myThread,myChart,myLabel);
            }
            if (i < right)
            {
                QuickSort(data, i, right, myThread,myChart,myLabel);
            }
        }

        #endregion
        #endregion

        #region Search Algorithms

        #region Linear
        private void Linear(int[] data, int myThread, Chart myChart, Label myLabel, int myCompare)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Equals(myCompare))
                {

                    this.Invoke(new MethodInvoker(() =>
                    {

                        myChart.Series[0].Points[i].Color = Color.Red;
                        myLabel.Text = "Comparisons: " + comparisons[myThread];
                    }));
                    Thread.Sleep(waitTime);
                    break;
                }
                else
                {
                    comparisons[myThread]++;
                    this.Invoke(new MethodInvoker(() =>
                    {
                        myChart.Series[0].Points[i].Color = Color.Red;
                        myLabel.Text = "Comparisons: " + comparisons[myThread];
                    }));                   
                    Thread.Sleep(waitTime);
                }
                
            }


        }
        #endregion

        #region Binary
        private void Binary(int[] data, int left, int right, int myThread, Chart myChart, Label myLabel, int myCompare)
        {
            int i = (left+right) / 2;
            this.Invoke(new MethodInvoker(() =>
            {

                myChart.Series[0].Points[i].Color = Color.Red;
                myLabel.Text = "Comparisons: " + comparisons[myThread];
            }));
            Thread.Sleep(waitTime);
            if (myCompare.Equals(i))
            {
                comparisons[myThread]++;
                return;
            }
            else if(i > myCompare)
            {
                comparisons[myThread]++;
                Binary(data, left, i, myThread, myChart, myLabel,myCompare);
            }
            else
            {
                comparisons[myThread]++;
                Binary(data, i, right, myThread, myChart,myLabel,myCompare);
            }


        }
        #endregion 

        #endregion

        #region Array Elements
        int[] GenerateArray (int length, bool order)
        {
            // Creat a int array with my length.
            int[] myReturn = new int[length];
            // Random all array elements.
            if (!order)
            {
                for (int i = 0; i < length; i++)
                {
                    myReturn[i] = RandomArrayElements(myReturn[i], myReturn, i);
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    myReturn[i] = i;
                }
            }
            // Return this array.
            return myReturn;
        }
        int RandomArrayElements(int i, int[] numbers, int myPosition)
        {
            // Random a number, between 0 and total numbers.
            i = random.Next(0, numbers.Length);
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
