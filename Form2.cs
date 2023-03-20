using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace Filmography
{
    public partial class Form2 : Form
    {

        SqlConnection connect;
        SqlDataAdapter adapter;
        DataSet saver;
        string server = @"Data Source = NAME_OF_YOUR_SERVER; " +
            @"Initial catalog = Filmography; " +
            @"Integrated security = SSPI";
        public Form2()
        {
            InitializeComponent();
            connect = new SqlConnection(server);
            Refresh();
            MessageBox.Show("Режим редактирования - удобство изменения базы данных. Здесь Вы имеете доступ к 3 таблицам бд для добавления новых строк, удаления ненужных элементов, внесения изменений в данные и загрузки и изменения обложек фильмов.");
            button4.Enabled = false;
            button2.Enabled = false;
        }

        private void Refresh()
        {
            if (radioButton1.Checked)
            {
                string sqlcom = "select * from Films ";

                adapter = new SqlDataAdapter(sqlcom, connect);
                SqlCommandBuilder b = new SqlCommandBuilder(adapter);
                saver = new DataSet();
                adapter.Fill(saver, "Films");
                dataGridView1.DataSource = saver.Tables["Films"];
                button2.Enabled = true;
                button4.Enabled = true;
            }
            else if (radioButton2.Checked)
            {
                string sqlcom = "select * from Genres ";

                adapter = new SqlDataAdapter(sqlcom, connect);
                SqlCommandBuilder b = new SqlCommandBuilder(adapter);
                saver = new DataSet();
                adapter.Fill(saver, "Genres");
                dataGridView1.DataSource = saver.Tables["Genres"];
                button2.Enabled = false;
                button4.Enabled = false;
            }
            else if (radioButton3.Checked)
            {
                string sqlcom = "select * from Countries ";

                adapter = new SqlDataAdapter(sqlcom, connect);
                SqlCommandBuilder b = new SqlCommandBuilder(adapter);
                saver = new DataSet();
                adapter.Fill(saver, "Countries");
                dataGridView1.DataSource = saver.Tables["Countries"];
                button2.Enabled = false;
                button4.Enabled = false;
            }
            
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                string update = @"update Films set Name=@newName, Year_of_release=@newYear_of_release, GenreCODE = @newGenreCODE, CountryCODE=@newCountryCODE, Budjet=@newBudjet, Income=@newIncome, Rating = @newRating where id=@idC;";
                SqlCommand command = new SqlCommand(update, connect);

                command.Parameters.Add(new SqlParameter("@idC", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@newName", SqlDbType.NVarChar, 100));
                command.Parameters.Add(new SqlParameter("@newYear_of_release", SqlDbType.Date));
                command.Parameters.Add(new SqlParameter("@newGenreCODE", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@newCountryCODE", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@newBudjet", SqlDbType.Money));
                command.Parameters.Add(new SqlParameter("@newIncome", SqlDbType.Money));
                command.Parameters.Add(new SqlParameter("@newRating", SqlDbType.Float));

                command.Parameters["@newName"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@newYear_of_release"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@newGenreCODE"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@newCountryCODE"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@newBudjet"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@newIncome"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@newRating"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@idC"].SourceVersion = DataRowVersion.Original;

                command.Parameters["@newName"].SourceColumn = "Name";
                command.Parameters["@newYear_of_release"].SourceColumn = "Year_of_release";
                command.Parameters["@newGenreCODE"].SourceColumn = "GenreCODE";
                command.Parameters["@newCountryCODE"].SourceColumn = "CountryCODE";
                command.Parameters["@newBudjet"].SourceColumn = "Budjet";
                command.Parameters["@newIncome"].SourceColumn = "Income";
                command.Parameters["@newRating"].SourceColumn = "Rating";
                command.Parameters["@idC"].SourceColumn = "id";

                adapter = new SqlDataAdapter("", connect);
                adapter.UpdateCommand = command;
                adapter.Update(saver, "Films");
                Refresh();
            }
            
            else if (radioButton2.Checked)
            {
                string update = @"update Genres set Genre=@newGenre where id=@idC;";
                SqlCommand command = new SqlCommand(update, connect);

                command.Parameters.Add(new SqlParameter("@idC", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@newGenre", SqlDbType.NVarChar, 50));

                command.Parameters["@newGenre"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@idC"].SourceVersion = DataRowVersion.Original;

                command.Parameters["@newGenre"].SourceColumn = "Genre";
                command.Parameters["@idC"].SourceColumn = "id";

                adapter = new SqlDataAdapter("", connect);
                adapter.UpdateCommand = command;
                adapter.Update(saver, "Genres");
                Refresh();
            }
            else if (radioButton3.Checked)
            {
                string update = @"update Countries set Country=@newCountry where id=@idC;";
                SqlCommand command = new SqlCommand(update, connect);

                command.Parameters.Add(new SqlParameter("@idC", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@newCountry", SqlDbType.NVarChar, 50));

                command.Parameters["@newCountry"].SourceVersion = DataRowVersion.Current;
                command.Parameters["@idC"].SourceVersion = DataRowVersion.Original;

                command.Parameters["@newCountry"].SourceColumn = "Country";
                command.Parameters["@idC"].SourceColumn = "id";

                adapter = new SqlDataAdapter("", connect);
                adapter.UpdateCommand = command;
                adapter.Update(saver, "Countries");
                Refresh();
            }
        }
        string path = "";
        private void Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Формат jpeg|*.jpg|Формат png|*.png";
            openFileDialog.FileName = "";
            openFileDialog.InitialDirectory = @"h:\";
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                LoadPicture();
            }
            Refresh();
        }

        private void LoadPicture()
        {
            byte[] pic;
            pic = CreatePicture();
            connect.Open();
            int index = 1;
            int.TryParse(textBox1.Text, out index);
            if (index == -1)
            {
                connect.Close();
                return;
            }
            if (textBox1.Text == null || textBox1.Text.Length == 0)
            {
                connect.Close();
                return;
            }
            string update = @"update Films set Cover= @newImage where id =" + index + ";"; 
            SqlCommand sqlCommand = new SqlCommand(update, connect);
            sqlCommand.Parameters.Add("@newImage", SqlDbType.VarBinary, pic.Length).Value = pic;
            sqlCommand.ExecuteNonQuery();
            connect.Close();
        }

        private byte[] CreatePicture()
        {
            Image img = Image.FromFile(path);
            int maxwidth = 300;
            int maxheight = 300;
            double rationX = (double)maxwidth / img.Width;
            double rationY = (double)maxheight / img.Height;
            double ration = Math.Min(rationX, rationY);
            int newWidth = (int)(ration * img.Width);
            int newHeight = (int)(ration * img.Height);

            Image newimg = new Bitmap(newWidth, newHeight);
            Graphics graphics = Graphics.FromImage(newimg);
            graphics.DrawImage(img, 0, 0, newWidth, newHeight);
            MemoryStream ms = new MemoryStream();
            newimg.Save(ms, ImageFormat.Jpeg);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(ms);
            byte[] array = br.ReadBytes((int)ms.Length);
            return array;
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                adapter.Update(saver, "Films");
                Refresh();
            }

            else if (radioButton2.Checked)
            {
                adapter.Update(saver, "Genres");
                Refresh();
            }
            else if (radioButton3.Checked)
            {
                adapter.Update(saver, "Countries");
                Refresh();
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            try {
                if (textBox2.Text != "")
                {
                    int ind = -1;
                    int.TryParse(textBox2.Text, out ind);
                    if (ind != -1)
                    {
                        string s = "select * from Films where id =" + ind + ";";
                        SqlDataAdapter adapt = new SqlDataAdapter(s, connect);
                        SqlCommandBuilder b = new SqlCommandBuilder(adapt);
                        DataSet check = new DataSet();
                        adapt.Fill(check);
                        if (check.Tables[0].Rows.Count == 0)
                        {
                            MessageBox.Show("Строки с таким id не существует.");
                        }
                        else
                        {
                            check.Tables[0].Rows[0].Delete();
                            adapt.Update(check);
                            Refresh();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при вводе id");
                    }

                }
                else
                {
                    MessageBox.Show("Выберите id строки для удаления и внесите его в колонку.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("У Вас нет прав удалить данный фильм!");
            }
            
        }
    }
}
