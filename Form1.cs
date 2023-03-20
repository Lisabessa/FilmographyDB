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
using System.Drawing.Imaging;
using System.IO;

namespace Filmography
{
    public partial class Form1 : Form
    {
        SqlConnection connect;
        string server = @"Data Source = NAME_OF_YOUR_SERVER; " +
            @"Initial catalog = Filmography; " +
            @"Integrated security = SSPI";
        Searcher searcher;
        public Form1()
        {
            InitializeComponent();
            connect = new SqlConnection();
            connect.ConnectionString = server;
            Refresh(1);
            searcher = new Searcher(connect);
            
        }
        string sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна],  Budjet as [Бюджет], Income as [Сборы], " +
            "(select count(*) from Connection_country where Films.id =Film) as [Страны показа], " +
            "(select count(*) from Connection_lang where Films.id =Film) as [Языки перевода], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id; ";
        private void Refresh(int index)
        {
            SqlCommand command = new SqlCommand(sqltext, connect);
            connect.Open();
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            int ind = 0;
            do
            {
                while (reader.Read())
                {
                    if (ind == 0)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            table.Columns.Add(reader.GetName(i));
                        }
                        ind++;
                    }

                    DataRow row = table.NewRow();
                    for (int i = 0; i < reader.FieldCount; i++)
                    { 
                            row[i] = reader[i];
    
                    }
                    table.Rows.Add(row);
                }

            } while (reader.NextResult());
          
            if(index == 1)
            {
                dataGridView1.DataSource = table;
            }
            else if(index == 2)
            {
                dataGridView2.DataSource = table;
            }
            connect.Close();
        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "RAZRAB2004")
            {
                Form2 newform = new Form2();
                newform.ShowDialog();
            }

            Refresh(1);
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна], Budjet as [Бюджет], Income as [Сборы], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id ORDER BY Year_of_release; ";
                
            }
            else if(comboBox1.SelectedIndex == 1)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна], Budjet as [Бюджет], Income as [Сборы], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id ORDER BY [Name]; ";
            }
            else if(comboBox1.SelectedIndex == 2)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна], Budjet as [Бюджет], Income as [Сборы], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id ORDER BY Budjet; ";
            }
            else if(comboBox1.SelectedIndex == 3)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна], Budjet as [Бюджет], Income as [Сборы], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id ORDER BY Country; ";
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна], Budjet as [Бюджет], Income as [Сборы], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id ORDER BY Rating; ";
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна], Budjet as [Бюджет], Income as [Сборы], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id ORDER BY Income; ";
            }
            else if (comboBox1.SelectedIndex == 6)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна], Budjet as [Бюджет], Income as [Сборы], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id ORDER BY (Income - Budjet); ";
            }
            else if (comboBox1.SelectedIndex == 7)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна], Budjet as [Бюджет], Income as [Сборы], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id ORDER BY Genre; ";
            }
            else if (comboBox1.SelectedIndex == 8)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
                "Genre as [Жанр], Country as [Страна],  Budjet as [Бюджет], Income as [Сборы], " +
                "(select count(*) from Connection_country where Films.id =Film) as [Страны показа], " +
                "(select count(*) from Connection_lang where Films.id =Film) as [Языки перевода], Rating as [Рейтинг], " +
                "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id " +
                "ORDER BY (select count(*) from Connection_lang where Films.id =Film); ";
            }
            else if (comboBox1.SelectedIndex == 9)
            {
                sqltext = "select Films.id, [Name] as [Название], Year_of_release as [Год выпуска], " +
               "Genre as [Жанр], Country as [Страна],  Budjet as [Бюджет], Income as [Сборы], " +
               "(select count(*) from Connection_country where Films.id =Film) as [Страны показа], " +
               "(select count(*) from Connection_lang where Films.id =Film) as [Языки перевода], Rating as [Рейтинг], " +
               "Cover as [Обложка] from Films, Genres, Countries where Films.GenreCODE = Genres.id and Films.CountryCODE = Countries.id " +
               "ORDER BY (select count(*) from Connection_country where Films.id =Film); ";
            }
            Refresh(2);
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if(textBox2.Text != "")
            {
                searcher.Loader(textBox2, pictureBox2, imageList1, label8);
            }
        }
    }
}
