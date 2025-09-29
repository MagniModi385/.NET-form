using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {
        NpgsqlConnection con;
        public Form2()
        {
            InitializeComponent();
            try
            {
                con = new NpgsqlConnection("server=localhost;port=5432;database=******;user id=postgres;password=******");
                MessageBox.Show("Connection Successful");
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("A");
            comboBox1.Items.Add("B");
            comboBox1.Items.Add("C");
            comboBox1.Items.Add("D");
            comboBox1.Items.Add("E");
            comboBox1.Items.Add("F");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from students", con);
            DataTable dt=new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private bool ValidateInput() 
        {
            if (textBox1.Text.Length < 3) 
            {
                MessageBox.Show("Name must be at least 3 characters long.");
                return false;
            }
            if (Convert.ToInt32(textBox2.Text) < 0) 
            {
                MessageBox.Show("Age must be a positive integer.");
                return false;
            }
            if (comboBox1.SelectedItem == null) 
            {
                MessageBox.Show("Please select a grade.");
                return false;
            }
            if (!radioButton1.Checked && !radioButton2.Checked) 
            {
                MessageBox.Show("Please select an enrollment status.");
                return false;
            }
            DateTime selectedDate = dateTimePicker1.Value;
            DateTime today = DateTime.Today;
            if (selectedDate > today)
            { 
            MessageBox.Show("Enrollment date cannot be in the past.");
                return false;
            } 
            return true;
        }
       private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateInput());
            else
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("insert into students(student_name,age,grade,enroll_status,enroll_date,student_id) values(@sname,@age,@grade,@enroll_stat,@enroll_date,@s_id)", con);
                    cmd.Parameters.AddWithValue("sname", textBox1.Text);
                    cmd.Parameters.AddWithValue("age", Convert.ToInt32(textBox2.Text));
                    cmd.Parameters.AddWithValue("grade", comboBox1.SelectedItem.ToString());
                    if (radioButton1.Checked) cmd.Parameters.AddWithValue("enroll_stat", true);
                    else cmd.Parameters.AddWithValue("enroll_stat", false);
                    cmd.Parameters.AddWithValue("enroll_date", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("s_id", Convert.ToInt32(textBox3.Text));
                    int result = cmd.ExecuteNonQuery();
                    if (result == 1) MessageBox.Show("Record Inserted");
                    else MessageBox.Show("Error in inserting record");
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from students", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) ;
            else
            {
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("update students set student_name=@sname,age=@age,grade=@grade,enroll_status=@enroll_stat,enroll_date=@enroll_date where student_id=@sid", con);
                    cmd.Parameters.AddWithValue("sname", textBox1.Text);
                    cmd.Parameters.AddWithValue("age", Convert.ToInt32(textBox2.Text));
                    cmd.Parameters.AddWithValue("grade", comboBox1.SelectedItem.ToString());
                    if (radioButton1.Checked) cmd.Parameters.AddWithValue("enroll_stat", true);
                    else cmd.Parameters.AddWithValue("enroll_stat", false);
                    cmd.Parameters.AddWithValue("enroll_date", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("sid", Convert.ToInt32(textBox3.Text));
                    int result=cmd.ExecuteNonQuery();
                    if (result == 1) MessageBox.Show("Record Updated");
                    else MessageBox.Show("Error in updating record");
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from students", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("delete from students where student_id=@sid",con);
            int rowInd = dataGridView1.SelectedCells[5].RowIndex;
            int selectedId= Convert.ToInt32(dataGridView1.Rows[rowInd].Cells["student_id"].Value);
            cmd.Parameters.AddWithValue("sid",selectedId);
            int r=cmd.ExecuteNonQuery();
            if (r == 1) MessageBox.Show("Record Deleted");
            else MessageBox.Show("Error in deleting record");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("select * from students", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            NpgsqlDataAdapter da=new NpgsqlDataAdapter("select * from students where student_name=@sname",con);
                da.SelectCommand.Parameters.AddWithValue("sname", textBox4.Text);
                DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
