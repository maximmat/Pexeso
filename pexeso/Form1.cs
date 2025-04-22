using System.Runtime.CompilerServices;

namespace pexeso
{
    public partial class Form1 : Form
    {
        private TableLayoutPanel herniPlocha;
        private List<Karticka> vybraneKarticky = new List<Karticka>(); //list pro karti�ky, tla��tka

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
            //schov�n� tla��tek pro nastavov�n� p�i h�e
            button1.Visible = false;
            numericUpDown1.Visible = false;
            label1.Visible = false;

            herniPlocha.Controls.Clear(); //smaz�n� dosavadn�ch prvk� na maticov� hern� plo�e
            vybraneKarticky.Clear();

            List<int> cisla = Enumerable.Range(1, (velikost * velikost) / 2).SelectMany(x => new[] { x, x }).ToList();
            /*
             * Enumerable.Range - vytvo�en� seznamu od 1 do polovinu celkov�ho po�tu karti�ek
             * SelectMany - vytvo�� ke ka�d�mu prvku stejnou dvojci 
             * Nakonec v�echny cisla do listu
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
                    herniPlocha.Controls.Add(actKarticka, r, s); //vykreseln� karti�ky a um�st�n� podle aktu�ln�ho ��dku a sloupce
                }
            }

        }

        private void kartickaVybrana(object sender, EventArgs e)
        {

            if (!(sender is Karticka karticka) || vybraneKarticky.Count >= 2) return; //kontrola zda sender je karti�ka jin� ne� ji� oto�en�, kontrola zda nen� vybr�no v�c ne� dv� karti�ky

            if (vybraneKarticky.Contains(karticka)) return; //zamezen� kliknut� na stejn� tla��tko dvakr�t

            karticka.Otocit(); //oto�en� karti�ky �e�en� v class karti�ka
            vybraneKarticky.Add(karticka); //p�id�n� vybran� karti�ky do listu

            if (vybraneKarticky.Count == 2)
            {
                if (vybraneKarticky[0].actHodnota == vybraneKarticky[1].actHodnota) //kontrola shody oto�en�ch karti�ek
                {
                    //oto�en� v�ech karti�ek v listu a nasetov�n� shody
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
                    //oto�en� kaarti�ek
                    timer1.Tick += (s, ev) => //timer1 p�id�n� funkce co p�i tick
                    {
                        vybraneKarticky.ForEach(k => k.Otocit());
                        vybraneKarticky.Clear();
                        timer1.Stop();
                    };
                    timer1.Start();
                }
            }

            if (herniPlocha.Controls.OfType<Karticka>().All(k => k.shoda)) //kontrola zda nemaj� ji� v�echny karti�ky nasetovanou prom�nou shoda
            {
                konecHry();
            }
        }

        private void konecHry()
        //vy�i�t�n� hern� plochy, listu a zobrazen� ovl�dac�ch prvk� pro novou hru
        {
            herniPlocha.Controls.Clear();
            vybraneKarticky.Clear();
            
            numericUpDown1.Visible = true;
            button1.Visible = true;
            label1.Visible = true;
            MessageBox.Show("GG Alkane");



        }

        private void button1_Click(object sender, EventArgs e) //tla��tko pro generov�n� hern�ho pole
        {
            herniPlocha.ResumeLayout(); //m��e b�t aktivn� SuspendLayout z minul� hry
            GenerujTlacitka((int)numericUpDown1.Value);
            herniPlocha.SuspendLayout(); //o�et�en�, aby se neposouvalo rozlo�en� karti�ek, kdy� je n�jak� pr�zdn� ��dek, zp�sobovalo zmaten� p�i hran�
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }


}
