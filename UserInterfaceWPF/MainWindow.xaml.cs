using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace UserInterfaceWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ageevButton_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(pathTextBox.Text))
            {
                double[] nums = GetFromFile(pathTextBox.Text);
                if (CheckArray(nums))
                {
                    textBoxInformation.Clear();
                    textBlockTask.Text = "";

                    List<List<double>> list = GetValuesInTable(nums, "DOL", "EUR");

                    textBlockTask.Text += "Dollar: ";
                    List<double> minusDollar = MinMaxRubles(true, list);

                    textBlockTask.Text += "Euro: ";
                    List<double> minusEuro = MinMaxRubles(false, list);

                    double[] dataX = new double[minusDollar.Count];
                    for (int i = 0; i < minusDollar.Count; i++)
                    {
                        dataX[i] = i + 2;
                    }

                    double[] dataY = minusDollar.ToArray();

                    WpfPlotRussiaRubles.Plot.Clear();
                    WpfPlotRussiaRubles.Plot.AddScatter(dataX, dataY);
                    WpfPlotRussiaRubles.Plot.Title($"Scatter Plot of rubles and the other");
                    WpfPlotRussiaRubles.Plot.XLabel("Days");
                    WpfPlotRussiaRubles.Plot.YLabel("Changes");
                    WpfPlotRussiaRubles.Refresh();
                }
            }
        }

        private List<double> MinMaxRubles(bool dolOrEur, List<List<double>> list)
        {
            List<double> newList = new List<double>();
            foreach (var el in list)
            {
                if (dolOrEur)
                {
                    newList.Add(el[0]);
                }
                else
                {
                    newList.Add(el[1]);
                }
            }

            List<double> minusList = new List<double>();
            for (int i = 0; i < newList.Count - 1; i++)
            {
                var temp = newList[i + 1] - newList[i];
                minusList.Add(temp);
            }

            List<double> temps = new List<double>(minusList);
            temps.Sort();

            int min_day, max_day;
            double min, max;

            min_day = minusList.FindIndex(el => el == temps[0]) + 2;
            min = temps[0];

            max_day = minusList.FindIndex(el => el == temps[temps.Count - 1]) + 2;
            max = temps[temps.Count - 1];

            textBlockTask.Text += $"\nDay max = {max_day}, max = {max}\nDay min = {min_day}, min = {min} \n\n";

            return minusList;
        }

        private void shelepkoButton_Click(object sender, RoutedEventArgs e)
        {
                if (File.Exists(pathTextBox.Text))
                {
                    double[] nums = GetFromFile(pathTextBox.Text);
                    if (CheckArray(nums))
                    {
                        textBoxInformation.Clear();
                        textBlockTask.Text = "";
                    }
                }
        }

        private double[] GetFromFile(string path)
        {
            string file = File.ReadAllText(path);
            double[] nums = file
                .Trim()
                .Split(new char[] { ',', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                .Select
                (
                    n =>
                    {
                        if (double.TryParse(n, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                        {
                            return num;
                        }
                        else
                        {
                            return double.NaN;
                        }
                    }
                )
                .ToArray();
           return nums;
        }

        private bool CheckArray(double[] arr)
        {
            if (arr.Contains(double.NaN))
            {
                return false;
            }
            return true;
        }

        private List<List<double>> GetValuesInTable(double[] nums, string nameOne, string nameTwo)
        {
            List<List<double>> list = new List<List<double>>();
            List<double> newList = new List<double>();

            textBoxInformation.Text += nameOne + "     " + nameTwo + "\n";
            for (int i = 0; i < nums.Length; i++)
            {
                if (i % 2 != 0)
                {
                    newList.Add(nums[i]);
                    textBoxInformation.Text += nums[i] + "\n";

                    list.Add(newList);
                    newList = new List<double>();
                }
                else
                {
                    newList.Add(nums[i]);
                    textBoxInformation.Text += nums[i] + "     ";
                }
            }
            return list;
        }
    }
}
