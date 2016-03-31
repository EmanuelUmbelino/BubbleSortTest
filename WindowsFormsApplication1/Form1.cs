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
        private Thread backgroundWorker1, backgroundWorker2;

        private int valueToRand1, valueToRand2, comparisons1, comparisons2;

        private bool finished1, finished2;
        #endregion

        #region Methods
        #region Initialize
        public Form1()
        {
            finished1 = true;
            finished2 = true;
            InitializeComponent();
            chart1.Series.Clear();
            chart2.Series.Clear();
        }
        #endregion

        #region Begin
        
        private void Start()
        {
            if (finished1 && finished2)
            {
                finished1 = false;
                finished2 = false;
                chart1.Series.Clear();
                chart2.Series.Clear();
                comparisons1 = 0;
                comparisons2 = 0;
                backgroundWorker1 = new Thread(DrawGraph1);
                backgroundWorker1.IsBackground = true;
                backgroundWorker1.Start();
                backgroundWorker2 = new Thread(DrawGraph2);
                backgroundWorker2.IsBackground = true;
                backgroundWorker2.Start();
            }
        }


        //// When the button "Go" is clicked, this method is called.
        //// This execute just in the first time.
        private void Generate(object sender, EventArgs e)
        {
            Start();
        }
        #endregion
        
        #region Draw
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
            int[] data = GenerateArray(valueToRand1);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart1.Series[0].Points.AddXY(k, data[k]);
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
                    chart1.Series[0].Points[k].Color = Color.Green;
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
            int[] data = GenerateArray(valueToRand2);
            for (int k = 0; k < data.Length; k++)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    chart2.Series[0].Points.AddXY(k, data[k]);
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
                    chart2.Series[0].Points[k].Color = Color.Green;
                }));
            }
            finished2 = true;
        }             
        #endregion

        #region Algorithm
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
                            for (int k = 0; k < data.Length; k++)
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {

                                    if (k.Equals(j + 1) || k.Equals(j + 2))
                                        chart1.Series[0].Points[k].Color = Color.Red;
                                    else
                                        chart1.Series[0].Points[k].Color = Color.Black;
                                    chart1.Series[0].Points[k].SetValueXY(k, data[k]);
                                    label1.Text = "Comparisons: " + comparisons1;
                                }));
                            }
                        }
                        else
                        {
                            comparisons2++;
                            for (int k = 0; k < data.Length; k++)
                            {
                                this.Invoke(new MethodInvoker(() =>
                                {

                                    if (k.Equals(j + 1) || k.Equals(j + 2))
                                        chart2.Series[0].Points[k].Color = Color.Red;
                                    else
                                        chart2.Series[0].Points[k].Color = Color.Black;
                                    chart2.Series[0].Points[k].SetValueXY(k, data[k]);
                                    label2.Text = "Comparisons: " + comparisons2;
                                }));
                            }
                        }
                        Thread.Sleep(5);

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

                    Thread.Sleep(5);
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
                Thread.Sleep(5);
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
                    Thread.Sleep(5);
                }

                for (int n = 0; n < data.Length; n++)
                {
                    this.Invoke(new MethodInvoker(() =>
                    {
                        if (n.Equals(i) || n.Equals(j))
                            chart2.Series[0].Points[n].Color = Color.Red;
                        else
                            chart2.Series[0].Points[n].Color = Color.Black;
                    }));
                }
                Thread.Sleep(5);
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
                            Thread.Sleep(5);
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
                            Thread.Sleep(5);
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
                        chart1.Series[0].Points[n].Color = Color.Black;
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
                        chart2.Series[0].Points[n].Color = Color.Black;
                        chart2.Series[0].Points[n].SetValueXY(n, data[n]);
                        label2.Text = "Comparisons: " + comparisons2;
                    }));
                }
            }
            Thread.Sleep(5);
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
