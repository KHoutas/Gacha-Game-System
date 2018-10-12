using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient; 

namespace Gacha_Game
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        string connectionString;
        int userId;
        Random rand = new Random();
        List<Card> userCollection = new List<Card>();

        public Form1()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["Gacha_Game.Properties.Settings.GachaDatabaseConnectionString"].ConnectionString;
        }

        private void loadCollection(int userId)
        {

            MessageBox.Show(userId.ToString());

            string query = "SELECT a.Name FROM CardList a " +
                "INNER JOIN AccountCards b ON a.Id = b.CardListId " +
                "WHERE b.AccountId = @AccountId";

            int cardId;
            Rarity rarity;
            Image cardImage;
            string cardName;
            bool isInside = false;

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@AccountId", userId);
                
                SqlDataReader dataReader = command.ExecuteReader();

                int count = 0;
                    while (dataReader.Read())
                    {
                        count++;

                    cardId = dataReader.GetInt32(0);

                    //MessageBox.Show("Test1");
                    //
                    //cardName = dataReader.GetString(1);
                    //
                    //MessageBox.Show("Test2");
                    //
                    //if (userCollection.Count != 0)
                    //{
                    //    for (int i = 0; i <= userCollection.Count; i++)
                    //    {
                    //        MessageBox.Show(i.ToString());
                    //        if (userCollection[i].CardName == cardName)
                    //        {
                    //            userCollection[i].IncrementCount();
                    //            isInside = true;
                    //        }
                    //    }
                    //}
                    //
                    //if (isInside != true)
                    //{
                    //    cardId = dataReader.GetInt32(0);
                    //
                    //    if (dataReader.GetString(2) == "Common")
                    //    {
                    //        rarity = Rarity.Common;
                    //
                    //        cardImage = Image.FromFile("Card Images\\Common\\" + cardName + ".png");
                    //    }
                    //    else if (dataReader.GetString(2) == "Uncommon")
                    //    {
                    //        rarity = Rarity.Uncommon;
                    //
                    //        cardImage = Image.FromFile("Card Images\\Uncommon\\" + cardName + ".png");
                    //    }
                    //    else if (dataReader.GetString(2) == "Rare")
                    //    {
                    //        rarity = Rarity.Rare;
                    //
                    //        cardImage = Image.FromFile("Card Images\\Rare\\" + cardName + ".png");
                    //    }
                    //    else if (dataReader.GetString(2) == "Epic")
                    //    {
                    //        rarity = Rarity.Epic;
                    //
                    //        cardImage = Image.FromFile("Card Images\\Epic\\" + cardName + ".png");
                    //    }
                    //    else if (dataReader.GetString(2) == "Legendary")
                    //    {
                    //        rarity = Rarity.Legendary;
                    //
                    //        cardImage = Image.FromFile("Card Images\\Legendary\\" + cardName + ".png");
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Not even possible...");
                    //        cardImage = null;
                    //        rarity = Rarity.Error;
                    //    }
                    //
                    //    userCollection.Add(new Card(cardImage, cardName, cardId, rarity));
                    //
                    //    isInside = false;
                    //}
                }
                MessageBox.Show(count.ToString());
            }

            

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text != string.Empty || txtPassword.Text != string.Empty)
            {
                string username = txtUserName.Text.ToLower();
                string password = txtPassword.Text;

                RegisterAccount(username, password);

                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;

                MessageBox.Show("Account successfully created.");
            }   
            else
            {
                MessageBox.Show("One or more fields were not filled out correctly...");
            }
            
        }

        private void RegisterAccount(string username, string password)
        {
            string query = "INSERT INTO Account VALUES (@UserName, @Password)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);

                command.ExecuteNonQuery();
            }

            setUpProfile();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text != string.Empty || txtUserName.Text != string.Empty)
            {
                string username = txtUserName.Text.ToLower();
                string password = txtPassword.Text;

                Login(username, password);
                loadCollection(userId);
            }
            else
            {
                MessageBox.Show("One or more fields were not filled out correctly...");
            }

            //PopulateCardsListBox();
        }

        private void Login(string username, string password)
        {
            string query = "SELECT * FROM Account WHERE User_Name = @Username";
            string query2 = "SELECT * FROM AccountInformation WHERE Id = @Id";

            using (connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlCommand command2 = new SqlCommand(query2, connection);

                connection.Open();

                command.Parameters.AddWithValue("@Username", username);
                
                SqlDataReader dataReader = command.ExecuteReader();
                
                if(dataReader.Read())
                {
                    if (dataReader.GetString(2) == password)
                    {
                        MessageBox.Show("Successfully logged in!");

                        userId = dataReader.GetInt32(0);

                        tabControl.SelectedIndex = 1;

                        lblWelcome.Text = "Welcome, " + dataReader.GetString(1);

                        dataReader.Close();

                        command2.Parameters.AddWithValue("@Id", userId);

                        dataReader = command2.ExecuteReader();

                        if (dataReader.Read())
                        {
                            lblCurrency.Text = dataReader.GetInt32(1).ToString();
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Your username or password is incorrect.");
                    }
                }
                else
                {
                    MessageBox.Show("Your username or password is incorrect.");
                }
            }
        }

        private void setUpProfile()
        {
            string query = "INSERT INTO AccountInformation VALUES (2500, 1, 0)";
            string query2 = "SELECT TOP 1 * FROM Account ORDER BY ID DESC";

            int accountId = 0;

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlCommand command2 = new SqlCommand(query2, connection);

                SqlDataReader dataReader;

                command.ExecuteNonQuery();

                dataReader = command2.ExecuteReader();

                if (dataReader.Read())
                {
                    accountId = dataReader.GetInt32(0);
                }
            }

        }

        private void btnSummon_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = 2;
            PopulateCardsListBox();
        }

        private void btnViewCollection_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = 3;
            DisplayCards();
        }

        private void SummonCards()
        {
            int RNGesus = rand.Next(1, 201);
            Rarity rarity;
            string cardRarity = string.Empty;
            int cardId = 0;
            string cardName;
            Image cardImage;
            bool cardIn = false;

            if (RNGesus >= 1 && RNGesus <= 100)
            {
                cardRarity = "Common";
                rarity = Rarity.Common;
            }
            else if (RNGesus > 100 && RNGesus <= 150)
            {
                cardRarity = "Uncommon";
                rarity = Rarity.Uncommon;
            }
            else if (RNGesus > 150 && RNGesus <= 175)
            {
                cardRarity = "Rare";
                rarity = Rarity.Rare;
            }
            else if (RNGesus > 175 && RNGesus <= 190)
            {
                cardRarity = "Epic";
                rarity = Rarity.Epic;
            }
            else if (RNGesus > 190 && RNGesus <= 200)
            {
                cardRarity = "Legendary";
                rarity = Rarity.Legendary;
            }
            else
            {
                cardRarity = "ERROR";
                MessageBox.Show("wtf");
                rarity = Rarity.Error;
            }

            string query = "SELECT TOP 1 * FROM CardList WHERE Rarity = @Rarity ORDER BY RAND()";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@Rarity", cardRarity);

                SqlDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {
                    cardName = dataReader.GetString(1);
                    cardId = dataReader.GetInt32(0);

                    if (userCollection.Count != 0)
                    {
                        for (int i = 0; i < userCollection.Count; i++)
                        {
                            if (userCollection[i].CardName == cardName)
                            {
                                userCollection[i].IncrementCount();

                                cardIn = true;
                            }
                        }
                    }

                    cardImage = Image.FromFile("Card Images\\" + cardRarity + "\\" + cardName + ".png");
                    picSummon.Image = cardImage;
                    MessageBox.Show(rarity + " card!\n" + cardName);
                    picSummon.Image = null;

                    if (!cardIn)
                    {
                        userCollection.Add(new Card(cardImage, cardName, cardId, rarity));
                        cardIn = false;
                    }
                    AddCards(cardId, cardName);


                }
            }
        }

        private void AddCards(int cardId, string cardName)
        {
            string query = "INSERT INTO AccountCards VALUES (@AccountId, @CardListId)";

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@AccountId", userId);
                command.Parameters.AddWithValue("@CardListId", cardId);
                command.ExecuteNonQuery();
            }

            //PopulateCardsListBox();
        }

        private void PopulateCardsListBox()
        {
            string query = "SELECT a.Name FROM CardList a " +
                "INNER JOIN AccountCards b ON a.Id = b.CardListId " +
                "WHERE b.AccountId = @AccountId";
        
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@AccountId", userId);
        
                DataTable cardTable = new DataTable();
                adapter.Fill(cardTable);
        
                listTest.DisplayMember = "Name";
                listTest.ValueMember = "Id";
                listTest.DataSource = cardTable;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SummonCards();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userId = 0;
            userCollection.Clear();
            tabControl.SelectedIndex = 0;
        }

        private void btnSummonNormal_Click(object sender, EventArgs e)
        {
            SummonCards();
            PopulateCardsListBox();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void btnBackSummon_Click(object sender, EventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

        private void btnBackCollect_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.tabControl.SelectedTab.Controls)
            {
                if (c.GetType().ToString() == "System.Windows.Forms.PictureBox")
                {
                    PictureBox box = c as PictureBox;
                    if (box.Image != null)
                    {
                        box.Image = null;
                    }
                }

                if (c.GetType().ToString() == "System.Windows.Forms.Label")
                {
                    Label label = c as Label;
                    if (label.Text != "0")
                    {
                        label.Text = "0";
                    }
                }

                
            }

            tabControl.SelectedIndex = 1;

        }

        private void DisplayCards()
        {
            userCollection.Sort((x, y) => x.CardName.CompareTo(y.CardName));
            
            for (int i = 0; i < userCollection.Count; i++)
            {
                userCollection[i].LabelName = i + 1;
            }

            int counter = 0;

            foreach (Control c in this.tabControl.SelectedTab.Controls)
            {
                if (c.GetType().ToString() == "System.Windows.Forms.PictureBox")
                {

                    if (counter < userCollection.Count)
                    {
                        PictureBox box = c as PictureBox;
                        if (box != null)
                        {
                            box.Image = userCollection[counter].CardImage;

                            foreach (Control cc in this.tabControl.SelectedTab.Controls)
                            {
                                if (cc.Name == "label" + userCollection[counter].LabelName)
                                {
                                    cc.Text = userCollection[counter].Count.ToString();
                                }
                            }
                            

                            counter++;
                        }
                    }
                }
            }
        }
        
    }
}
