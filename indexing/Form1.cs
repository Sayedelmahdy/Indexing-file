using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace indexing
{
    public partial class Form1 : Form
    {
        FileStream test;
        FileStream index;
        SortedDictionary<int, int> dic = new SortedDictionary<int, int>();
        public Form1()
        {
            InitializeComponent();
          

        }
        //64646654|sadasd|646464
        private void Form1_Load(object sender, EventArgs e)
        {
            test = new FileStream("test.txt", FileMode.Open, FileAccess.ReadWrite);
            index = new FileStream("index.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(index);
            StreamReader sr = new StreamReader(test);
            string l;
            int coun = 0;
            while((l=sr.ReadLine())!=null)
            {
                
                string[] arr = l.Split('|');
                if(l[0]=='*')
                {
                    coun += l.Length + 2;
                    continue;
                }
                sw.WriteLine(arr[0] + '|' + coun);
                dic.Add(Convert.ToInt32(arr[0]), coun);
                sw.Flush();
                coun += l.Length + 2;
            }
            sw.Close();
            sr.Close();
            test.Close();
            index.Close();
        }

        private void button5_Click(object sender, EventArgs e)//loadfile
        {
            test = new FileStream("test.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(test);
            textBox3.Text = sr.ReadToEnd();
            sr.Close();
            test.Close();
            MessageBox.Show("file has been loaded");
        }

        private void button1_Click(object sender, EventArgs e) // insert
        {
            test = new FileStream("test.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(test);
            test.Seek(0, SeekOrigin.End);
            int pos =Convert.ToInt32( test.Position);
            if(dic.ContainsKey(Convert.ToInt32(textBox1.Text)))
            {
                MessageBox.Show("This id was used before");
            }
            else
            {
                sw.WriteLine(textBox1.Text + '|' + textBox2.Text + '|' + textBox6.Text);
                dic.Add(Convert.ToInt32(textBox1.Text), pos);
            }
            textBox1.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            sw.Close();
            test.Close();
            rewrite();
           
        }

        private void button4_Click(object sender, EventArgs e) // load index
        {

            index = new FileStream("index.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(index);
            textBox3.Text = sr.ReadToEnd();
            sr.Close();
            index.Close();
            MessageBox.Show("file has been loaded");


        }
        public void rewrite ()
        {
            index = new FileStream("index.txt", FileMode.Truncate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(index);
            foreach (var it in dic)
            {
                sw.WriteLine(it.Key + "|" + it.Value);
                sw.Flush();
            }
            sw.Close();
            index.Close();
        }
        private void button6_Click(object sender, EventArgs e) 
        {
           Environment.Exit(0);

        }
        bool BinarySearch(int[] arr, int item)
        {
            int str = 0, end = arr.Length - 1;

            while (str <= end)
            {
                int mid = (end - str) / 2 + str;
                if (arr[mid] == item)
                {
                    return true;

                }
                else if (arr[mid]< item )
                {
                    str = mid + 1;
                }
                else
                {
                    end = mid - 1;
                }
            }
            return false;
        }
        private void button3_Click(object sender, EventArgs e) // Search
        {
            test = new FileStream("test.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(test);
            int[] arr = dic.Keys.ToArray();
            if (BinarySearch(arr, Convert.ToInt32(textBox4.Text)))
            {
                int pos = dic[Convert.ToInt32(textBox4.Text)];
                test.Seek(pos, SeekOrigin.Begin);
                string l = sr.ReadLine();
                MessageBox.Show(l);
            }
            else
            {
                MessageBox.Show("Not found");
            }
            textBox5.Text = "";
            rewrite();
            sr.Close();
            test.Close();

        }

        private void button2_Click(object sender, EventArgs e)//delete
        {
            test = new FileStream("test.txt", FileMode.Open, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(test);
            int[] arr = dic.Keys.ToArray();
            if (BinarySearch(arr,Convert.ToInt32( textBox5.Text)))
            {
                int pos = dic[Convert.ToInt32(textBox5.Text)];
                test.Seek(pos, SeekOrigin.Begin);
                sw.Write("*");
                dic.Remove(Convert.ToInt32(textBox5.Text));
            }
            else
            {
                MessageBox.Show("Not found");
            }
            textBox5.Text = "";
            rewrite();
            sw.Close();
            test.Close();
           // 123|40
        }

    }
}
