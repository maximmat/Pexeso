using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pexeso
{
    internal class Karticka : Button //třída kartička s dědičností z button
    {
        public int actHodnota { get; private set; } //získání hodnoty actHodnota a nastavení pro zápis jen v této třídě
        public bool shoda { get; set; } //načtení proměné shoda s možností zápisu

        public Karticka(int hodnota)
        //nastavení vygenreovaného čísla pro novou kartičku a prázdný text na kartičcce
        {
            actHodnota = hodnota;
            Text = "";

        }

        public void Otocit()
        {
            if (shoda == true) //pokud je shoda kartičku skryjeme 
            {
                this.Visible = false;
            }
            else
            {
                if (Text == "") //pokud je kartička otočená, tj text není tak na ni vykreslíme její hodnotu
                {
                    Text = actHodnota.ToString();
                }
                else //jinak je požadavek na otočení kartičky zpět, takže text tlačítka nastavíme na nic
                {
                    Text = "";
                }
            }
        }
            
    }
}
