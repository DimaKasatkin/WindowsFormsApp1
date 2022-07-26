using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        NumericUpDown numericUpDown = new NumericUpDown();
        public double[,] matrix;
        public int minimum, max;
        public int[] currentJ;

        public Form1()
        {
            InitializeComponent();
            Size();
            int size = Convert.ToInt32(5);
            matrix = new double[,] { { 0, 16, 15, 29, 19 }, { 16, 0, 28, 12, 10 }, { 15, 28, 0, 3, 3 }, { 29, 12, 3, 0, 13 }, { 19, 10, 3, 13, 0 } };
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    dataGridView[j, i].Value = matrix[i, j];
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView[e.RowIndex, e.ColumnIndex].Value = dataGridView[e.ColumnIndex, e.RowIndex].Value;
            if (e.RowIndex == e.ColumnIndex)
                dataGridView[e.ColumnIndex, e.RowIndex].Value = 0;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Size();
        }

        public void Size()
        {
            dataGridView.ColumnCount = Convert.ToInt32(5);
            dataGridView.RowCount = Convert.ToInt32(5);
            int size;
            size = dataGridView.Width / dataGridView.ColumnCount;
            for (int i = 0; i < dataGridView.ColumnCount; i++)
                dataGridView.Columns[i].Width = 50;
        }

        private void buttonSolution_Click(object sender, EventArgs e)
        {
            int size = Convert.ToInt32(5);
            matrix = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[j, i] = Convert.ToDouble(dataGridView[j, i].Value);
            int currentPosition = 0;
            double[] time = new double[size];
            currentJ = new int[size + 1];
            int prevIndex = 0;
            bool f;
            currentJ[0] = 0;
            double min = matrix[0, 1];
            for (int i = 0; i < size - 1; i++)
            {
                int j = 0;
                f = false;
                for (j = 0; j < size; j++)
                {
                    if (currentPosition != j)
                        if (!currentJ.Contains(j))
                            if (radioButtonMin.Checked && matrix[currentPosition, j] < min)
                            {
                                min = matrix[currentPosition, j];
                                currentJ[i + 1] = j;
                                f = true;
                            }
                            else if (radioButtonMax.Checked && matrix[currentPosition, j] > min)
                            {
                                min = matrix[currentPosition, j];
                                currentJ[i + 1] = j;
                                f = true;
                            }
                }
                time[i] = min;
                if (!f)
                    currentJ[i + 1] = prevIndex;
                currentPosition = currentJ[i + 1];

                bool ff = true;
                for (int k = 0; k < size && ff; k++)
                    if (!currentJ.Contains(k))
                    {
                        min = matrix[currentPosition, k];
                        prevIndex = k;
                        ff = false;
                    }
            }
            time[size - 1] = matrix[currentPosition, 0];
            currentJ[size] = 0;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    dataGridView.Rows[i].Cells[j].Style.BackColor = Color.White;
            for (int i = 0; i < size; i++)
                dataGridView.Rows[currentJ[i]].Cells[currentJ[i + 1]].Style.BackColor = Color.Green;

            double sum = 0;
            for (int i = 0; i < size; i++)
                sum += time[i];

            labelResult.Text = sum.ToString();
            labelWay.Text = "";
            for (int i = 0; i < currentJ.Length; i++)
                labelWay.Text += currentJ[i].ToString() + " ";
        }

        private void buttonSolutiontFull_Click(object sender, EventArgs e)
        {
            int size = Convert.ToInt32(5);
            currentJ = new int[Convert.ToInt32(5) + 1];
            minimum = 10000;
            max = 0;
            int[] A = new int[dataGridView.RowCount];
            for (int i = 0; i < A.Length; i++)
                A[i] = i;
            permutation(A, A.Length, A.Length);
            if (radioButtonMin.Checked)
                labelResult.Text = minimum.ToString();
            else
                labelResult.Text = max.ToString();

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    dataGridView.Rows[i].Cells[j].Style.BackColor = Color.White;
            for (int i = 0; i < size; i++)
                dataGridView.Rows[currentJ[i]].Cells[currentJ[i + 1]].Style.BackColor = Color.Green;
            labelWay.Text = "";
            for (int i = 0; i < currentJ.Length; i++)
                labelWay.Text += currentJ[i].ToString() + " ";
        }

        private void swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        private void procesing(int[] A, int n)
        {
            int sum = 0;
            int[] B = new int[A.Length + 1];
            for (int i = 0; i < A.Length; i++)
                B[i] = A[i];
            B[A.Length] = A[0];
            int count = 0;
            for (int i = 0; i < A.Length; i++)
            {
                sum += Convert.ToInt32(dataGridView[B[count + 1], B[count]].Value.ToString());
                count++;
            }
            if (minimum > sum && radioButtonMin.Checked == true)
            {
                minimum = sum;
                for (int i = 0; i < B.Length; i++)
                    currentJ[i] = B[i];
            }
            if (max < sum && radioButtonMax.Checked == true)
            {
                max = sum;
                for (int i = 0; i < B.Length; i++)
                    currentJ[i] = B[i];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void permutation(int[] A, int n, int N)
        {
            if (n == 1)
                procesing(A, N);
            else
            {
                for (int i = 0; i < n; i++)
                {
                    swap(ref A[i], ref A[n - 1]);
                    permutation(A, n - 1, N);
                    swap(ref A[i], ref A[n - 1]);
                }
            }
        }
    }
}

