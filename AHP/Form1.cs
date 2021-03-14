using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AHP
{

	public partial class Form1 : Form
	{
		List<Alternative> ListAlternative;
		class Alternative
		{
			public Alternative(string name, double[,] matrix)
			{
				Name = name;
				Matrix = matrix;
			}
			string Name { get; set; }
			double[,] Matrix { get; set; }
		}
		double[,] MatrixTurgenevo = new double[,] { {1, 5, 4 },
													 {0.2, 1, 0.5 },
													 {0.25, 2, 1 }};
		double[,] a = new double[5, 5];

		public Form1()
		{
			InitializeComponent();
			ListAlternative = new List<Alternative> { new Alternative("Тургенево", MatrixTurgenevo) };
		}

		private void button1_Click(object sender, EventArgs e)
		{
			matrixA(a);
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					textBox1.Text += a[i, j].ToString("0.00") + '\t';
				}
				textBox1.Text += Environment.NewLine;
				textBox1.Text += Environment.NewLine;
			}
		}

		public void matrixA(double[,] matrix)
		{
			for (int i = 0; i < 5; i++)
				for (int j = 0; j < 5; j++)
				{
					if (i == j)
						matrix[i, j] = 1;
				}
			for (int i = 0; i < 5; i++)
			{
				string name = "trackBar";
				TrackBar trackBar = (TrackBar)Controls.Find(name + i, true)[0];
				for (int j = i + 1; j < 5; j++)
				{
					TrackBar trackBarNext = (TrackBar)Controls.Find(name + j, true)[0];
					if (trackBar.Value > trackBarNext.Value)
					{	
						
						matrix[i, j] = trackBar.Value - trackBarNext.Value;
						matrix[j, i] = 1 / (Convert.ToDouble(trackBar.Value) - Convert.ToDouble(trackBarNext.Value));
					}
					else if (trackBar.Value < trackBarNext.Value)
					{
						matrix[i, j] = 1 / (Convert.ToDouble(trackBarNext.Value) - Convert.ToDouble(trackBar.Value));
						matrix[j, i] = trackBarNext.Value - trackBar.Value;
					}
					else
					{
						matrix[i, j] = 1;
						matrix[j, i] = 1;
					}
				}
			}
		}


		public double[] Weight(double[,] matrix)
		{
			double[] sum = new double[5];
			double asum;
			double[,] matrixA = new double[5,5];
			double[] weight = new double[5];
			for (int j = 0; j < 5; j++)
			{
				sum[j] = 0;
				for (int i = 0; i < 5; i++)
				{
					sum[j] += matrix[i, j];
				}
			}
			for (int i = 0; i < 5; i++)
			{
				asum = 0.0;
				for (int j = 0; j < 5; j++)
				{
					matrixA[i, j] = matrix[i, j] / sum[i];
					asum = asum + matrixA[i, j];
				}
				weight[i] = asum / 5;
			}
			return weight;
		}
	}
}
