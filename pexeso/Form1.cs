using System.Runtime.CompilerServices;

namespace pexeso
{
    public partial class Form1 : Form
    {
        private TableLayoutPanel herniPlocha;
        private List<Karticka> vybraneKarticky = new List<Karticka>(); //list pro kartièky, tlaèítka

        public Form1()
        {
            InitializeComponent();
            herniPlocha = new TableLayoutPanel //vytvoøení herní plochy s maticovým uspoøádaním prvkù
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
            //schování tlaèítek pro nastavování pøi høe
            button1.Visible = false;
            numericUpDown1.Visible = false;
            label1.Visible = false;

            herniPlocha.Controls.Clear(); //smazání dosavadních prvkù na maticové herní ploše
            vybraneKarticky.Clear();

            List<int> cisla = Enumerable.Range(1, (velikost * velikost) / 2).SelectMany(x => new[] { x, x }).ToList();
            /*
             * Enumerable.Range - vytvoøení seznamu od 1 do polovinu celkového poètu kartièek
             * SelectMany - vytvoøí ke každému prvku stejnou dvojci 
             * Nakonec všechny cisla do listu
             */

            Random rand = new Random();
            for (int i = cisla.Count - 1; i > 0; i--) //procházení listu od zadu
            {
                int j = rand.Next(i + 1); //vybere náhodný index mezi zaèátkem a aktuálním prvkem i v seznamu
                (cisla[i], cisla[j]) = (cisla[j], cisla[i]); //prohodí je
            }

            int index = 0;

            for (int r = 0; r < velikost; r++)
            {
                for (int s = 0; s < velikost; s++)
                {
                    var actKarticka = new Karticka(cisla[index++]); //vytvoøení tlaèítka a pøiøazení hodnoty z listu 
                    actKarticka.Click += kartickaVybrana; //umožnìní kliknutí na vygenerovanou kartièku, pøi kliknutí volá kartickaVybrana
                    herniPlocha.Controls.Add(actKarticka, r, s); //vykreselní kartièky a umístìní podle aktuálního øádku a sloupce
                }
            }

        }

        private void kartickaVybrana(object sender, EventArgs e)
        {

            if (!(sender is Karticka karticka) || vybraneKarticky.Count >= 2) return; //kontrola zda sender je kartièka jiná než již otoèená, kontrola zda není vybráno víc než dvì kartièky

            if (vybraneKarticky.Contains(karticka)) return; //zamezení kliknutí na stejné tlaèítko dvakrát

            karticka.Otocit(); //otoèení kartièky øešené v class kartièka
            vybraneKarticky.Add(karticka); //pøidání vybrané kartièky do listu

            if (vybraneKarticky.Count == 2)
            {
                if (vybraneKarticky[0].actHodnota == vybraneKarticky[1].actHodnota) //kontrola shody otoèených kartièek
                {
                    //otoèení všech kartièek v listu a nasetování shody
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
                    //otoèení kaartièek
                    timer1.Tick += (s, ev) => //timer1 pøidání funkce co pøi tick
                    {
                        vybraneKarticky.ForEach(k => k.Otocit());
                        vybraneKarticky.Clear();
                        timer1.Stop();
                    };
                    timer1.Start();
                }
            }

            if (herniPlocha.Controls.OfType<Karticka>().All(k => k.shoda)) //kontrola zda nemají již všechny kartièky nasetovanou promìnou shoda
            {
                konecHry();
            }
        }

        private void konecHry()
        //vyèištìní herní plochy, listu a zobrazení ovládacích prvkù pro novou hru
        {
            herniPlocha.Controls.Clear();
            vybraneKarticky.Clear();
            
            numericUpDown1.Visible = true;
            button1.Visible = true;
            label1.Visible = true;
            MessageBox.Show("GG Alkane");



        }

        private void button1_Click(object sender, EventArgs e) //tlaèítko pro generování herního pole
        {
            herniPlocha.ResumeLayout(); //mùže být aktivní SuspendLayout z minulé hry
            GenerujTlacitka((int)numericUpDown1.Value);
            herniPlocha.SuspendLayout(); //ošetøení, aby se neposouvalo rozložení kartièek, když je nìjaký prázdný øádek, zpùsobovalo zmatení pøi hraní
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }


}
