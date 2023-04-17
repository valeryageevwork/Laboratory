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
                }
            }
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
                        
                        List<List<double>> list = GetValuesInTable(nums, "YEAR", "PEOPLE");
                        textBlockTask.Text += "Procent:";
                        List<double> minusList = ProcentPeople(list);

                        List<double> newListX = new List<double>();
                        List<double> newListY = new List<double>();
                        foreach (var el in list)
                        {
                            newListX.Add(el[0]);
                            newListY.Add(el[1]);
                        }
                        double[] dataX = newListX.ToArray();
                        double[] dataY = newListY.ToArray();

                        WpfPlotRussiaRubles.Plot.AddScatter(dataX, dataY);
                        WpfPlotRussiaRubles.Refresh();
                    }
                }
        }

        private List<double> ProcentPeople(List<List<double>> list)
        {
            List<double> newList = new List<double>();
            foreach (var el in list)
            {
                newList.Add(el[1]);
            }

            List<double> minusList = new List<double>();
            for (int i = 0; i < newList.Count - 1; i++)
            {
                var temp = newList[i + 1] - newList[i];
                temp /= newList[i];
                temp *= 100;
                minusList.Add(temp);
            }
            textBlockTask.Text += "\n";
            for (int i = 0; i < minusList.Count; i++)
            {
                textBlockTask.Text += minusList[i].ToString() + "\n";
            }

            return minusList;
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
