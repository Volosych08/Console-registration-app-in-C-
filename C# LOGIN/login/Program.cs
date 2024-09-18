using System;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Xml;

class login {
    public static void Main(String[] args)
    {
        Console.WriteLine("Wellcome to somthing who has regestration system.");
        DbConnect();
        WellcomeStartChoice();

    }

   static void DbConnect(){
        string dbPath = "Data Source=users.db;Version=3;";
        // data base path
        // Console.WriteLine("Database Path: " + System.IO.Path.GetFullPath("users.db"));
    }

    static void WellcomeStartChoice(){
        string choice;
        bool CorectChoice = false;
        Console.WriteLine("Welcome! (console app alredy start)");
        do{
            Console.Write("1.Login-In \n2.Sign-Up \nChoose your choice: ");
            choice = Console.ReadLine();
            if(choice == "1"){
                logIn();
                CorectChoice=true;
                break;
            }
            if(choice == "2"){
                signUp();
                CorectChoice=true;
                break;
            }
            else{
                Console.WriteLine("Incorrect choice! Try again.");
            }
        }
        while(CorectChoice == false);
    }

    static void logIn(){
        string userName,password,option;
        bool isUserNameCorrect=false, isPasswordCorrect=false, IsUserLogined=false;
        string dbPath = "Data Source=users.db;Version=3;";

        using (SQLiteConnection conn = new SQLiteConnection(dbPath)){
            conn.Open();
             while (!isUserNameCorrect)
        {
            Console.Write("Enter your username: ");
            userName = Console.ReadLine();

            // Query to check if the username exists
            string query = "SELECT * FROM Users WHERE userName = @userName";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@userName", userName);

                  using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {   
                        string storedHashedPassword = reader["userPassword"].ToString();
                        while(!isPasswordCorrect)
                        {
                            Console.Write("Enter your password: ");
                            password = Console.ReadLine();
                            
                            // Verify password using BCrypt
                            if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                            {
                                isUserNameCorrect = true;
                                isPasswordCorrect = true;
                                Console.WriteLine("Login successful");
                                Console.WriteLine("Welcome back! " + userName);
                                menu();
                            }
                            else
                            {
                                Console.WriteLine("Your password is incorrect.\nTry again.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Username not found, try again.");
                        while(!IsUserLogined)
                        {
                            Console.Write("Do you already have an account?\nY/N: ");
                            option = Console.ReadLine().ToLower();
                            switch(option)
                            {
                                case "y":
                                    logIn();
                                    isUserNameCorrect = false;
                                    IsUserLogined = true;
                                    break;
                                case "n":
                                    IsUserLogined = true;
                                    Console.WriteLine("Please sign up!");
                                    signUp();
                                    break;
                                default:
                                    Console.WriteLine("Incorrect option, try again!");
                                    break;
                            }
                        }

                    }
                }
            }
        }
        //end of username and password search
        }//sql con
    }

    static void hashPassword(){

    }

    static void printUser(string userSearch){
        string dbPath = "Data Source=users.db;Version=3;";
        using (SQLiteConnection conn = new SQLiteConnection(dbPath))
        {
            conn.Open();

            // Query to search for a specific user by name
            string query = "SELECT * FROM Users WHERE userName = @userName";

            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@userName", userSearch);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["id"]}: {reader["userName"]}, {reader["password"]}");
                    }
                }
            }
        }

    }
    static void signUp(){
        bool provePassword=false;
        string userName, password, repeatPassword;
        string dbPath = "Data Source=users.db;Version=3;";
        Console.Write("Enter your username: ");
        userName=Console.ReadLine();

        while (!provePassword)
        {
        Console.Write("Enter your password: ");
        password = Console.ReadLine();
        Console.Write("Repeat your password: ");
        repeatPassword = Console.ReadLine();

        if (password == repeatPassword)
        {
            // Hash the password using BCrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                // Insert username and hashed password into the Users table
                string insertUserQuery = "INSERT INTO Users (userName, userPassword) VALUES (@userName, @userPassword)";
                using (SQLiteCommand insertCmd = new SQLiteCommand(insertUserQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@userName", userName);
                    insertCmd.Parameters.AddWithValue("@userPassword", hashedPassword);
                    insertCmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("Hi " + userName + "!");
            provePassword = true;
            menu();
        }
        else
        {
            Console.WriteLine("Incorrect password, try again!");
        }
        }
    }
        
    static void UserChek(){
        string dbPath = "Data Source=users.db;Version=3;";
        using (SQLiteConnection conn = new SQLiteConnection(dbPath))
            {
                conn.Open();

                string query = "SELECT * FROM Users";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["id"]} {reader["userName"]} {reader["userPassword"]}");
                        }
                    }
                }
            }       
    }
    static void menu(){
        Console.WriteLine("HI in menu!");
    }
}