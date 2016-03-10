using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Random random = new Random();
        int valueToRand = 1001;
        public Form1()
        {
            InitializeComponent();
        }

        private void Generate(object sender, EventArgs e)
        {
            int[] numbers = new int[50];
            label1.Text = "";
            for (int i = 0; i < numbers.Length; i++ )
            {
                numbers[i] = RandomAndTest(numbers[i], numbers, i);
            }
            foreach(int ou in numbers)
            {
                
                label1.Text += ou.ToString() + "; ";
            }
        }
        int RandomAndTest (int i, int[] numbers, int myPosition)
        {
            i = random.Next(0, valueToRand);
            for (int f = 0; f < myPosition; f++)
            {
                if (i.Equals(numbers[f]))
                {
                    i = RandomAndTest(i, numbers, myPosition);
                }
                if (trackBar1.Value.Equals(0) && i > numbers[f])
                {
                    int ou = i;
                    i = numbers[f];
                    numbers[f] = ou;
                }
                else if (trackBar1.Value.Equals(2) && i < numbers[f])
                {
                    int ou = i;
                    i = numbers[f];
                    numbers[f] = ou;
                }
            }

            return i;
        }

    }
}
