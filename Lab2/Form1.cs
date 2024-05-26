using Lab2.objects;
using Lab2.Utils;

namespace Lab2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dInput.Items.Add(0.1);
            dInput.Items.Add(0.01);
            dInput.Items.Add(0.001);
            dInput.Items.Add(0.0001);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void bInput_ValueChanged(object sender, EventArgs e)
        {

        }

        private void aInput_ValueChanged(object sender, EventArgs e)
        {

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (aInput.Value == null || dInput.SelectedItem == null || bInput.Value == null || nInput.Value == null)
            {
                MessageBox.Show("Nie wszystkie pola zosta³y uzupe³nione", "B³¹d");
            } 
            else
            {

                if ((int)aInput.Value >= (int)bInput.Value)
                {
                    MessageBox.Show("Niepoprawny przedzia³", "B³¹d");
                } 
                else
                {
                    List<Osobnik> osobniks = new List<Osobnik>();
                    for (int i = 1; i <= nInput.Value; i++)
                    {
                        osobniks.Add(new Osobnik(i, (int)aInput.Value, (int)bInput.Value, (double)dInput.SelectedItem));
                    }
                    SelectionUtils.SetUpFitValue(osobniks);
                    SelectionUtils.SetUpDistribuator(osobniks);
                    SelectionUtils.SetUpNewOsobnikAfterSelection(osobniks);
                    osobniki.DataSource = osobniks;
                }
            }
        }
    }
}