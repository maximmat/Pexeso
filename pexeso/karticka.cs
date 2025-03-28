using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pexeso
{
    internal class Karticka : Button
    {
        public int actHodnota { get; private set; }
        public bool shoda { get; set; }

        public Karticka(int hodnota)
        {
            actHodnota = hodnota;
            Text = "";

        }

        public void Otocit()
        {
            if (shoda == true)
            {
                this.Visible = false;
            }
            else
            {
                if (Text == "")
                {
                    Text = actHodnota.ToString();
                }
                else
                {
                    Text = "";
                }
            }
        }
            
    }
}
