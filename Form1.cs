using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp15
{
    public partial class Form1 : Form
    {
        private BindingSource bindingSource;
        private Model1 dbContext;  // Tạo trường để tái sử dụng dbContext


        public Form1()
        {
            InitializeComponent();
            bindingSource = new BindingSource();
            dbContext = new Model1();  // Khởi tạo dbContext

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var students = dbContext.Student.ToList();
                bindingSource.DataSource = students;
                dataGridView1.DataSource = bindingSource;
                bindingNavigator1.BindingSource = bindingSource;


                textBox1.DataBindings.Add("Text", bindingSource, "StudentID");
                textBox2.DataBindings.Add("Text", bindingSource, "FullName");
                textBox3.DataBindings.Add("Text", bindingSource, "AverageScore");
                textBox4.DataBindings.Add("Text", bindingSource, "FacultyID");

                bindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dataGridView1.Columns["MajorID"].Visible=false;
            dataGridView1.Columns["Avatar"].Visible = false;

            dataGridView1.Columns["Faculty"].Visible = false;

            dataGridView1.Columns["Major"].Visible = false;

        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            dbContext.Dispose();
            base.OnFormClosed(e);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Model1 dbContext = new Model1())
            {
                try
                {
                    // Kiểm tra xem StudentID có trùng không
                    if (dbContext.Student.Any(s => s.StudentID == textBox1.Text))
                    {
                        MessageBox.Show("StudentID đã tồn tại. Vui lòng nhập StudentID khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Tạo sinh viên mới từ dữ liệu nhập vào
                    var newStudent = new Student
                    {
                        StudentID = textBox1.Text, // Giá trị mới và duy nhất
                        FullName = textBox2.Text,
                        AverageScore = double.TryParse(textBox3.Text, out double avgScore) ? avgScore : 0,
                        FacultyID = int.TryParse(textBox4.Text, out int facultyId) ? (int?)facultyId : null
                    };

                    // Thêm sinh viên vào dbContext và lưu vào cơ sở dữ liệu
                    dbContext.Student.Add(newStudent);
                    dbContext.SaveChanges();

                    // Cập nhật lại dữ liệu cho BindingSource và DataGridView
                    bindingSource.DataSource = dbContext.Student.ToList();
                    bindingSource.ResetBindings(false);

                    MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm sinh viên: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Model1 dbContext = new Model1())
            {
                try
                {
                    // Lấy sinh viên đang được chọn từ BindingSource
                    Student currentStudent = bindingSource.Current as Student;
                    if (currentStudent != null)
                    {
                        // Tìm sinh viên trong dbContext bằng StudentID
                        Student studentToUpdate = dbContext.Student.Find(currentStudent.StudentID);
                        if (studentToUpdate != null)
                        {
                            // Cập nhật thuộc tính của sinh viên từ dữ liệu nhập vào, không thay đổi StudentID
                            studentToUpdate.FullName = textBox2.Text;
                            studentToUpdate.AverageScore = double.TryParse(textBox3.Text, out double avgScore) ? avgScore : 0;
                            studentToUpdate.FacultyID = int.TryParse(textBox4.Text, out int facultyId) ? (int?)facultyId : null;

                            // Lưu thay đổi vào cơ sở dữ liệu
                            dbContext.SaveChanges();

                            // Cập nhật lại dữ liệu cho BindingSource và DataGridView
                            bindingSource.DataSource = dbContext.Student.ToList();
                            bindingSource.ResetBindings(false);

                            MessageBox.Show("Sửa thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không có sinh viên nào được chọn để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi sửa sinh viên: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (Model1 dbContext = new Model1())
            {
                Student currentEmloyee = (Student)bindingSource.Current;
                Student emloyeeToDelete = dbContext.Student.Find(currentEmloyee.StudentID);
                if ((emloyeeToDelete != null))
                {
                    dbContext.Student.Remove(emloyeeToDelete);
                    dbContext.SaveChanges();
                    bindingSource.DataSource = dbContext.Student.ToList();
                }
                {
                    
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bindingSource.MoveNext();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            bindingSource.MovePrevious();

        }
    }
    
}
