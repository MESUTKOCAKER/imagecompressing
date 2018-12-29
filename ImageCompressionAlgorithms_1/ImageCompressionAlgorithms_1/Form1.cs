using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageCompressionAlgorithms_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Width += 1;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Width = this.Width / 2 - 1;
            label2.Left = pictureBox2.Left + 3;
            label2.Top = pictureBox2.Top + 3;
        }

        private Image openImage()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Jpg|*.jpg";
            if (file.ShowDialog() == DialogResult.OK)
            {
                label3.Text = (file.FileName.Split('\\')[file.FileName.Split('\\').Length - 1]);
                return Image.FromFile(file.FileName);
            }
            else
            {
                MessageBox.Show("Please, Image Selected");
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Image image = openImage();
            if (image != null)
            {
                pictureBox1.Image = image;
                button4.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            compressing();
        }

        private void compressing()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            #region MyRegion
            Bitmap bit = new Bitmap(pictureBox1.Image);
            Bitmap compres = new Bitmap(bit.Width / 2, bit.Height / 2);
            int xx = 0, yy = 0;
            for (int i = 0; i < bit.Width - 1; i += 2)
            {
                yy = 0;
                for (int j = 0; j < bit.Height - 1; j += 2)
                {
                    int color11 = (bit.GetPixel(i, j).R + bit.GetPixel(i, j).G + bit.GetPixel(i, j).B) / 3;

                    int color12 = (bit.GetPixel(i, j + 1).R + bit.GetPixel(i, j + 1).G + bit.GetPixel(i, j + 1).B) / 3;

                    int color21 = (bit.GetPixel(i + 1, j).R + bit.GetPixel(i + 1, j).G + bit.GetPixel(i + 1, j).B) / 3;

                    int color22 = (bit.GetPixel(i + 1, j + 1).R + bit.GetPixel(i + 1, j + 1).G + bit.GetPixel(i + 1, j + 1).B) / 3;

                    compres.SetPixel(xx, yy, Color.FromArgb(color11, color12, color21, color22));
                    yy++;
                }
                xx++;
            }
            pictureBox2.Image = compres;
            pictureBox2.Image.Save(Application.StartupPath + "/imgComp/as" + label3.Text);
            #endregion

            sw.Stop();

            double frequ = Stopwatch.Frequency;
            double nanoSec = (Math.Pow(1000, 3)) / frequ;
            double stopNanoSec = sw.ElapsedTicks * nanoSec;

            MessageBox.Show("Successful Saving");
            MessageBox.Show(string.Format("MiliSecond : {0}\nNanoSecond : {1}", sw.ElapsedMilliseconds.ToString(), stopNanoSec.ToString()));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            viewOrginal();
        }

        private void viewOrginal()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            #region MyRegion
            Bitmap bit = new Bitmap(pictureBox1.Image); // 100
            Bitmap compres = new Bitmap(bit.Width * 2, bit.Height * 2); //200
            int xx = 0, yy = 0;
            for (int i = 0; i < compres.Width - 1; i += 2)
            {
                yy = 0;
                for (int j = 0; j < compres.Height - 1; j += 2)
                {
                    int a, r, g, b;
                    a = bit.GetPixel(xx, yy).A;
                    r = bit.GetPixel(xx, yy).R;
                    g = bit.GetPixel(xx, yy).G;
                    b = bit.GetPixel(xx, yy).B;

                    compres.SetPixel(i, j, Color.FromArgb(a, a, a));
                    compres.SetPixel(i, j + 1, Color.FromArgb(r, r, r));
                    compres.SetPixel(i + 1, j, Color.FromArgb(g, g, g));
                    compres.SetPixel(i + 1, j + 1, Color.FromArgb(b, b, b));
                    yy++;
                }
                xx++;
            }
            pictureBox2.Image = compres;
            pictureBox2.Image.Save(Application.StartupPath + "/imgComp/ViewOrginal_" + label3.Text);
            #endregion

            sw.Stop();

            double frequ = Stopwatch.Frequency;
            double nanoSec = (Math.Pow(1000, 3)) / frequ;
            double stopNanoSec = sw.ElapsedTicks * nanoSec;

            MessageBox.Show("Successfully Viewed Original");
            MessageBox.Show(string.Format("MiliSecond : {0}\nNanoSecond : {1}", sw.ElapsedMilliseconds.ToString(), stopNanoSec.ToString()));

        }

        private void imageSave(Image image)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "jpg|*.jpg";
            if (save.ShowDialog() == DialogResult.OK)
            {
                image.Save(save.FileName);
                MessageBox.Show("Successfully Save Image");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                bit = new Bitmap(pictureBox1.Image);
                xx1 = 0;
                timer1.Start();
            }
        }

        int xx1 = 0;
        Bitmap bit;
        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                if (xx1 < bit.Width)
                {
                    timer1.Stop();
                    for (int j = 0; j < bit.Height; j++)
                    {
                        int color = (bit.GetPixel(xx1, j).R + bit.GetPixel(xx1, j).G + bit.GetPixel(xx1, j).B) / 3;
                        bit.SetPixel(xx1, j, Color.FromArgb(color, color, color));
                    }
                    pictureBox1.Image = bit;
                    xx1++;

                    timer1.Start();
                }
                else
                {
                    i = 151;
                    pictureBox1.Image = bit;
                    timer1.Stop();
                    imageSave(pictureBox1.Image);
                }
            }
        }
    }
}
