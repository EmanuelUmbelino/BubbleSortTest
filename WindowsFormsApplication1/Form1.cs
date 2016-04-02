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
        private Thread backgroundWorker1, backgroundWorker2, backgroundWorker3, backgroundWorker4;

        private int valueToRand1, valueToRand2, valueToRand3, valueToRand4, comparisons1, comparisons2, comparisons3, comparisons4, waitTime;
        private int[] compare;

        private bool finished1, finished2, finished3, finished4;
        #endregion

        #region Methods
        #region Initialize
        public Form1()
        {
            finished1 = true;
            finished2 = true;
            finished3 = true;
            finished4 = true;
            InitializeComponent();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
            compare = new int[2];
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

        private void StartSorting()
        {
            if (finished1 && finished2)
            {
                finished1 = false;
                finished2 = false;
                chart1.Series.Clear();
                chart2.Series.Clear();
                comparisons1 = 0;
                comparisons2 = 0;
                waitTime = 5;
                backgroundWorker1 = new Thread(DrawGraph1);
                backgroundWorker1.IsBackground = true;
                backgroundWorker1.Start();
                backgroundWorker2 = new Thread(DrawGraph2);
                backgroundWorker2.IsBackground = true;
                backgroundWorker2.Start();
                waitTime = int.Parse(numericUpDown8.Value.ToString());
            }
        }


        //// When the button "Go" is clicked, this method is called.
        //// This execute just in the first time.
        private void GoSorting(object sender, EventArgs e)
        {
            StartSorting();
        }
        #endregion

        #region Start Search
        private void StartSearch()
        {
            if (finished3 && finished4)
            {
                finished3 = false;
                finished4 = false;
                chart3.Series.Clear();
                chart4.Series.Clear();
                comparisons3 = 0;
                comparisons4 = 0;
                compare[0] = int.Parse(numericUpDown5.Value.ToString());
                compare[1] = int.Parse(numericUpDown6.Value.ToString());
                backgroundWorker3 = new Thread(DrawGraph3);
                backgroundWorker3.IsBackground = true;
                backgroundWorker3.Start();
                backgroundWorker4 = new Thread(DrawGraph4);
                backgroundWorker4.IsBackground = true;
                backgroundWorker4.Start();
                waitTime = int.Parse(numericUpDown7.Value.ToString());
            }
        }

        private void GoSearch(object sender, EventArgs e)
        {
            StartSearch();
        }
        #endregion

        #region Draw Sorting
        private void DrawGraph1()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                chart1.Series.Add("Execution: " + domainUpDown1.Text);
                chart1.Series[0].IsVisibleInLegend = false;
                chart1.Series[0].ChartType = SeriesChartType.Column;
                chart1.Series[0].Points.Clear();
            }));

            valueToRand1 = int.Parse(numericUpDown1.Value.ToString());
            int[] data = GenerateArray(valueToRand1, false);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart1.Series[0].Points.AddXY(k, data[k]);
                    chart1.Series[0].Points[k].Color = Color.White;
                }));
            }
            if (domainUpDown1.Text.Equals("BubbleSort"))
                BubbleSort(data,1);
            else if (domainUpDown1.Text.Equals("MergeSort"))
                MergeSort(data, 0, data.Length-1,1);
            else if (domainUpDown1.Text.Equals("QuickSort"))
                QuickSort(data, 0, data.Length - 1, 1);
            else
            { 
                this.Invoke(new MethodInvoker(() =>
                {
                    label1.Text = "Select a Type";
                }));
            }

            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart1.Series[0].Points[k].Color = Color.Lime;
                }));
            }

            finished1 = true;
        }
        private void DrawGraph2()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                chart2.Series.Add("Execution: " + domainUpDown2.Text);
                chart2.Series[0].IsVisibleInLegend = false;
                chart2.Series[0].ChartType = SeriesChartType.Column;
                chart2.Series[0].Points.Clear();
            }));

            valueToRand2 = int.Parse(numericUpDown2.Value.ToString());
            int[] data = GenerateArray(valueToRand2, false);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart2.Series[0].Points.AddXY(k, data[k]);
                    chart2.Series[0].Points[k].Color = Color.White;
                }));
            }
            if (domainUpDown2.Text.Equals("BubbleSort"))
                BubbleSort(data,2);
            else if (domainUpDown2.Text.Equals("MergeSort"))
                MergeSort(data, 0, data.Length - 1, 2);
            else if (domainUpDown2.Text.Equals("QuickSort"))
                QuickSort(data, 0, data.Length - 1, 2);
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    label2.Text = "Select a Type";
                }));
            }

            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart2.Series[0].Points[k].Color = Color.Lime;
                }));
            }
            finished2 = true;
        }
        #endregion

        #region Draw Search
        private void DrawGraph3()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                chart3.Series.Add("Execution: " + domainUpDown3.Text);
                chart3.Series[0].IsVisibleInLegend = false;
                chart3.Series[0].ChartType = SeriesChartType.Column;
                chart3.Series[0].Points.Clear();
            }));

            valueToRand3 = int.Parse(numericUpDown3.Value.ToString());
            int[] data = GenerateArray(valueToRand3, true);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart3.Series[0].Points.AddXY(k, data[k]);
                    if (k.Equals(compare[0]))
                        chart3.Series[0].Points[k].Color = Color.Lime;
                    else
                        chart3.Series[0].Points[k].Color = Color.White;

                }));
            }
            if (domainUpDown3.Text.Equals("Linear"))
                Linear(data, 1);
            else if (domainUpDown3.Text.Equals("Binary"))
                Binary(data, 0, data.Length - 1 , 1);
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    label3.Text = "Select a Type";
                }));
            }
            

            finished3 = true;
        }
        private void DrawGraph4()
        {
            this.Invoke(new MethodInvoker(() =>
            {
                chart4.Series.Add("Execution: " + domainUpDown4.Text);
                chart4.Series[0].IsVisibleInLegend = false;
                chart4.Series[0].ChartType = SeriesChartType.Column;
                chart4.Series[0].Points.Clear();
            }));

            valueToRand4 = int.Parse(numericUpDown4.Value.ToString());
            int[] data = GenerateArray(valueToRand4, true);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart4.Series[0].Points.AddXY(k, data[k]);
                    if (k.Equals(compare[1]))
                        chart4.Series[0].Points[k].Color = Color.Lime;
                    else
                        chart4.Series[0].Points[k].Color = Color.White;
                }));
            }
            if (domainUpDown4.Text.Equals("Linear"))
                Linear(data, 2);
            else if (domainUpDown4.Text.Equals("Binary"))
                Binary(data, 0, data.Length - 1, 2);
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    label4.Text = "Select a Type";
                }));
            }
            

            finished4 = true;
        }
        #endregion

        #region Sorting Algorithms
        #region BubbleSort
        private void BubbleSort(int[] data, int myThread)
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
                        if (myThread.Equals(1))
                        {
                            comparisons1++;
                            this.Invoke(new MethodInvoker(() =>
                            {
                                if (j + 1 < i)
                                    chart1.Series[0].Points[j + 1].Color = Color.Red;
                                if (j + 2 < i)
                                    chart1.Series[0].Points[j+2].Color = Color.Red;
                                chart1.Series[0].Points[j].Color = Color.White;
                                if(j > 0)
                                    chart1.Series[0].Points[j-1].Color = Color.White;
                                for (int k = 0; k < data.Length; k++)
                                {
                                    chart1.Series[0].Points[k].SetValueXY(k, data[k]);
                                }
                                label1.Text = "Comparisons: " + comparisons1;
                            }));
                        }
                        else
                        {
                            comparisons2++;
                            this.Invoke(new MethodInvoker(() =>
                            {
                                if (j + 1 < i)
                                    chart2.Series[0].Points[j + 1].Color = Color.Red;
                                if (j + 2 < i)
                                    chart2.Series[0].Points[j + 2].Color = Color.Red;
                                chart2.Series[0].Points[j].Color = Color.White;
                                if (j > 0)
                                    chart2.Series[0].Points[j - 1].Color = Color.White;
                                for (int k = 0; k < data.Length; k++)
                                {
                                    chart2.Series[0].Points[k].SetValueXY(k, data[k]);
                                }
                                label2.Text = "Comparisons: " + comparisons2;
                            }));
                        }
                        Thread.Sleep(waitTime);
                    }
                    else
                    {
                        if (myThread.Equals(1))
                        {
                            comparisons1++;
                            this.Invoke(new MethodInvoker(() =>
                            {
                                if (j + 1 < i)
                                    chart1.Series[0].Points[j + 1].Color = Color.Red;
                                if (j + 2 < i)
                                    chart1.Series[0].Points[j + 2].Color = Color.Red;
                                chart1.Series[0].Points[j].Color = Color.White;
                                if (j > 0)
                                    chart1.Series[0].Points[j - 1].Color = Color.White;
                                for (int k = 0; k < data.Length; k++)
                                {
                                    chart1.Series[0].Points[k].SetValueXY(k, data[k]);
                                }
                                label1.Text = "Comparisons: " + comparisons1;
                            }));
                        }
                        else
                        {
                            comparisons2++;
                            this.Invoke(new MethodInvoker(() =>
                            {
                                if (j + 1 < i)
                                    chart2.Series[0].Points[j + 1].Color = Color.Red;
                                if (j + 2 < i)
                                    chart2.Series[0].Points[j + 2].Color = Color.Red;
                                chart2.Series[0].Points[j].Color = Color.White;
                                if (j > 0)
                                    chart2.Series[0].Points[j - 1].Color = Color.White;
                                for (int k = 0; k < data.Length; k++)
                                {
                                    chart2.Series[0].Points[k].SetValueXY(k, data[k]);
                                }
                                label2.Text = "Comparisons: " + comparisons2;
                            }));
                        }
                        Thread.Sleep(waitTime);
                    }
                }
            }

            
        }
        #endregion 

        #region MergeSort
        void MergeSort(int[] data, int left, int right, int myThread)
        {
            int i, j, k, mid;
            int []sup;

            if (left == right) return;
            
            mid = (left + right) / 2;
            MergeSort(data, left, mid, myThread);
            MergeSort(data, mid + 1, right, myThread);
            
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
                if(myThread.Equals(1))
                    comparisons1++;
                else
                    comparisons2++;
            }
            if (myThread.Equals(1))
            {
                for (i = left; i <= right; i++)
                {
                    data[i] = sup[i - left];

                    this.Invoke(new MethodInvoker(() =>
                    {
                        chart1.Series[0].Points[i].Color = Color.Red;
                        chart1.Series[0].Points[i].SetValueXY(i, data[i]);
                        label1.Text = "Comparisons: " + comparisons1;
                    }));

                    Thread.Sleep(waitTime);
                }

                for (int n = 0; n < data.Length; n++)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        if (n.Equals(i) || n.Equals(j))
                            chart1.Series[0].Points[n].Color = Color.Red;
                        else
                            chart1.Series[0].Points[n].Color = Color.White;
                    }));
                }
                Thread.Sleep(waitTime);
            }

            else
            {
                for (i = left; i <= right; i++)
                {
                    data[i] = sup[i - left];

                    this.Invoke(new MethodInvoker(() =>
                    {
                        chart2.Series[0].Points[i].Color = Color.Red;
                        chart2.Series[0].Points[i].SetValueXY(i, data[i]);
                        label2.Text = "Comparisons: " + comparisons2;
                    }));
                    Thread.Sleep(waitTime);
                }

                for (int n = 0; n < data.Length; n++)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        if (n.Equals(i) || n.Equals(j))
                            chart2.Series[0].Points[n].Color = Color.Red;
                        else
                            chart2.Series[0].Points[n].Color = Color.White;
                    }));
                }
                Thread.Sleep(waitTime);
            }
        }
        #endregion

        #region QuickSort
        void QuickSort(int[] data, int left, int right, int myThread)
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
                        if (myThread.Equals(1))
                        {

                            comparisons1++;
                            this.Invoke(new MethodInvoker(() =>
                            {
                                chart1.Series[0].Points[i].Color = Color.Red;
                                chart1.Series[0].Points[j].Color = Color.Red;
                                label1.Text = "Comparisons: " + comparisons1;
                            }));
                            Thread.Sleep(waitTime);
                        }
                        else
                        {

                            comparisons2++;
                            this.Invoke(new MethodInvoker(() =>
                            {
                                chart2.Series[0].Points[i].Color = Color.Red;
                                chart2.Series[0].Points[j].Color = Color.Red;
                                label2.Text = "Comparisons: " + comparisons2;
                            }));
                            Thread.Sleep(waitTime);
                        }
                    }
                }
            }
            if (myThread.Equals(1))
            {
                for (int n = 0; n < data.Length; n++)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        chart1.Series[0].Points[n].Color = Color.White;
                        chart1.Series[0].Points[n].SetValueXY(n, data[n]);
                        label1.Text = "Comparisons: " + comparisons1;
                    }));
                }
            }
            else
            {
                for (int n = 0; n < data.Length; n++)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        chart2.Series[0].Points[n].Color = Color.White;
                        chart2.Series[0].Points[n].SetValueXY(n, data[n]);
                        label2.Text = "Comparisons: " + comparisons2;
                    }));
                }
            }
            Thread.Sleep(waitTime);
            if (j > left)
            {
                QuickSort(data, left, j, myThread);
            }
            if (i < right)
            {
                QuickSort(data, i, right, myThread);
            }
        }

        #endregion
        #endregion

        #region Search Algorithms

        #region Linear
        private void Linear(int[] data, int myThread)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Equals(compare[myThread-1]))
                {
                    
                    if (myThread.Equals(1))
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                                
                            chart3.Series[0].Points[i].Color = Color.Red;
                            label3.Text = "Comparisons: " + comparisons3;
                        }));
                    }
                    else
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                                
                            chart4.Series[0].Points[i].Color = Color.Red;
                            label4.Text = "Comparisons: " + comparisons4;
                        }));
                    }
                    Thread.Sleep(waitTime);
                    break;
                }
                else
                {
                    if (myThread.Equals(1))
                    {
                        comparisons3++;
                        this.Invoke(new MethodInvoker(() =>
                        {
                            chart3.Series[0].Points[i].Color = Color.Red;
                            label3.Text = "Comparisons: " + comparisons3;
                        }));
                    }
                    else
                    {
                        comparisons4++;
                        this.Invoke(new MethodInvoker(() =>
                        {
                            chart4.Series[0].Points[i].Color = Color.Red;
                            label4.Text = "Comparisons: " + comparisons4;
                        }));
                    }
                    Thread.Sleep(waitTime);
                }
                
            }


        }
        #endregion

        #region Binary
        private void Binary(int[] data, int left, int right, int myThread)
        {
            int i = (left+right) / 2;
            if (myThread.Equals(1))
            {
                this.Invoke(new MethodInvoker(() =>
                {

                    chart3.Series[0].Points[i].Color = Color.Red;
                    label3.Text = "Comparisons: " + comparisons3;
                }));
            }
            else
            {
                this.Invoke(new MethodInvoker(() =>
                {

                    chart4.Series[0].Points[i].Color = Color.Red;
                    label4.Text = "Comparisons: " + comparisons4;
                }));
            }
            Thread.Sleep(waitTime);
            if (compare[myThread-1].Equals(i))
            {
                if (myThread.Equals(1))
                    comparisons3++;                
                else
                    comparisons4++;
                return;
            }
            else if(i > compare[myThread - 1])
            {
                if (myThread.Equals(1))
                    comparisons3++;
                else
                    comparisons4++;
                Binary(data, left, i, myThread);
            }
            else
            {
                if (myThread.Equals(1))
                    comparisons3++;
                else
                    comparisons4++;
                Binary(data, i, right, myThread);
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
