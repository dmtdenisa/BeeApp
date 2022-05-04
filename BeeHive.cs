using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using beeAppLibrary;

namespace BeeApp
{
    public partial class BeeHive : Form
    {
        Hive hive = new Hive();
        string historyPath = @"C:\Users\dumit\source\repos\BeeApp\Reports\colonyActivityHistory.txt";
        string infoPath = @"C:\Users\dumit\source\repos\BeeApp\Reports\colonyInfo.txt";


        public BeeHive()
        {
            
            InitializeComponent();
            hiveCapacityBar.Value = 0;
            stressLevelBar.Value = 0;
            
        }

        private void BeeHive_Load(object sender, EventArgs e)
        {
            setUpComboBox();
            richTextBox1.AppendText($"Hive created: {Hive._dateOfCreation}\n");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string enteredID = textBox1.Text.Trim().ToLower();
            if (radioButton1.Checked && textBox1.Text.Trim().Length > 0)
            {
                hive._inhabitants.Add(new QueenBee(enteredID));
                stressLevelBar.Value += QueenBee.StressLevel;
                radioButton1.Checked = false;
                radioButton1.Enabled = false;
                hiveCapacityBar.Value++;
                richTextBox1.AppendText($"New bee was welcomed into the hive: {enteredID}!\n");
            }
            else if (radioButton2.Checked && textBox1.Text.Trim().Length > 0)
            {
                hive._inhabitants.Add(new DroneBee(textBox1.Text.Trim().ToLower()));
                stressLevelBar.Value += DroneBee.StressLevel;
                radioButton2.Checked = false;
                hiveCapacityBar.Value++;
                richTextBox1.AppendText($"New bee was welcomed into the hive: {enteredID}!\n");
            }
            else if (radioButton3.Checked && textBox1.Text.Trim().Length > 0)
            {
                hive._inhabitants.Add(new ForagerBee(textBox1.Text.Trim().ToLower()));
                stressLevelBar.Value += ForagerBee.StressLevel;
                radioButton3.Checked = false;
                hiveCapacityBar.Value++;
                richTextBox1.AppendText($"New bee was welcomed into the hive: {enteredID}!\n");
            }
            else if (radioButton4.Checked && textBox1.Text.Trim().Length > 0)
            {
                hive._inhabitants.Add(new HiveBee(textBox1.Text.Trim().ToLower()));
                stressLevelBar.Value += HiveBee.StressLevel;
                radioButton4.Checked = false;
                hiveCapacityBar.Value++;
                richTextBox1.AppendText($"New bee was welcomed into the hive: {enteredID}!\n");
            }
            else
            {
                MessageBox.Show("You forgot to name your bee and/or choose its type!");
            }
            setUpComboBox();
            textBox1.Clear();
            timer1.Start();
            timer2.Start();
        }

        private void chosenWorkBeeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string chosenBeeType = chosenWorkBeeCB.SelectedItem.GetType().Name;
                label4.Text = chosenBeeType;
            }
            catch
            {
                label4.Text = " ";
            }

        }

        public void setUpComboBox()
        {
            BindingList<HoneyBee> listaSursa = new BindingList<HoneyBee>(hive._inhabitants);
            BindingSource sursaCB = new BindingSource(listaSursa, null);
            chosenWorkBeeCB.DataSource = sursaCB;
            chosenWorkBeeCB.DisplayMember = "ID";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HoneyBee selectedBee = (HoneyBee)chosenWorkBeeCB.SelectedItem;
            richTextBox1.AppendText(selectedBee.DoWork());
            switch (selectedBee.GetType().Name)
            {
                case "WorkingClass":
                case "HoneyBee":
                    break;
                case "DroneBee":
                    reduceFoodResources();
                    break;
                case "HiveBee":
                    reduceHiveStress();
                    increaseBeeHapiness();
                    break;
                case "ForagerBee":
                    addFoodResources();
                    break;
                case "QueenBee":
                    increaseBeeHapiness();
                    break;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (HoneyBee bee in hive._inhabitants)
            {
                //albinele ar trebui sa creasca la fiecare ~aproximativ~ 2 minute de la "nastere"
                if (Math.Abs(bee.dateOfBirth.Subtract(DateTime.Now).Minutes) % 2 == 0 && Math.Abs(bee.dateOfBirth.Subtract(DateTime.Now).Minutes) > 0)
                {
                    bee.Age++;
                    richTextBox1.AppendText($"{bee.ID} has aged! Age: {bee.Age}\n");
                }

            }

        }

        public void reduceHiveStress()
        {
            if (stressLevelBar.Value >= 1)
            {
                stressLevelBar.Value--;
            }
        }

        public void addFoodResources()
        {
            if (progressBar2.Value <= 90)
            {
                progressBar2.Value += 10;
            }
        }

        public void increaseBeeHapiness()
        {
            if (progressBar1.Value <= 90)
            {
                progressBar1.Value += 10;
            }
        }

        public void decreaseBeeHapiness()
        {
            if (progressBar1.Value >= 10)
            {
                progressBar1.Value -= 10;
            }
        }

        public void reduceFoodResources()
        {
            if (progressBar2.Value >= 10)
            {
                progressBar2.Value -= 10;
            }
        }

        public void increaseHiveStress()
        {
            if (stressLevelBar.Value <20)
            {
                stressLevelBar.Value++;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            reduceFoodResources();
            decreaseBeeHapiness();
            increaseHiveStress();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            using(StreamWriter writer = new StreamWriter(infoPath))
            {
                writer.WriteLine(hive._inhabitants.ToString());
                
                
            }
            MessageBox.Show("You can find the text file in the Reports folder!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
            richTextBox1.SaveFile(historyPath, RichTextBoxStreamType.PlainText);
            MessageBox.Show("You can find the text file in the Reports folder!");
        }
    }

    
}
