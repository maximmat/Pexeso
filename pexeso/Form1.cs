using System.Runtime.CompilerServices;

namespace pexeso
{
    public partial class Form1 : Form
    {
        private TableLayoutPanel herniPlocha;
        private List<Karticka> vybraneKarticky = new List<Karticka>();

        public Form1()
        {
            InitializeComponent();
            herniPlocha = new TableLayoutPanel //vytvo�en� hern� plochy s maticov�m uspo��dan�m prvk�
            {
                Size = new Size(900, 900),
                Location = new Point(25, 25),
            };
            Controls.Add(herniPlocha);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void GenerujTlacitka(int velikost)
        {
            button1.Visible = false;
            numericUpDown1.Visible = false;
            label1.Visible = false;

            herniPlocha.Controls.Clear(); //smaz�n� dosavadn�ch prvk� na maticov� hern� plo�e
            vybraneKarticky.Clear();

            List<int> cisla = Enumerable.Range(1, (velikost * velikost) / 2).SelectMany(x => new[] { x, x }).ToList();
            /*
             * Enumerable.Range - vytvo�en� seznamu od 1 do polovinu celkov�ho po�tu karti�ek
             * SelectMany - vytvo�� ke ka�d�mu prvku stejnou dvojci 
             */

            Random rand = new Random();
            for (int i = cisla.Count - 1; i > 0; i--) //proch�zen� listu od zadu
            {
                int j = rand.Next(i + 1); //vybere n�hodn� index mezi za��tkem a aktu�ln�m prvkem i v seznamu
                (cisla[i], cisla[j]) = (cisla[j], cisla[i]); //prohod� je
            }

            int index = 0;

            for (int r = 0; r < velikost; r++)
            {
                for (int s = 0; s < velikost; s++)
                {
                    var actKarticka = new Karticka(cisla[index++]); //vytvo�en� tla��tka a p�i�azen� hodnoty z listu 
                    actKarticka.Click += kartickaVybrana; //umo�n�n� kliknut� na vygenerovanou karti�ku, p�i kliknut� vol� kartickaVybrana
                    herniPlocha.Controls.Add(actKarticka, r, s);
                }
            }

        }

        private void kartickaVybrana(object sender, EventArgs e)
        {

            if (!(sender is Karticka karticka) || karticka.shoda || vybraneKarticky.Count >= 2) return;

            if (vybraneKarticky.Contains(karticka)) return; //zamezen� kliknut� na stejn� tla��tko dvakr�t

            karticka.Otocit();
            vybraneKarticky.Add(karticka);

            if (vybraneKarticky.Count == 2)
            {
                if (vybraneKarticky[0].actHodnota == vybraneKarticky[1].actHodnota)
                {
                    vybraneKarticky.ForEach(k =>
                    {
                        k.shoda = true;
                        timer1.Tick += (s, ev) =>
                        {
                            k.Otocit();
                            vybraneKarticky.Clear();
                            timer1.Stop();
                        };
                        timer1.Start();
                        
                    });
                    
                }
                else
                {
                    timer1.Tick += (s, ev) =>
                    {
                        vybraneKarticky.ForEach(k => k.Otocit());
                        vybraneKarticky.Clear();
                        timer1.Stop();
                    };
                    timer1.Start();
                }
            }

            if (herniPlocha.Controls.OfType<Karticka>().All(k => k.shoda))
            {
                konecHry();
            }
        }

        private void konecHry()
        {
            herniPlocha.Controls.Clear();
            vybraneKarticky.Clear();
            

            label1.Visible = true;
            numericUpDown1.Visible = true;
            button1.Visible = true;
            label1.Text = "GG Alkane";


        }

        private void button1_Click(object sender, EventArgs e)
        {
            herniPlocha.ResumeLayout();
            GenerujTlacitka((int)numericUpDown1.Value);
            herniPlocha.SuspendLayout();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }


}
