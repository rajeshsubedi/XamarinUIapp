using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamApp
{
    public partial class MainPage : ContentPage
    {

        public class mysqlList
        {
            public int Id { get; set; }
            public string  Name { get; set; }
            public string Body { get; set; }
        }


        SqlConnection sqlConnection;
        public MainPage()
        {
            InitializeComponent();
            string srvrdbname = "mydb";
            string srvrname = "192.168.1.126";
            string srvrusername = "Rajesh";
            string srvrpassword = "12345";
            string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                //string srvrdbname = "mydb";
                //string srvrname = "192.168.1.73";
                //string srvrusername = "samir";
                //string srvrpassword = "123456";

                //string sqlconn = $"Data Source={srvrname};Initial Catalog={srvrdbname};User ID={srvrusername};Password={srvrpassword}";
                //SqlConnection sqlConnection = new SqlConnection(sqlconn);
                sqlConnection.Open();
                await App.Current.MainPage.DisplayAlert("Alert", "Connection Established", "Ok");
                sqlConnection.Close();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
           
        }

        private async void getbutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                List<mysqlList> mysqlLists = new List<mysqlList>();
                sqlConnection.Open();
                string queryString = "Select * from dbo.mysql";
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    mysqlLists.Add(new mysqlList
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Body = reader["Body"].ToString(),
                    }
                    );
                }
                reader.Close();
                sqlConnection.Close();

                MyListView.ItemsSource = mysqlLists;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert",ex.Message,"Ok");
                throw;
            }
        }

        private async void postbutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();
                //using (SqlCommand command = new SqlCommand("INSERT INTO dbo.mysql VALUES(@Id, @Name , @Body)",sqlConnection))
                //{
                //    command.Parameters.Add(new SqlParameter("Id", UserId.Text));
                //    command.Parameters.Add(new SqlParameter("Name", UserTitle.Text));
                //    command.Parameters.Add(new SqlParameter("Body", UserBody.Text));
                //    command.ExecuteNonQuery();
                //}
                using (var sqlCommand = new SqlCommand("SELECT count(*) FROM dbo.mysql WHERE Id=1", sqlConnection))
                {

                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Activated", "Ok");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Alert", "Not activated", "Ok");
                    }

                    reader.Close();
                    reader.Dispose();
                }

                sqlConnection.Close();

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }



        }

        private async void updatebutton_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();

                int IdtoBeUpdated = Convert.ToInt32(UserId.Text);
                string NameTobeUpdated = UserTitle.Text;
                string BodyTobeUpdated = UserBody.Text;


                string qerystr = $"UPDATE dbo.mysql SET Id='{IdtoBeUpdated}' , Name ='{NameTobeUpdated}' , Body ='{BodyTobeUpdated}' WHERE Id='{IdtoBeUpdated}'";

                using (SqlCommand command = new SqlCommand(qerystr, sqlConnection))
                {
                    command.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                await App.Current.MainPage.DisplayAlert("Alert","Congrats you have successfully updated the row item", "Ok");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }
        }

        private async void deletebutton_Clicked(object sender, EventArgs e)
        {

            try
            {

                sqlConnection.Open();
                int idtodelete = Convert.ToInt32(UserId.Text);
                using (SqlCommand command = new SqlCommand($"Delete FROM dbo.mysql WHERE Id = {idtodelete} ", sqlConnection))
                {
                    command.ExecuteNonQuery();
                }
                sqlConnection.Close();
                await App.Current.MainPage.DisplayAlert("Alert", "Congrats you have successfully deleted the row item", "Ok");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }

        }
    }
}
