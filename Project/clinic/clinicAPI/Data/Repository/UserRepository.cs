using System.Data.SqlClient;
using clinicAPI.Common;
using clinicAPI.Data.Model;

namespace clinicAPI.Data.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Method to get all users
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Users", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserID = (int)reader["UserID"],
                            Role = reader["Role"].ToString(),
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Mobile = reader["Mobile"].ToString()
                        });
                    }
                }
            }
            return users;
        }

        // Method to add a new user
        public void AddUser(User user)
        {
            var PasswordHash = Utilities.PasswordHash(user);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("INSERT INTO Users (Username, PasswordHash, Email, Role, FirstName, LastName,Mobile) VALUES (@Username, @PasswordHash, @Email, @Role, @FirstName, @LastName,@Mobile)", connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@PasswordHash", PasswordHash);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                command.Parameters.AddWithValue("@Mobile", user.Mobile);

                command.ExecuteNonQuery();
            }
        }

        // Method to get a user by ID
        public User GetUserById(int userId)
        {
            User user = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE UserID = @UserID", connection);
                command.Parameters.AddWithValue("@UserID", userId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserID = (int)reader["UserID"],
                            Role = reader["Role"].ToString(),
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Mobile = reader["Mobile"].ToString()
                        };
                    }
                }
            }
            return user;
        }
        public User GetUserByMobile(string Mobile)
        {
            User user = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Mobile = @Mobile", connection);
                command.Parameters.AddWithValue("@Mobile", Mobile);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserID = (int)reader["UserID"],
                            Role = reader["Role"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Mobile = reader["Mobile"].ToString()
                        };
                    }
                }
            }
            return user;
        }

    }
}
