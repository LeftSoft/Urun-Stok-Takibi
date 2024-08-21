using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;
using System.IO;
namespace Otomasyon_LeftSoft
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlBaglantisi bgl = new SqlBaglantisi();
        private void button1_Click(object sender, EventArgs e)
        {
            
            OleDbCommand komut = new OleDbCommand("Select * From Tbl_Giris where K_ADİ=@p1 and K_SİFRE=@p2", bgl.sqlbaglan());
            komut.Parameters.AddWithValue("@p1", bunifuMaterialTextbox1.Text);
            komut.Parameters.AddWithValue("@p2", bunifuMaterialTextbox2.Text);
            OleDbDataReader dr = komut.ExecuteReader();
            // Veri Okuma İşlemi //
            if (dr.Read())
            {
                MessageBox.Show("Giriş Başarılı","Başarılı - LeftSoft",MessageBoxButtons.OK,MessageBoxIcon.Information);
                Arayuz frm = new Arayuz();
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı Kullanıcı veya Şifre", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                
            }
            bgl.sqlbaglan().Close();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml("#1F1D28");
            bunifuMaterialTextbox1.BackColor = ColorTranslator.FromHtml("#1F1D28");
            bunifuMaterialTextbox2.BackColor = ColorTranslator.FromHtml("#1F1D28");
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

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            OleDbCommand komut = new OleDbCommand("Select * From Tbl_Giris where K_ADİ=@p1 and K_SİFRE=@p2", bgl.sqlbaglan());
            komut.Parameters.AddWithValue("@p1", bunifuMaterialTextbox1.Text);
            komut.Parameters.AddWithValue("@p2", bunifuMaterialTextbox2.Text);
            OleDbDataReader dr = komut.ExecuteReader();
            // Veri Okuma İşlemi //
            if (dr.Read())
            {
                MessageBox.Show("Giriş Başarılı", "Başarılı - LeftSoft", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Arayuz frm = new Arayuz();
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı Kullanıcı veya Şifre", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            bgl.sqlbaglan().Close();
        }
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
