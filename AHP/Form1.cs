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
		public List<Alternative> ListAlternative;
		public class Alternative
		{
			public string Name { get; set; }
			public int CloseTown { get; set; }
			public int Ecology { get; set; }
			public int Road { get; set; }
			public int Infr { get; set; }
			public int Cost { get; set; }
			public Alternative(string name, int closetown, int ecology, int road, int infr, int cost)
			{
				Name = name;
				CloseTown = closetown;
				Ecology = ecology;
				Road = road;
				Infr = infr;
				Cost = cost;
			}

		}
		double[,] a = new double[5, 5];

		double[,] closeTown;

		double[,] ecology;

		double[,] road;

		double[,] infr;

		double[,] cost;

		int[] value = new int[] 
		{ 1, 3, 5, 7, 9 };

		public Form1()
		{
			InitializeComponent();
			ListAlternative = new List<Alternative> {
				new Alternative("Тургенево", 1, 3, 5, 7, 9),
				new Alternative("Карачарово", 7, 5, 3, 9, 1),
				new Alternative("Панфилово", 1, 3, 3, 1, 5),
				new Alternative("Ковардицы", 3, 5, 9, 1, 7),
				new Alternative("Чаадаево", 9, 1, 5, 7, 3) };
			viewAll();
		}

		private void viewAll()
		{
			listView1.Items.Clear();
			foreach (var r in ListAlternative)
			{
				listView1.Items.Add(r.Name);
			}
		}


		private void button1_Click(object sender, EventArgs e)
		{
			MatrixA(a);
			int len = ListAlternative.Count;

			closeTown = new double[len, len];
			ecology = new double[len, len];
			road = new double[len, len];
			infr = new double[len, len];
			cost = new double[len, len];

			double[] weight = new double[5];
			double[] result = new double[len];
			double[] weightCloseTown = new double[len];
			double[] weightEcology = new double[len];
			double[] weightRoad = new double[len];
			double[] weightInfr = new double[len];
			double[] weightCost = new double[len];
			weight = Weight(a);

			for (int i = 0; i < len; i++)
				for (int j = 0; j < len; j++)
					if (i == j)
					{
						closeTown[i, j] = 1;
						ecology[i, j] = 1;
						road[i, j] = 1;
						infr[i, j] = 1;
						cost[i, j] = 1;
					}

			int icr = 0;
			foreach (var el1 in ListAlternative)
			{
				int jec = 0;
				foreach (var el2 in ListAlternative)
				{
					if (ListAlternative.IndexOf(el2) < icr + 1)
					{
						jec++;
						continue;
					}

					//формирование матрицы близость к городу
					if ( el1.CloseTown > el2.CloseTown )
					{
						closeTown[icr, jec] = el1.CloseTown - el2.CloseTown;
						closeTown[jec, icr] = 1/(Convert.ToDouble(el1.CloseTown) - Convert.ToDouble(el2.CloseTown));
					}
					else if (el1.CloseTown < el2.CloseTown)
					{
						closeTown[icr, jec] = 1 / (Convert.ToDouble(el2.CloseTown) - Convert.ToDouble(el1.CloseTown));
						closeTown[jec, icr] = el2.CloseTown - el1.CloseTown;
					}
					else
					{
						closeTown[icr, jec] = 1;
						closeTown[jec, icr] = 1;
					}

					//формирование матрицы экология
					if (el1.Ecology > el2.Ecology)
					{
						ecology[icr, jec] = el1.Ecology - el2.Ecology;
						ecology[jec, icr] = 1 / (Convert.ToDouble(el1.Ecology) - Convert.ToDouble(el2.Ecology));
					}
					else if (el1.Ecology < el2.Ecology)
					{
						ecology[icr, jec] = 1 / (Convert.ToDouble(el2.Ecology) - Convert.ToDouble(el1.Ecology));
						ecology[jec, icr] = el2.Ecology - el1.Ecology;
					}
					else
					{
						ecology[icr, jec] = 1;
						ecology[jec, icr] = 1;
					}

					//формирование матрицы состояние дорог
					if (el1.Road > el2.Road)
					{
						road[icr, jec] = el1.Road - el2.Road;
						road[jec, icr] = 1 / (Convert.ToDouble(el1.Road) - Convert.ToDouble(el2.Road));
					}
					else if (el1.Road < el2.Road)
					{
						road[icr, jec] = 1 / (Convert.ToDouble(el2.Road) - Convert.ToDouble(el1.Road));
						road[jec, icr] = el2.Road - el1.Road;
					}
					else
					{
						road[icr, jec] = 1;
						road[jec, icr] = 1;
					}

					//формирование матрицы ифраструктура
					if (el1.Infr > el2.Infr)
					{
						infr[icr, jec] = el1.Infr - el2.Infr;
						infr[jec, icr] = 1 / (Convert.ToDouble(el1.Infr) - Convert.ToDouble(el2.Infr));
					}
					else if (el1.Infr < el2.Infr)
					{
						infr[icr, jec] = 1 / (Convert.ToDouble(el2.Infr) - Convert.ToDouble(el1.Infr));
						infr[jec, icr] = el2.Infr - el1.Infr;
					}
					else
					{
						infr[icr, jec] = 1;
						infr[jec, icr] = 1;
					}

					//формирование матрицы цена участка
					if (el1.Cost > el2.Cost)
					{
						cost[icr, jec] = el1.Cost - el2.Cost;
						cost[jec, icr] = 1 / (Convert.ToDouble(el1.Cost) - Convert.ToDouble(el2.Cost));
					}
					else if (el1.Cost < el2.Cost)
					{
						cost[icr, jec] = 1 / (Convert.ToDouble(el2.Cost) - Convert.ToDouble(el1.Cost));
						cost[jec, icr] = el2.Cost - el1.Cost;
					}
					else
					{
						cost[icr, jec] = 1;
						cost[jec, icr] = 1;
					}

					jec++;
				}
				icr++;
			}

			 

			weightCloseTown = Weight(closeTown);
			weightEcology = Weight(ecology);
			weightRoad = Weight(road);
			weightInfr = Weight(infr);
			weightCost = Weight(cost);

			double[,] weightMatrix = new double[len, 5];
 
			for (int j = 0; j < len; j++)
			{
				weightMatrix[j, 0] = weightCloseTown[j];
				weightMatrix[j, 1] = weightEcology[j];
				weightMatrix[j, 2] = weightRoad[j];
				weightMatrix[j, 3] = weightInfr[j];
				weightMatrix[j, 4] = weightCost[j];
			}

			result = Multy(weight, weightMatrix);

			for (int i = 0; i < len; i++)
			{
				textBox3.Text += result[i].ToString("0.000");
				textBox3.Text += Environment.NewLine;
				textBox3.Text += Environment.NewLine;
			}
			/*
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
			*/
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
			int size = matrix.GetLength(0);
			double[] sum = new double[size];
			double asum;
			double[,] matrixA = new double[size, size];
			double[] weight = new double[size];
			for (int j = 0; j < size; j++)
			{
				sum[j] = 0;
				for (int i = 0; i < size; i++)
				{
					sum[j] += matrix[i, j];
				}
			}

			for (int j = 0; j < size; j++)
			{
				for (int i = 0; i < size; i++)
				{
					matrixA[i, j] = matrix[i, j] / sum[j];
				}
			}

			for (int i = 0; i < size; i++)
			{
				asum = 0.0;
				for (int j = 0; j < size; j++)
				{
					asum = asum + matrixA[i, j];
				}
				weight[i] = asum / size;
			}

			return weight;
		}

		/*
		public double[] WeightForA(double[,] matrix)
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
		*/

		public double[] Multy(double[] vector, double[,] matrix)
		{
			//int size = matrix.GetLength(1);
			double[] multyVector = new double[matrix.GetLength(0)];
			for (int i = 0; i < matrix.GetLength(0); i++)
			{
				multyVector[i] = 0;	
				for (int j = 0; j < matrix.GetLength(1); j++)
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

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count > 0)
			{
				foreach(var r in ListAlternative)
				{
					if (r.Name == listView1.SelectedItems[0].Text)
					{
						/*
						public int CloseTown { get; set; }
						public int Ecology { get; set; }
						public int Road { get; set; }
						public int Infr { get; set; }
						public int Cost { get; set; }
						*/
						for(int i = 0; i<5; i++)
						{
							if (value[i] == r.CloseTown)
								trackBar5.Value = i;
							if (value[i] == r.Ecology)
								trackBar6.Value = i;
							if (value[i] == r.Road)
								trackBar7.Value = i;
							if (value[i] == r.Infr)
								trackBar8.Value = i;
							if (value[i] == r.Cost)
								trackBar9.Value = i;
						}
					}
				}
			}
			else
			{
				return;
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			ListAlternative.Add(new Alternative(textBox1.Text, value[trackBar5.Value], value[trackBar6.Value], value[trackBar7.Value], value[trackBar8.Value], value[trackBar9.Value]));
			textBox1.Text = "";
			viewAll();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (listView1.SelectedItems.Count > 0)
			{
				foreach (var r in ListAlternative)
				{
					if (r.Name == listView1.SelectedItems[0].Text)
					{
						/*
						public int CloseTown { get; set; }
						public int Ecology { get; set; }
						public int Road { get; set; }
						public int Infr { get; set; }
						public int Cost { get; set; }
						*/
						r.CloseTown = value[trackBar5.Value];
						r.Ecology = value[trackBar6.Value];
						r.Road = value[trackBar7.Value];
						r.Infr = value[trackBar8.Value];
						r.Cost = value[trackBar9.Value];
					}
				}
			}
			else
			{
				return;
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			trackBar5.Value = 0;
			trackBar6.Value = 0;
			trackBar7.Value = 0;
			trackBar8.Value = 0;
			trackBar9.Value = 0;
		}
	}
}
