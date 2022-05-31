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
        SortedDictionary<string, int> dick = new SortedDictionary<string, int>();
        public Form1()
        {
            InitializeComponent();
          

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int c = 0;
          test = new FileStream("test.txt",FileMode.Open,FileAccess.ReadWrite);
           index = new FileStream("indexfile.txt",FileMode.Open,FileAccess.ReadWrite);
            StreamReader stt = new StreamReader(test);
            StreamWriter swi = new StreamWriter(index);
            string l;
            while ((l = stt.ReadLine()) != null)
            {
                c += l.Length;
                dick.Add(l, c);
                swi.WriteLine(l+","+c) ;
                c += 2;
            }
            test.Close();
            index.Close();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("test.txt");
            textBox3.Text = sr.ReadToEnd();
            sr.Close();
        }

        private void button1_Click(object sender, EventArgs e) // insert
        {
            FileStream flst = new FileStream("test.txt", FileMode.Open, FileAccess.Write);
            StreamWriter sw = new StreamWriter(flst);
            flst.Seek(0, SeekOrigin.End);
            int pos =Convert.ToInt32( flst.Position);
            sw.WriteLine(textBox1.Text);
            pos += textBox1.Text.Length;
            pos += 2;
            dick.Add(textBox1.Text, pos);
            sw.Close();
            flst.Close();
            MessageBox.Show("inserted");

        }

        private void button4_Click(object sender, EventArgs e) // load index
        {
            dick.Clear();
            StreamReader sr = new StreamReader("indexfile.txt");
            string l;
            textBox3.Text = "KEY\tLOC\r\n";
            while ((l=sr.ReadLine())!=null)
            {
                string[] arr = l.Split(',');
                dick.Add(arr[0], Convert.ToInt32(arr[1]));
                textBox3.Text += arr[0] + "\t" + arr[1] + "\r\n";
            }
            sr.Close();
            MessageBox.Show("index file has been loaded");
           

        }

        private void button6_Click(object sender, EventArgs e) // rewrite
        {
            FileStream fs = new FileStream("indexfile.txt", FileMode.Truncate, FileAccess.Write);
            StreamWriter sm = new StreamWriter(fs);
            fs.Seek(0, SeekOrigin.Begin);
            foreach (KeyValuePair<string,int> it in dick)
            {
                sm.Write(it.Key +"," + it.Value+"\r\n");
                 sm.Flush();
            }
            fs.Close();
            MessageBox.Show("Rewrite Succseful");

        }
        bool BinarySearch(string[] arr, string item)
        {
            int str = 0, end = arr.Length - 1;

            while (str <= end)
            {
                int mid = (end - str) / 2 + str;
                if (arr[mid] == item)
                {
                    return true;

                }
                else if (String.Compare(arr[mid], item) < 0)
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
            string[] arr = dick.Keys.ToArray();
            if (BinarySearch(arr, textBox4.Text))
            {

                MessageBox.Show("The location of this item is : "+
                   Convert.ToString(dick[textBox4.Text])
                    ) ;
            }
            else
            {
                MessageBox.Show("Not Found");
            }

        }

        private void button2_Click(object sender, EventArgs e)//delete
        {
            
            string[] arr = dick.Keys.ToArray();
            if (BinarySearch(arr, textBox5.Text))
            {
                int coun = 0;
                dick.Remove(textBox5.Text);
                FileStream sf = new FileStream("test.txt", FileMode.Open, FileAccess.ReadWrite);
                StreamReader st = new StreamReader(sf);
                st.DiscardBufferedData();
                StreamWriter sw = new StreamWriter(sf);
                string l;
                while((l=st.ReadLine())!=null)
                {
                    
                    if(l==textBox5.Text)
                    {
                        sf.Seek(coun, SeekOrigin.Begin);
                        sw.Write("*");
                        sw.Flush();
                        break;
                    }
                    coun += l.Length + 2;
                }
                sf.Close();
               
            }
            else
            {
                MessageBox.Show("Not Found");
            }
        }
    }
}
