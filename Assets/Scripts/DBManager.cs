using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Data;
using static UnityEngine.Rendering.DebugUI;
using System.Data.SqlClient;
using TMPro;


public class DBManager : MonoBehaviour
{    
    public static DBManager Instance { get; private set; }
    public PlayerData LoggedInPlayerData { get; private set; }

    private string connectionString;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = "DESKTOP-T12P6AE";
        builder.InitialCatalog = "ProgAvanzadaBD";
        builder.UserID = "sa";
        builder.Password = "sa";
        builder.IntegratedSecurity = false;

        connectionString = builder.ConnectionString;
    }

    public void Ok()
    {

        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            Debug.Log("funciona");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    public string RegisterUser(string username, string password)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();

            // Verificar si el usuario ya existe
            string checkUserQuery = "SELECT COUNT(*) FROM Player WHERE Username = @Username";
            SqlCommand checkCommand = new SqlCommand(checkUserQuery, connection);
            checkCommand.Parameters.AddWithValue("@Username", username);

            int userCount = (int)checkCommand.ExecuteScalar();

            if (userCount > 0)
            {
                return "Ese usuario ya existe"; // Usuario ya existe
            }

            // Insertar nuevo usuario
            string insertUserQuery = "INSERT INTO Player (Username, Password) VALUES (@Username, @Password); SELECT SCOPE_IDENTITY();";
            SqlCommand insertCommand = new SqlCommand(insertUserQuery, connection);
            insertCommand.Parameters.AddWithValue("@Username", username);
            insertCommand.Parameters.AddWithValue("@Password", password);

            // Obtener el ID del nuevo usuario
            int newUserId = Convert.ToInt32(insertCommand.ExecuteScalar());

            if (newUserId > 0)
            {
                // Insertar registro inicial en PlayerPoints
                string insertPointsQuery = "INSERT INTO PlayerPoints (id_player, Score) VALUES (@id_player, 0)";
                SqlCommand pointsCommand = new SqlCommand(insertPointsQuery, connection);
                pointsCommand.Parameters.AddWithValue("@id_player", newUserId);
                pointsCommand.ExecuteNonQuery();

                // Insertar registro inicial en SaveGame con valores predeterminados
                string insertSaveGameQuery = @"
                INSERT INTO SaveGame (id_player, LifePowerup, DamagePowerup, AmountAmmo, Round, CurrentLife)
                VALUES (@id_player, 0, 0, 1, 1, 100)";
                SqlCommand saveGameCommand = new SqlCommand(insertSaveGameQuery, connection);
                saveGameCommand.Parameters.AddWithValue("@id_player", newUserId);
                saveGameCommand.ExecuteNonQuery();

                return "Usuario registrado con éxito";
            }
            else
            {
                return "Error al registrar el usuario";
            }
        }
        catch (SqlException e)
        {
            Debug.LogError("Error en la base de datos: " + e.Message);
            return "Error de conexión a la base de datos";
        }
        finally
        {
            // Cerrar la conexión en el bloque finally para garantizar su cierre
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }


    public string LoginUser(string username, string password)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();

            // Comprobar las credenciales
            string loginQuery = "SELECT Id FROM Player WHERE Username = @Username AND Password = @Password";
            SqlCommand loginCommand = new SqlCommand(loginQuery, connection);
            loginCommand.Parameters.AddWithValue("@Username", username);
            loginCommand.Parameters.AddWithValue("@Password", password);

            object result = loginCommand.ExecuteScalar();
            if (result != null)
            {
                int playerId = (int)result;

                LoadPlayerData(playerId);

                return "Inicio de sesión exitoso";
            }
            else
            {
                return "Usuario o contraseña incorrectos";
            }
        }
        catch (SqlException e)
        {
            Debug.LogError("Error en la base de datos: " + e.Message);
            return "Error de conexión a la base de datos";
        }
        finally
        {
            connection.Close();
        }
    }

    private void LoadPlayerData(int playerId)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();

            // Consulta para obtener los datos del jugador
            string query = @"
                SELECT p.Username, pp.Score, sg.LifePowerup, sg.DamagePowerup, sg.AmountAmmo, sg.Round, sg.CurrentLife 
                FROM Player p
                JOIN PlayerPoints pp ON p.Id = pp.id_player
                JOIN SaveGame sg ON p.Id = sg.id_player
                WHERE p.Id = @Id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", playerId);

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                // Crear y asignar los datos del jugador a CurrentPlayer
                LoggedInPlayerData = new PlayerData
                {
                    UserId = playerId,
                    Username = reader["Username"].ToString(),
                    HighScore = (int)reader["Score"],
                    LifePowerup = (bool)reader["LifePowerup"],
                    DamagePowerup = (bool)reader["DamagePowerup"],
                    AmountAmmo = (int)reader["AmountAmmo"],
                    Round = (int)reader["Round"],
                    CurrentLife = (int)reader["CurrentLife"]
                };
            }
            reader.Close();
        }
        catch (SqlException e)
        {
            Debug.LogError("Error al cargar los datos del jugador: " + e.Message);
        }
        finally
        {
            connection.Close();
        }
    }

    public int GetHighScore(string username)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                
                string highScoreQuery = @"
                    SELECT MAX(Score)
                    FROM PlayerPoints
                    WHERE id_player = (SELECT id FROM Player WHERE Username = @Username)";

                SqlCommand highScoreCommand = new SqlCommand(highScoreQuery, connection);
                highScoreCommand.Parameters.AddWithValue("@Username", username);

                object result = highScoreCommand.ExecuteScalar();
                return result != null && result != DBNull.Value ? (int)result : 0;
            }
            catch (SqlException e)
            {
                Debug.LogError("Error al obtener la puntuación: " + e.Message);
                return 0;
            }
        }
    }

    public void UpdateHighScore(string username, int newScore)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Primero, obtenemos el puntaje máximo actual del jugador
                string getHighScoreQuery = @"
                SELECT MAX(Score)
                FROM PlayerPoints
                WHERE id_player = (SELECT id FROM Player WHERE Username = @Username)";

                SqlCommand getHighScoreCommand = new SqlCommand(getHighScoreQuery, connection);
                getHighScoreCommand.Parameters.AddWithValue("@Username", username);

                object result = getHighScoreCommand.ExecuteScalar();
                int currentHighScore = result != null && result != DBNull.Value ? (int)result : 0;

                // Si el nuevo puntaje es mayor, actualizamos la tabla
                if (newScore > currentHighScore)
                {
                    string updateScoreQuery = @"
                    INSERT INTO PlayerPoints (id_player, Score)
                    VALUES ((SELECT id FROM Player WHERE Username = @Username), @NewScore)";

                    SqlCommand updateScoreCommand = new SqlCommand(updateScoreQuery, connection);
                    updateScoreCommand.Parameters.AddWithValue("@Username", username);
                    updateScoreCommand.Parameters.AddWithValue("@NewScore", newScore);

                    updateScoreCommand.ExecuteNonQuery();
                    Debug.Log("Puntuación máxima actualizada con éxito");
                }
                else
                {
                    Debug.Log("El nuevo puntaje no supera el puntaje máximo actual");
                }
            }
            catch (SqlException e)
            {
                Debug.LogError("Error al actualizar el puntaje: " + e.Message);
            }
        }
    }
}

