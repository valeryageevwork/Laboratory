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
