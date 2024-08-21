using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Data.OleDb;
namespace Otomasyon_LeftSoft
{
    public partial class Arayuz : Form
    {
        public Arayuz()
        {
            InitializeComponent();
        }
        
        private void Arayuz_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        SqlBaglantisi bgln = new SqlBaglantisi();
        private void timer1_Tick(object sender, EventArgs e)
        {
            XmlTextReader oku = new XmlTextReader("appUpdate.xml");

            string elemanAdi = "";
            string versionno = "";
            try
            {


                oku.MoveToContent();
                if ((oku.NodeType == XmlNodeType.Element) && (oku.Name == "program"))
                {
                    while (oku.Read())
                    {
                        if (oku.NodeType == XmlNodeType.Element)
                        {
                            elemanAdi = oku.Name;
                        }
                        else
                        {
                            if ((oku.NodeType == XmlNodeType.Text) && (oku.HasValue))
                            {
                                switch (elemanAdi)
                                {
                                    case "version":
                                        versionno = oku.Value;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            string verno;
            verno = Properties.Settings.Default.versionapp;
            Properties.Settings.Default.versionapp = versionno;
            Properties.Settings.Default.Save();
            if (versionno != verno)
            {
                MessageBox.Show("Yeni Güncelleme Mevcut Uygulamanız Kapatılıp Güncelleme Servisi Açılacaktır - LeftSoft", "LeftSoft - Güncelleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("guncellemeServisi.exe");
                Application.Exit();
            }
        }
        DataTable dt;
        OleDbDataAdapter da;
        void tabloekle()
        {
             dt = new DataTable();
             da = new OleDbDataAdapter("Select * From Satis", bgln.sqlbaglan());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void position(Button b)
        {
            p1.Location = new Point(b.Location.X - p1.Width, b.Location.Y);
        }
        private void Active(Button b)
        {
            foreach(Control ctr in p1.Controls){
                if (ctr.GetType()==typeof(Button))
                {
                    if(ctr.Name==b.Name)
                    {
                        b.BackColor = ColorTranslator.FromHtml("##FFC246");
                        b.ForeColor = Color.White;
                    }
                    else
                    {
                        ctr.BackColor = ColorTranslator.FromHtml("##FFC246");
                        b.ForeColor = Color.White;
                    }
                }
            }
        }
        private void Arayuz_Load(object sender, EventArgs e)
        {
            pAnasayfa.Visible = true;
            pParca.Visible = false;
            pUrun.Visible = false;
            p1.Height = button5.Height;
            p1.Location = new Point(button5.Location.X-p1.Width,button5.Location.Y);
            Active(button1);
            panel1.BackColor = ColorTranslator.FromHtml("#1F1D28");
            panel2.BackColor = ColorTranslator.FromHtml("#1B1A24");
            dataGridView1.BackColor = ColorTranslator.FromHtml("#1F1D28");
            this.BackColor = ColorTranslator.FromHtml("#1F1D28");
            groupBox2.BackColor = ColorTranslator.FromHtml("#1F1D28");
            groupBox3.BackColor = ColorTranslator.FromHtml("#1F1D28");
            çek();
            timer1.Start();
            tabloekle();
            urun();
       
            Kasaçek();
        }


        void temizle()
        {
            // Tabloyu Temizle //
            dt.Clear();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            try
            {
                if(checkBox1.Checked == true)
                {
                    MessageBox.Show("Lütfen Güncelleme İşaret Tikini Kaldırınız.","Bilgi - LeftSoft",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                else if (textBox1.Text ==""|| textBox2.Text ==""||textBox3.Text =="")
                {
                    MessageBox.Show("Lütfen Alanları Boş Bırakmayınız","HATA - LeftSoft",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                
            else{
            OleDbCommand komut = new OleDbCommand("insert into Satis(Urun_ad,Stok,Fiyat,FiyatAdet) values (@p1,@p2,@p3,@p4)", bgln.sqlbaglan());
            komut.Parameters.AddWithValue("@p1", textBox1.Text);
            komut.Parameters.AddWithValue("@p2", textBox2.Text);
            komut.Parameters.AddWithValue("@p3", Convert.ToInt32(textBox2.Text) * Convert.ToInt32(textBox3.Text));
            komut.Parameters.AddWithValue("@p4", textBox3.Text);
            komut.ExecuteReader();
            MessageBox.Show("Başarıyla Eklendi!", "Otomasyon - LeftSoft", MessageBoxButtons.OK, MessageBoxIcon.Information);
            temizle();
            tabloekle();
            comboBox1.Items.Clear();
            urun();
            dsss.Clear();
            çek();
            bgln.sqlbaglan().Close();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Aynı İsimden Ürün Bulunmakta","HATA - LeftSoft",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox4.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try { 
            OleDbCommand sil = new OleDbCommand("delete from satis where ID=@p1", bgln.sqlbaglan());
            sil.Parameters.AddWithValue("@p1", textBox4.Text);
            sil.ExecuteReader();
            MessageBox.Show("Ürün Silindi","Başarılı - LeftSoft",MessageBoxButtons.OK,MessageBoxIcon.Information);
            temizle();
            tabloekle();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            bgln.sqlbaglan().Close();
            comboBox1.Items.Clear();
            urun();
            dsss.Clear();
            çek();
                }
            catch(Exception)
            {
                MessageBox.Show("Silmek İstediğiniz Ürünün Üzerine Tıklayınız.","HATA - LeftSoft",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try { 
            OleDbCommand guncelle = new OleDbCommand("update satis set Urun_ad=@p1,Stok=@p2,Fiyat=@p3,FiyatAdet=@p4 where ID=" + textBox4.Text, bgln.sqlbaglan());

            
            if(checkBox1.Checked == true)
            {
                guncelle.Parameters.AddWithValue("@p1", textBox1.Text);
                guncelle.Parameters.AddWithValue("@p2", textBox2.Text);
                guncelle.Parameters.AddWithValue("@p3", Convert.ToInt32(textBox2.Text) * Convert.ToInt32(textBox7.Text));
                guncelle.Parameters.AddWithValue("@p4", textBox7.Text);
                guncelle.ExecuteReader();
                MessageBox.Show("Ürün Güncellendi.", "Başarılı - LeftSoft", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
                tabloekle();
                bgln.sqlbaglan().Close();
                comboBox1.Items.Clear();
                urun();
                dsss.Clear();
                çek();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox7.Text = "";
                textBox4.Text = "";
            }
            else
            {
                MessageBox.Show("Lütfen Güncelleme Kutucuğuna Basarak 'Fiyat(Adet) Güncelleme' Yerini Değiştirin","BİLGİ - LeftSoft",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            
                }

            catch (Exception hata)
            {
               
                 if (hata.Message == "'ID=' sorgu ifadesi içindeki Sözdizimi hatası (eksik işleç)")
                {
                    MessageBox.Show("Lütfen Anasayfaya Dönerek Ürün Seçin ve Güncelleme İşlemini Gerçekleştirin.", "HATA - LeftSoft", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                 else if (hata.Message == "Giriş dizesi doğru biçimde değildi.")
                {
                    MessageBox.Show("Lütfen Alanları Doldurduğunuza Emin Olun", "HATA - LeftSoft", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Ooops bir terslik oldu => "+hata.Message, "HATA - LeftSoft", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter("Select * from satis where urun_ad like '%" + textBox5.Text + "%'", bgln.sqlbaglan());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            bgln.sqlbaglan().Close();
        }

        
        void urun()
        {
            OleDbCommand cmd2 = new OleDbCommand("Select * from Satis", bgln.sqlbaglan());
              OleDbDataReader dr2 = cmd2.ExecuteReader();

            while (dr2.Read())
            {
                // KolonAdı //

                comboBox1.Items.Add(dr2["Urun_ad"].ToString());
               

            }
            bgln.sqlbaglan().Close();

        }
        OleDbDataAdapter data;
        DataSet dsss;
        
        void çek()
        {
            data = new OleDbDataAdapter("Select * from Satis", bgln.sqlbaglan());
            dsss = new DataSet();
            data.Fill(dsss);
            
        }

        OleDbDataAdapter dataKasa;
        DataSet dsssKasa;

        void Kasaçek()
        {
            dataKasa = new OleDbDataAdapter("Select * from Kasa where KasaID=1", bgln.sqlbaglan());
            dsssKasa = new DataSet();
            dataKasa.Fill(dsssKasa);
            label14.Text = "Kazanç: " + dsssKasa.Tables[0].Rows[0][1].ToString();
        }
        
        int sayac = 0;
        string id = "";
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                
                if (comboBox1.Text == dsss.Tables[0].Rows[i][1].ToString())
                {
                    id = dsss.Tables[0].Rows[i][0].ToString();
                    sayac = i;
                    
                }
            }
        }
       
        int a = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Lütfen Alanı Boş Geçmeyin", "HATA - LeftSoft", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                
                OleDbCommand guncelle = new OleDbCommand("update satis set Urun_ad=@p1,Stok=@p2,Fiyat=@p3 where ID=" +id, bgln.sqlbaglan());

                guncelle.Parameters.AddWithValue("@p1", comboBox1.Text);
                guncelle.Parameters.AddWithValue("@p2", (Convert.ToInt32(dsss.Tables[0].Rows[sayac][2].ToString()) - Convert.ToInt32(textBox6.Text)));
                guncelle.Parameters.AddWithValue("@p3", (Convert.ToInt32(dsss.Tables[0].Rows[sayac][3].ToString())) - (Convert.ToInt32(dsss.Tables[0].Rows[sayac][4].ToString()) * Convert.ToInt32(textBox6.Text)));
                 
                guncelle.ExecuteReader();
                MessageBox.Show("Ürün Güncellendi.", "Başarılı - LeftSoft", MessageBoxButtons.OK, MessageBoxIcon.Information);

                a = ((Convert.ToInt32(dsss.Tables[0].Rows[sayac][4])) * (Convert.ToInt32(textBox6.Text))) + (Convert.ToInt32(dsssKasa.Tables[0].Rows[0][1]));
               
                MessageBox.Show(a.ToString());
               
                OleDbCommand kasaguncelle = new OleDbCommand("update Kasa set Kasa=@p1 where KasaID=1",bgln.sqlbaglan());
                kasaguncelle.Parameters.AddWithValue("@p1", a.ToString());
                kasaguncelle.ExecuteReader();

                dsss.Clear();
                temizle();
                dsssKasa.Clear();
                Kasaçek();
                tabloekle();
                çek();
                textBox6.Text = "";
                comboBox1.Focus();
                label14.Text = "Kazanç: " + a.ToString();
                bgln.sqlbaglan().Close();
                
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                OleDbDataAdapter data2 = new OleDbDataAdapter("Select * from Satis where ID=" + id, bgln.sqlbaglan());
                DataSet dsss2 = new DataSet();
                data2.Fill(dsss2);
                
                if (Convert.ToInt32(dsss2.Tables[0].Rows[0][2].ToString()) - (Convert.ToInt32(textBox6.Text)) < 0)
                {
                    MessageBox.Show("Stok Adedi Sınırını Geçiyorsunuz.","Bilgi - LeftSoft",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    textBox6.Text = "";
                }
            }
            catch(Exception)
            {
                comboBox1.Focus();
            }
            
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                label3.Text = "Fiyat";
                label3.Location = new Point(337,173);
                textBox7.Enabled = true;
                textBox3.Enabled = false;
            }
            else
            {
                label3.Text = "Fiyat(Adet)";
                label3.Location = new Point(273, 173);
                textBox7.Enabled = false;
                textBox3.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void button9_Click(object sender, EventArgs e)
        {
            pAnasayfa.Visible = false;
            pParca.Visible = false;
            pUrun.Visible = false;
            pKasa.Visible = true;
            position(button9);
            Active(button9);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            pAnasayfa.Visible = false;
            pParca.Visible = false;
            pKasa.Visible = false;
            pUrun.Visible = true;
            position(button6);
            Active(button6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pAnasayfa.Visible = false;
            pUrun.Visible = false;
            pKasa.Visible = false;
            pParca.Visible = true;
            position(button7);
            Active(button7);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pUrun.Visible = false;
            pParca.Visible = false;
            pKasa.Visible = false;
            pAnasayfa.Visible = true;
            position(button5);
            Active(button1);
        }
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        
    }
}
