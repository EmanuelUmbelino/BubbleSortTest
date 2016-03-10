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
        int valueToRand = 10;
        int changeTimes = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Generate(object sender, EventArgs e)
        {
            AddElemets();
        }
        void AddElemets()
        {
            label1.Text = "";
            changeTimes = 0;
            while (valueToRand + 5 * changeTimes <= 100)
            {
                int[] numbers = RandomNumbers(valueToRand + changeTimes * 5);
                numbers = Organize(numbers);
                
                foreach (int ou in numbers)
                {
                    label1.Text += ou.ToString() + "; ";
                }
                label1.Text += "\n";
                changeTimes += 1;
            }
        }
        int[] RandomNumbers (int length)
        {
            int[] myReturn = new int[length];
            for (int i = 0; i < length; i++)
            {
                myReturn[i] = random.Next(0, valueToRand + 5 * changeTimes);
            }
            return myReturn;
        }
        int[] Organize(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                for (int f = i; f < numbers.Length; f++)
                {
                    if (f != i)
                    {
                        if (trackBar1.Value.Equals(0))
                        {
                            if (numbers[i] > numbers[f])
                            {
                                int ou = numbers[i];
                                numbers[i] = numbers[f];
                                numbers[f] = ou;

                            }
                        }
                        else if (trackBar1.Value.Equals(2))
                        {
                            if (numbers[i] < numbers[f])
                            {
                                int ou = numbers[i];
                                numbers[i] = numbers[f];
                                numbers[f] = ou;
                                
                            }
                        }
                    }
                }
            }
            return numbers;
        }

    }
}
