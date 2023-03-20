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
    class Searcher
    {
        SqlConnection connect;
        SqlDataAdapter ada;
        SqlCommandBuilder builder = new SqlCommandBuilder();
        DataSet one;

        public Searcher(SqlConnection connection)
        {
            connect = connection;
        }

        public void Loader(TextBox textBox, PictureBox pictureBox, ImageList imageList, Label label)
        {
            try
            {
                string comma = "select * from Films where [Name] = '" + textBox.Text + "';";
                ada = new SqlDataAdapter(comma, connect);
                one = new DataSet();
                ada.Fill(one);
                if (one.Tables[0].Rows.Count == 0)
                {
                    MessageBox.Show("Такого фильма нет в базе данных.");
                    pictureBox.Image = imageList.Images[0];
                    label.Text = "";
                }
                else
                {
                    Load_picture(textBox, pictureBox, imageList);
                    Load_information(textBox, label);
                }
        }
            catch
            {
                MessageBox.Show("Ошибка!");
                pictureBox.Image = imageList.Images[0];
                label.Text = "";
            }
}
        private void Load_picture(TextBox textBox, PictureBox pictureBox, ImageList imageList)
        {
            try
            {
                string comma = "select Cover from Films where [Name] = '" + textBox.Text + "';";
                ada = new SqlDataAdapter(comma, connect);
                one = new DataSet();
                ada.Fill(one);
                byte[] bytes = (byte[])one.Tables[0].Rows[0]["Cover"];
                MemoryStream ms = new MemoryStream(bytes);
                pictureBox.Image = Image.FromStream(ms);

            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке обложки произошла ошибка.");
                pictureBox.Image = imageList.Images[0];
            }
        }


        private void Load_information(TextBox textBox, Label label)
        {
            try
            {
                label.Text = "Название: " + textBox.Text + "\n";
                string comma = "select Year_of_release, Genre, Country, Budjet, Income, Income - Budjet as [Profit]," +
               "(select count(*) from Connection_country where Films.id =Film) as [Countries], " +
               "(select count(*) from Connection_lang where Films.id =Film) as [Languages], Rating " +
               "from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id and [Name] = '" + textBox.Text + "'; ";
                ada = new SqlDataAdapter(comma, connect);
                one = new DataSet();
                ada.Fill(one);
                label.Text += "Год выпуска: " + one.Tables[0].Rows[0]["Year_of_release"].ToString().ToCharArray()[6] + one.Tables[0].Rows[0]["Year_of_release"].ToString().ToCharArray()[7] + one.Tables[0].Rows[0]["Year_of_release"].ToString().ToCharArray()[8] + one.Tables[0].Rows[0]["Year_of_release"].ToString().ToCharArray()[9] + "\n";
                label.Text += "Жанр: " + one.Tables[0].Rows[0]["Genre"].ToString() + "\n";
                label.Text += "Страна: " + one.Tables[0].Rows[0]["Country"].ToString() + "\n";
                label.Text += "Бюджет фильма: " + one.Tables[0].Rows[0]["Budjet"].ToString() + "$" + "\n";
                label.Text += "Кассовые сборы: " + one.Tables[0].Rows[0]["Income"].ToString() + "$" + "\n";
                label.Text += "Прибыль: " + one.Tables[0].Rows[0]["Profit"] + "$" + "\n";
                label.Text += "Количество стран, в которых был показан данный фильм: " + one.Tables[0].Rows[0]["Countries"].ToString() + "\n";
                label.Text += "Количество языков, на которые был переведён данный фильм:  " + one.Tables[0].Rows[0]["Languages"].ToString() + "\n";
                label.Text += "Рейтинг фильма: " + one.Tables[0].Rows[0]["Rating"].ToString() + "\n";
            }
            catch(Exception ex)
            {
                MessageBox.Show("При загрузке данных произошла ошибка.");
                label.Text = "";
            }
        }
    }
}
