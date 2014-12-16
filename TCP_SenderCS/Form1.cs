using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCP_SenderCS
{
    public partial class Form1 : Form
    {

        private const string securityToken = "xuhuixjsoh4s564c456dcsdc48d96c5cs";

        public Form1()
        {
            InitializeComponent();
            textBox4.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                label5.Text = "Empty host!";
            }
            else
            {
                label5.Text = "";
                pictureBox2.Visible = true;
                textBox1.Enabled = false;
                numericUpDown1.Enabled = false;
                textBox2.Enabled = false;
                textBox4.Enabled = false;
                button1.Enabled = false;

                sendData(textBox1.Text, Int32.Parse(numericUpDown1.Value.ToString()), textBox2.Text);

                receiveData(Int32.Parse(numericUpDown1.Value.ToString()));
            }
        }

        private void sendData(string host, int port, string str)
        {
            TcpClient tcpCl = null;
            try
            {
                tcpCl = new TcpClient(host, port);
                NetworkStream ns = tcpCl.GetStream();

                byte[] bytes = Encoding.UTF8.GetBytes(str + Environment.NewLine + securityToken + "#sender_hostname:#" + textBox4.Text);
                ns.Write(bytes, 0, bytes.Length);

                ns.Flush();
                ns.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                tcpCl.Close();
            }
            
        }

        private void receiveData(int port)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = null;

            try
            {
                tcpListener = new TcpListener(ip, port);
                tcpListener.Start();

                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                StreamReader sr = new StreamReader(tcpClient.GetStream());

                string outputText = "";
                bool isAnswer = false;
                while (true)
                {
                    string line = sr.ReadLine();
                    if (line == null || line == "end" || line == "exit")
                    {
                        break;
                    }
                    else if (line == securityToken)
                    {
                        isAnswer = true;
                    }
                    else
                    {
                        outputText += line;
                    }
                }

                if (isAnswer == true)
                {
                    textBox3.Text = outputText;
                    pictureBox2.Visible = false;
                    pictureBox1.Visible = true;
                }

                sr.Close();
                tcpClient.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                tcpListener.Stop();
            }
        }
    }
}
