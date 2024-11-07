//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data.SqlClient;
//using System.Windows.Forms;
//using static UnityEngine.Rendering.DebugUI;

//namespace TPIntegrador
//{
//    internal class AccesoBD
//    {
//        internal class AccesoDB
//        {
//            private string connectionString = "Data Source=334-06-71385;Initial Catalog=ParcialPrimero;Integrated Security=True;Encrypt=False;";

//            public bool Ok()
//            {
//                SqlConnection connection = new SqlConnection(connectionString);
//                try
//                {
//                    connection.Open();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error al conectarse: " + ex.Message);
//                    return false;
//                }
//                finally
//                {
//                    connection.Close();
//                }
//                return true;
//            }

//            public class Player
//            {
//                public int id { get; set; }
//                public string Name { get; set; }
//                public string Password { get; set; }
//                public int points { get; set; }
//            }

//            public class Points
//            {
//                public int id;
//                public int points;
//            }

//            public class SaveGame
//            {
//                public int id { get; set; }
//                public bool LifePowerup { get; set; }
//                public bool SpeedShootPowerup { get; set; }
//                public bool DamagePowerup { get; set; }
//                public int AmountAmmo { get; set; }
//                public int Round { get; set; }
//            }

//            public List<Player> GetTopPlayers()
//            {
//                List<Player> PlayerList = new List<Player>();

//                SqlConnection connection = new SqlConnection(connectionString);
//                try
//                {
//                    connection.Open();
//                    SqlCommand command = new SqlCommand("SELECT PlayerTable.id, PlayerTable.Name, PointsTable.Points FROM PlayerTable JOIN PointsTable ON PlayerTable.id=PointsTable.idPlayer ORDER BY PointsTable.Points DESC OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;", connection);
//                    SqlDataReader reader = command.ExecuteReader();
//                    while (reader.Read())
//                    {
//                        Player Player = new Player();
//                        Player.id = reader.GetInt32(0);
//                        Player.Name = reader.GetString(1);
//                        Player.points = reader.GetInt32(2);

//                        PlayerList.Add(Player);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error de lectura de datos: " + ex.Message);
//                }
//                finally
//                {
//                    connection.Close();
//                }

//                return PlayerList;
//            }

//            public void RegisterPlayer(string name, int password)
//            {
//                SqlConnection connection = new SqlConnection(connectionString);

//                try
//                {
//                    connection.Open();
//                    SqlCommand cmd = new SqlCommand("INSERT INTO PlayerTable (Name, Password) VALUES" + "(@name, @password)", connection);
//                    cmd.Parameters.AddWithValue("@nombre", name);
//                    cmd.Parameters.AddWithValue("@password", password);
//                    cmd.ExecuteNonQuery();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("Error al registrarse: " + ex.Message);
//                }
//                finally
//                {
//                    connection.Close();
//                }
//            }

//            //public Player GetUsers(int id)
//            //{
//            //    Player usuario = new Player();
//            //    SqlConnection connection = new SqlConnection(connectionString);
//            //    try
//            //    {
//            //        connection.Open();
//            //        SqlCommand command = new SqlCommand("SELECT id, Nombre, DNI, Sexo, Domicilio FROM Persona" + " WHERE id=@id", connection);
//            //        command.Parameters.AddWithValue("@id", id);
//            //        SqlDataReader reader = command.ExecuteReader();

//            //        while (reader.Read()) //por si llega a venir con valores nulos
//            //        {
//            //            usuario.Id = reader.GetInt32(0);
//            //            usuario.Nombre = reader.GetString(1);
//            //            usuario.DNI = reader.GetInt32(2);
//            //            usuario.Sexo = reader.GetString(3);
//            //            usuario.DomicilioID = reader.GetInt32(4);
//            //        }
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        MessageBox.Show("Error de lectura de datos: " + ex.Message);
//            //    }
//            //    finally
//            //    {
//            //        connection.Close();
//            //    }

//            //    return usuario;
//            //}

//            //public void UpdateUser(string nombre, int dni, string sexo, int domicilio, int Id)
//            //{
//            //    SqlConnection connection = new SqlConnection(connectionString);
//            //    try
//            //    {
//            //        connection.Open();
//            //        SqlCommand cmd = new SqlCommand("UPDATE Persona SET Nombre=@nombre, DNI=@dni, Sexo=@sexo, Domicilio=@domicilio WHERE id=@id", connection);
//            //        cmd.Parameters.AddWithValue("@nombre", nombre);
//            //        cmd.Parameters.AddWithValue("@dni", dni);
//            //        cmd.Parameters.AddWithValue("@sexo", sexo);
//            //        cmd.Parameters.AddWithValue("@domicilio", domicilio);
//            //        cmd.Parameters.AddWithValue("@id", Id);
//            //        cmd.ExecuteNonQuery();
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        MessageBox.Show("Error al actualizar: " + ex.Message);
//            //    }
//            //    finally
//            //    {
//            //        connection.Close();
//            //    }
//            //}

//            //public void DeleteUser(int Id)
//            //{
//            //    SqlConnection connection = new SqlConnection(connectionString);
//            //    try
//            //    {
//            //        connection.Open();
//            //        SqlCommand cmd = new SqlCommand("DELETE FROM Persona WHERE id=@id", connection);
//            //        cmd.Parameters.AddWithValue("@id", Id);
//            //        cmd.ExecuteNonQuery();
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        MessageBox.Show("Error al eliminar: " + ex.Message);
//            //    }
//            //    finally
//            //    {
//            //        connection.Close();
//            //    }
//            //}

//            //public List<Domicilio> GetDomicilios()
//            //{
//            //    List<Domicilio> domicilios = new List<Domicilio>();

//            //    SqlConnection connection = new SqlConnection(connectionString);
//            //    try
//            //    {
//            //        connection.Open();
//            //        SqlCommand command = new SqlCommand("SELECT id, Calle, Numero, CodPostal, Localidad FROM Domicilio", connection);
//            //        SqlDataReader reader = command.ExecuteReader();
//            //        while (reader.Read())
//            //        {
//            //            Domicilio domicilio = new Domicilio();
//            //            domicilio.Id = reader.GetInt32(0);
//            //            domicilio.Calle = reader.GetString(1);
//            //            domicilio.Numero = reader.GetInt32(2);
//            //            domicilio.CodPostal = reader.GetInt32(3);
//            //            domicilio.Localidad = reader.GetString(4);
//            //            domicilios.Add(domicilio);
//            //        }
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        MessageBox.Show("Error de lectura de datos: " + ex.Message);
//            //    }
//            //    finally
//            //    {
//            //        connection.Close();
//            //    }

//            //    return domicilios;
//            //}

//            //public Domicilio GetDomicilios(int id)
//            //{
//            //    Domicilio domicilio = new Domicilio();
//            //    SqlConnection connection = new SqlConnection(connectionString);
//            //    try
//            //    {
//            //        connection.Open();
//            //        SqlCommand command = new SqlCommand("SELECT id, Nombre, DNI, Sexo, Domicilio FROM Persona" + " WHERE id=@id", connection);
//            //        command.Parameters.AddWithValue("@id", id);
//            //        SqlDataReader reader = command.ExecuteReader();

//            //        while (reader.Read()) //por si llega a venir con valores nulos
//            //        {
//            //            domicilio.Id = reader.GetInt32(0);
//            //            domicilio.Calle = reader.GetString(1);
//            //            domicilio.Numero = reader.GetInt32(2);
//            //            domicilio.CodPostal = reader.GetInt32(3);
//            //            domicilio.Localidad = reader.GetString(4);
//            //        }
//            //    }
//            //    catch (Exception ex)
//            //    {
//            //        MessageBox.Show("Error de lectura de datos: " + ex.Message);
//            //    }
//            //    finally
//            //    {
//            //        connection.Close();
//            //    }

//            //    return domicilio;
//            //}
//        }
//    }
//}
