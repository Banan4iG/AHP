using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace AHP
{

	public partial class Form1 : Form
	{
		List<Alternative> ListAlternative;
		class Alternative
		{
			public Alternative(string name, int closeTown, int ecology, int road, int infr, int cost)
			{
				Name = name;
				CloseTown = closeTown;
				Ecology = ecology;
				Road = road;
				Infr = infr;
				Cost = cost;
			}
			string Name { get; set; }
			int CloseTown { get; set; }
			int Ecology { get; set; }
			int Road { get; set; }
			int Infr { get; set; }
			int Cost { get; set; }
		}

		double[,] a = new double[5, 5];

		double[,] closeTown = new double[,]
		{
			{1, 0.11, 0.125, 0.2, 0.5 },
			{9, 1, 2, 5, 6 },
			{8, 0.5, 1, 4, 3 },
			{5, 0.2, 0.25, 1, 2 },
			{2, 0.16, 0.33, 0.5, 1 }
		};

		double[,] ecology = new double[,]
		{
			{1, 3, 2, 1, 0.25 },
			{0.33, 1, 0.5, 0.25, 0.33 },
			{0.5, 2, 1, 0.5, 0.5 },
			{1, 4, 2, 1, 1 },
			{4, 3, 2, 1, 1 }
		};

		double[,] road = new double[,]
		{
			{1, 0.2, 0.16, 0.33, 1 },
			{5, 1, 2, 0.5, 3 },
			{6, 0.5, 1, 4, 4 },
			{3, 2, 0.25, 1, 1 },
			{1, 0.33, 0.25, 1, 1 }
		};

		double[,] infr = new double[,]
		{
			{1, 0.33, 0.25, 0.33, 1 },
			{3, 1, 0.5, 1, 2 },
			{4, 2, 1, 0.5, 3 },
			{3, 1, 2, 1, 4 },
			{1, 0.5, 0.33, 0.25, 1 }
		};

		double[,] cost = new double[,]
		{
			{1, 3, 5, 2, 1 },
			{0.33, 1, 2, 0.5, 1 },
			{0.2, 0.5, 1, 0.2, 0.16 },
			{0.5, 2, 5, 1, 0.5 },
			{1, 1, 6, 2, 1 }
		};

		int[] value = new int[] 
		{ 1, 3, 5, 7, 9 };

		public Form1()
		{
			InitializeComponent();
			ListAlternative = new List<Alternative> {
				new Alternative("Тургенево", 1, 1, 1, 1, 1),
				new Alternative("Карачарово", 1, 1, 1, 1, 1),
				new Alternative("Панфилово", 1, 1, 1, 1, 1),
				new Alternative("Ковардицы", 1, 1, 1, 1, 1),
				new Alternative("Чаадаево", 1, 1, 1, 1, 1) };
			
		}

		private void button1_Click(object sender, EventArgs e)
		{
			MatrixA(a);
			//for (int i = 0; i < 5; i++)
			//{
			//	for (int j = 0; j < 5; j++)
			//	{
			//		textBox1.Text += a[i, j].ToString("0.00") + '\t';
			//	}
			//	textBox1.Text += Environment.NewLine;
			//	textBox1.Text += Environment.NewLine;
			//}

			double[] weight = new double[5];
			double[] result = new double[5];
			double[] weightCloseTown = new double[5];
			double[] weightEcology = new double[5];
			double[] weightRoad = new double[5];
			double[] weightInfr = new double[5];
			double[] weightCost = new double[5];
			weight = Weight(a);
			weightCloseTown = Weight(closeTown);
			weightEcology = Weight(ecology);
			weightRoad = Weight(road);
			weightInfr = Weight(infr);
			weightCost = Weight(cost);

			double[,] weightMatrix = new double[5, 5];
 
			for (int j = 0; j < 5; j++)
			{
				weightMatrix[0, j] = weightCloseTown[j];
				weightMatrix[1, j] = weightEcology[j];
				weightMatrix[2, j] = weightRoad[j];
				weightMatrix[3, j] = weightInfr[j];
				weightMatrix[4, j] = weightCost[j];
			}

			result = Multy(weight, weightMatrix);

			for (int i = 0; i < 5; i++)
			{
				textBox3.Text += result[i].ToString("0.000");
				textBox3.Text += Environment.NewLine;
				textBox3.Text += Environment.NewLine;
			}
			chart1.Series.Clear();
			chart1.Series.Add(new Series("ColumnSeries")
			{
				ChartType = SeriesChartType.Doughnut
			});
			chart2.Series.Clear();
			chart2.Series.Add(new Series("ColumnSeries2")
			{
				ChartType = SeriesChartType.Column
			});
			string[] alter = { "Тургенево", "Карачарово", "Панфилово", "Ковардицы", "Чаадаево" };
			chart1.Series["ColumnSeries"].Points.DataBindXY(alter, result);
			chart2.Series["ColumnSeries2"].Points.DataBindXY(alter, result);
		}

		public void MatrixA(double[,] matrix)
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
					if (value[trackBar.Value] > value[trackBarNext.Value])
					{

						matrix[i, j] = value[trackBar.Value] - value[trackBarNext.Value];
						matrix[j, i] = 1 / (Convert.ToDouble(value[trackBar.Value]) - Convert.ToDouble(value[trackBarNext.Value]));
					}
					else if (value[trackBar.Value] < value[trackBarNext.Value])
					{
						matrix[i, j] = 1 / (Convert.ToDouble(value[trackBarNext.Value]) - Convert.ToDouble(value[trackBar.Value]));
						matrix[j, i] = value[trackBarNext.Value] - value[trackBar.Value];
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

			for (int j = 0; j < 5; j++)
			{
				for (int i = 0; i < 5; i++)
				{
					matrixA[i, j] = matrix[i, j] / sum[j];
				}
			}

			for (int i = 0; i < 5; i++)
			{
				asum = 0.0;
				for (int j = 0; j < 5; j++)
				{
					asum = asum + matrixA[i, j];
				}
				weight[i] = asum / 5;
			}
			
			return weight;
		}

		public double[] Multy(double[] vector, double[,] matrix)
		{
			double[] multyVector = new double[5];
			for (int i = 0; i < 5; i++)
			{
				multyVector[i] = 0;
				for (int j = 0; j < 5; j++)
				{
					multyVector[i] += matrix[i, j] * vector[j];
				}
			}
			return multyVector;
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void trackBar0_ValueChanged(object sender, EventArgs e)
		{
			switch (trackBar0.Value)
			{
				case 0: label8.Text = "Не важно"; break;
				case 1: label8.Text = "Немного важно"; break;
				case 2: label8.Text = "Средней важности"; break;
				case 3: label8.Text = "Важно"; break;
				case 4: label8.Text = "Наиболее важно"; break;
			}
		}

		private void trackBar1_ValueChanged(object sender, EventArgs e)
		{
			switch (trackBar1.Value)
			{
				case 0: label6.Text = "Не важно"; break;
				case 1: label6.Text = "Немного важно"; break;
				case 2: label6.Text = "Средней важности"; break;
				case 3: label6.Text = "Важно"; break;
				case 4: label6.Text = "Наиболее важно"; break;
			}
		}

		private void trackBar2_ValueChanged(object sender, EventArgs e)
		{
			switch (trackBar2.Value)
			{
				case 0: label7.Text = "Не важно"; break;
				case 1: label7.Text = "Немного важно"; break;
				case 2: label7.Text = "Средней важности"; break;
				case 3: label7.Text = "Важно"; break;
				case 4: label7.Text = "Наиболее важно"; break;
			}
		}

		private void trackBar3_ValueChanged(object sender, EventArgs e)
		{
			switch (trackBar3.Value)
			{
				case 0: label9.Text = "Не важно"; break;
				case 1: label9.Text = "Немного важно"; break;
				case 2: label9.Text = "Средней важности"; break;
				case 3: label9.Text = "Важно"; break;
				case 4: label9.Text = "Наиболее важно"; break;
			}
		}

		private void trackBar4_ValueChanged(object sender, EventArgs e)
		{
			switch (trackBar4.Value)
			{
				case 0: label10.Text = "Не важно"; break;
				case 1: label10.Text = "Немного важно"; break;
				case 2: label10.Text = "Средней важности"; break;
				case 3: label10.Text = "Важно"; break;
				case 4: label10.Text = "Наиболее важно"; break;
			}
		}
	}
}
