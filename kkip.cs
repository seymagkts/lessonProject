using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace proje
{
    public partial class Form1 : Form
    {
        // Database baglanti parametresinin guncellenmesi gerekmektedir. Kendi bilgisayarimdaki database'e baglaniyor.
        SqlConnection baglan = new SqlConnection(@"Server=DESKTOP-L5EBS4A\SGOKTAS; Initial Catalog=projee; Integrated Security= SSPI");

        SqlCommand komut;
        SqlDataAdapter getir;
        DataSet sakla;


        public Form1()
        {
            InitializeComponent();
            comboBox3.Items.Add("K");
            comboBox3.Items.Add("E");

            comboBox4.Items.Add("K");
            comboBox4.Items.Add("E");

        }

        public void listele()
        {
            baglan.Open();
            getir = new SqlDataAdapter("Select * from Kabahat1 order by KabahatNO asc", baglan);
            sakla = new DataSet();
            getir.Fill(sakla);
            dataGridView1.DataSource = sakla.Tables[0];
            baglan.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            listele();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'projeeDataSet.Kabahatler' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            this.kabahatlerTableAdapter.Fill(this.projeeDataSet.Kabahatler);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglan.Open();

            komut = new SqlCommand("insert into Kabahat1(KabahatNO, AdSoyad, KabahatMaddesi, Tarih, Cinsiyet) values(@KabahatNO, @ad, @madde, @tarih, @cinsiyet)", baglan);
            SqlCommand sorgu = new SqlCommand("select max(KabahatNO)+1 from Kabahat1", baglan);
            komut.Parameters.AddWithValue("@KabahatNo", sorgu.ExecuteScalar());
            komut.Parameters.AddWithValue("@ad", textBox1.Text);
            komut.Parameters.AddWithValue("@madde", comboBox1.Text);
            komut.Parameters.AddWithValue("@tarih", dateTimePicker1.Value);
            komut.Parameters.AddWithValue("@cinsiyet", comboBox4.Text);

            komut.ExecuteNonQuery();

            baglan.Close();
            listele();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            label6.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            comboBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label6.Text == "") MessageBox.Show("Güncellenecek kaydı seçiniz.");
            else
            {
                baglan.Open();
                komut = new SqlCommand("update Kabahat1 set AdSoyad = @ad, KabahatMaddesi = @madde, Tarih = @tarih, Cinsiyet = @cinsiyet where KabahatNO = @Kabahat", baglan);
                komut.Parameters.AddWithValue("@Kabahat", label6.Text);
                komut.Parameters.AddWithValue("@ad", textBox1.Text);
                komut.Parameters.AddWithValue("@madde", comboBox1.Text);
                komut.Parameters.AddWithValue("@tarih", dateTimePicker1.Value);
                komut.Parameters.AddWithValue("@cinsiyet", comboBox4.Text);

                komut.ExecuteNonQuery();

                baglan.Close();
                listele();

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (label6.Text == "") MessageBox.Show("Silinecek kaydı seçiniz.");
            else
            {
                baglan.Open();
                komut = new SqlCommand("delete from Kabahat1 where KabahatNO = @Kabahat", baglan);
                komut.Parameters.AddWithValue("@Kabahat", label6.Text);

                komut.ExecuteNonQuery();

                baglan.Close();
                listele();
            }

        }

        public string arananlar()
        {
            string aranan = " ";

            if (textBox3.Text.Length != 0) aranan += " and AdSoyad like '" + textBox3.Text + "%'";
            if (checkBox1.Checked == true) aranan += " and Tarih= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
            if (checkBox2.Checked == true) aranan += " and KabahatMaddesi= '" + comboBox2.Text + "'";
            if (checkBox3.Checked == true) aranan += " and Cinsiyet='" + comboBox3.Text + "'";

            return aranan;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            baglan.Open();
            string aranan = arananlar();
            SqlDataAdapter getir2 = new SqlDataAdapter("Select AdSoyad, Tarih, KabahatMaddesi, Cinsiyet from Kabahat1 where KabahatNo != 0 " + aranan, baglan);
            DataSet sakla2 = new DataSet();
            getir2.Fill(sakla2);

            dataGridView2.DataSource = sakla2.Tables[0];
            baglan.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) dateTimePicker2.Enabled = true;
            else
            {
                dateTimePicker2.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true) comboBox2.Enabled = true;
            else
            {
                comboBox2.Enabled = false;
                comboBox2.Text = "";
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true) comboBox3.Enabled = true;
            else
            {
                comboBox3.Enabled = false;
                comboBox3.Text = "";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (label8.Text.Length != 0) MessageBox.Show("İdari para cezası iki katına çıkarıldı.");
            else { MessageBox.Show("Kayıt bulunamadı."); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlDataAdapter getir3 = new SqlDataAdapter("Select AdSoyad, k.KabahatMaddesi,  KabahatTürü , İPCMiktarı from Kabahat1 k , Kabahatler kk where k.KabahatMaddesi=kk.KabahatMaddesi Group By AdSoyad, k.KabahatMaddesi,  KabahatTürü , İPCMiktarı Having Count(AdSoyad) > 1 and (k.KabahatMaddesi) > 1", baglan);
            DataSet sakla3 = new DataSet();
            getir3.Fill(sakla3);

            dataGridView3.DataSource = sakla3.Tables[0];
            baglan.Close();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            label8.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
            label9.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            label6.Text = "";
        }
    }
}

